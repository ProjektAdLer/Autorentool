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
    window.terminateH5pStandalone = async function () {
        const el = document.getElementById("h5p-container");
        resetHtmlContainerOfH5p(el);
        localStorage.clear();
        sessionStorage.clear();
        H5PIntegration.contents = {};
        resetH5pInstance();
        window.location.reload();
        console.log("Container completely removed and rebuilt.");
    };
    
    


    // can throw exceptions
    window.displayH5p = async function (h5pJsonPath) {

        console.log("displayH5p javascript function called with path:", h5pJsonPath);
        resetH5pInstance();
        await buildH5pToDisplay(h5pJsonPath);
    }

    async function buildH5pToDisplay(h5pJsonPath) {

        const el = document.getElementById("h5p-container");

        if (el) {
            const options = CreateH5pOptions(h5pJsonPath);
            try {
                await createH5pInstanceToDisplay(el, options);
            } catch (error) {
                console.error("Error during instantiation of H5P:", error);
                throw  error;
            }

        } else {
            console.error("Can not find H5P-Container element.");
        }
    }
    
    async function createH5pInstanceToDisplay(el, options) {

        h5pInstance = new H5PStandalone.H5P(el, options);
        await h5pInstance;
    }
    
    
    
    // can throw exceptions
    window.validateH5p = async function (h5pJsonPath) {

        console.log("validateH5p javascript function called with path:", h5pJsonPath);
        resetH5pInstance();
        await buildH5pToValidate(h5pJsonPath);
    }

    async function buildH5pToValidate(h5pJsonPath) {

        const el = document.getElementById("h5p-container");

        if (el) {
            const options = CreateH5pOptions(h5pJsonPath);
            try {
                await createH5pInstanceToValidate(el,options);
            } catch (error) {
                console.error("Error during instantiation of H5P:", error);
                throw  error;
            }

        } else {
            console.error("Can not find H5P-Container element.");
        }
    }

    async function createH5pInstanceToValidate(el, options) {

        h5pInstance = new H5PStandalone.H5P(el, options).then(function () {
            H5P.externalDispatcher.on("xAPI", (event) => {
                console.log("xAPI event", event);

                // Konvertiere das JavaScript-Event in ein JSON-String
                const jsonData = JSON.stringify(event);

                // JSON-Daten an die .NET-Methode senden  
                DotNet.invokeMethodAsync('H5pPlayer', 'ReceiveJsonData', jsonData);
            });
        });
        await h5pInstance;
        // frage an Phil ist raus: warum wird der completed in 3d gebraucht
        // h5pInstance.xAPICompletedListener = xAPICompletedListener;
    }

    
    function CreateH5pOptions(h5pJsonPath) {
        const options = {
            h5pJsonPath: h5pJsonPath,
            frameJs: '//localhost:8001/H5pStandalone/frame.bundle.js',
            frameCss: '//localhost:8001/H5pStandalone/styles/h5p.css',
        }
        return options;
    }

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

