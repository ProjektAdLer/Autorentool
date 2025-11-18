# Stage 1: Build the application
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH

WORKDIR /src

# Install Node.js for npm dependencies
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash - && \
    apt-get install -y nodejs --no-install-recommends && apt-get clean

# Copy project files
COPY . .

# Restore .NET tools and install electron-sharp
RUN dotnet tool restore

# Install npm dependencies and run Tailwind build
WORKDIR /src/AuthoringTool
RUN npm install && npm run tailwind-build

# Build the application based on target architecture
RUN if [ "$TARGETARCH" = "amd64" ]; then \
        dotnet electron-sharp build /target linux; \
    elif [ "$TARGETARCH" = "arm64" ]; then \
        dotnet electron-sharp build /target custom "linux-arm64;linux" /electron-arch arm64; \
    else \
        dotnet electron-sharp build /target linux; \
    fi

# Stage 2: Runtime image
FROM ubuntu:24.04

RUN apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y \
    socat curl xvfb libgtk-3-0 libnss3 libasound2t64 ca-certificates \
    # zlib1g-dev is required for arm64
    zlib1g-dev \
    --no-install-recommends && \
    rm -rf /var/lib/apt/lists/* &&  \
    apt-get clean

WORKDIR /app

# Copy the built AppImage from build stage
COPY --from=build /src/AuthoringTool/bin/Desktop_Publish/*.AppImage ./authoring-tool.AppImage

RUN chmod +x authoring-tool.AppImage && \
    ./authoring-tool.AppImage --appimage-extract && \
    mv squashfs-root/* . && \
    rm -rf squashfs-root authoring-tool.AppImage

EXPOSE 8002

HEALTHCHECK --interval=5s --timeout=3s --start-period=60s --retries=3 CMD curl -f http://localhost:8001 || exit 1

CMD ["sh", "-c", "rm -f /tmp/.X*-lock && socat TCP-LISTEN:8002,fork TCP:127.0.0.1:8001 & xvfb-run -e /dev/stdout ./authoring-tool --no-sandbox"]