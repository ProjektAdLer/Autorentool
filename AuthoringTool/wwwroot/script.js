function subscribeToResize(dotNetReference) {
    console.log("subscribeToResize aufgerufen"); // Debugging-Ausgabe
    window.addEventListener("resize", () => {
        const dimensions = getScreenDimensions();
        console.log("Resize-Event ausgelöst: ", dimensions); // Debugging-Ausgabe
        dotNetReference.invokeMethodAsync("OnResize", dimensions);
    });
}

function getScreenDimensions() {
    const dimensions = {
        width: window.innerWidth,
        height: window.innerHeight,
    };
    console.log("getScreenDimensions aufgerufen: ", dimensions); // Debugging-Ausgabe
    return dimensions;
}
