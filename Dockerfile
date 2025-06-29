FROM ubuntu:24.04

RUN apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y \
    socat curl xvfb libgtk-3-0 libnss3 libasound2t64 ca-certificates \
    # zlib1g-dev is required for arm64
    zlib1g-dev \
    --no-install-recommends && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY AuthoringTool/bin/Desktop_Publish/*.AppImage ./authoring-tool.AppImage

RUN chmod +x authoring-tool.AppImage && \
    ./authoring-tool.AppImage --appimage-extract && \
    mv squashfs-root/* . && \
    rm -rf squashfs-root authoring-tool.AppImage

EXPOSE 8002

HEALTHCHECK --interval=5s --timeout=3s --start-period=60s --retries=3 CMD curl -f http://localhost:8001 || exit 1

CMD ["sh", "-c", "rm -f /tmp/.X*-lock && socat TCP-LISTEN:8002,fork TCP:127.0.0.1:8001 & xvfb-run -e /dev/stdout ./authoring-tool --no-sandbox"]