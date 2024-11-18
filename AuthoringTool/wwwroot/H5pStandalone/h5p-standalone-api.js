// General information: 
// We cant use JavaScript 6 modules
// if we use JavaScript modules we get error: 
// Failed to load module script: Expected a JavaScript 
// module script but the server responded with a MIME type of
// "text/html". Strict MIME type checking is enforced for
// module scripts per HTML spec

window.onload = function () {

    let h5pInstance = null;



    // can throw exceptions
    window.displayH5p = async function (h5pJsonPath) {

        console.log("displayH5p javascript function called with path:", h5pJsonPath);
        resetH5pInstance();
        await buildH5p(h5pJsonPath);
    }

    // can throw exceptions
    window.validateH5p = async function (h5pJsonPath) {

        console.log("validateH5p javascript function called with path:", h5pJsonPath);
        resetH5pInstance();
        await buildH5p(h5pJsonPath);


        // H5PStandalone.prototype.externalDispatcher.on("xAPI", (event) => {
        //     console.log("xAPI event", event);
        //
        //     // Konvertiere das JavaScript-Event in ein JSON-String
        //     const jsonData = JSON.stringify(event);
        //
        //     // JSON-Daten an die .NET-Methode senden  
        //   //  DotNet.invokeMethodAsync('Presentation', 'ReceiveJsonData', jsonData);
        // });
        //
        // H5PStandalone.H5P.xAPICompletedListener = xAPICompletedListener;
    }

    async function buildH5p(h5pJsonPath) {

        const el = document.getElementById("h5p-container");

        if (el) {
            const options = {
                h5pJsonPath: h5pJsonPath,
                frameJs: '//localhost:8001/H5pStandalone/frame.bundle.js',
                frameCss: '//localhost:8001/H5pStandalone/styles/h5p.css',
            }

            try {
                await createH5pInstance(el, options);
            } catch (error) {
                console.error("Error during instantiation of H5P:", error);
                throw  error;
            }

        } else {
            console.error("Can not find H5P-Container element.");
        }
    }

    async function createH5pInstance(el, options) {

        h5pInstance = new H5PStandalone.H5P(el, options);
        await h5pInstance;
    }


    // can throw exceptions
    window.terminateH5pStandalone = async function () {
        const el = document.getElementById("h5p-container");
        resetHtmlContainerOfH5p(el);
        localStorage.clear();
        sessionStorage.clear();
        window.location.reload();
        H5PIntegration.contents = {};
        resetH5pInstance();
        console.log("Container completely removed and rebuilt.");
    };


    function resetH5pInstance() {
        if (h5pInstance) {
            h5pInstance = null;
        }
    }

    function resetHtmlContainerOfH5p(el) {
        if (el) {
            el.innerHTML = "";
        }
    }
}

