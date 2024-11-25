// General information: 
// We cant use JavaScript 6 modules
// if we use JavaScript modules we get error: 
// Failed to load module script: Expected a JavaScript 
// module script but the server responded with a MIME type of
// "text/html". Strict MIME type checking is enforced for
// module scripts per HTML spec

window.onload = function () {

    let h5pInstance = null;
    let externalDispatcher = null;
    let xAPICompletedListener = null;


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
        setupEventListenersAndCallbackToDotNet();
    }
         
    // can throw exceptions
    window.terminateH5pStandalone = async function () {
        const el = document.getElementById("h5p-container");
        resetHtmlContainerOfH5p(el);
        localStorage.clear();
        sessionStorage.clear();
        releaseEventListeners();
        window.location.reload();
        H5PIntegration.contents = {};
        resetH5pInstance();
        console.log("Container completely removed and rebuilt.");
    };

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

  
    function setupEventListenersAndCallbackToDotNet() {
        
        if (h5pInstance && !externalDispatcher) {
            
            externalDispatcher = h5pInstance.externalDispatcher;

            if (externalDispatcher) {
                
                externalDispatcher.on("xAPI", onXAPIEvent);
                xAPICompletedListener = onXAPIEvent; 
            } else {
                console.error("External dispatcher is not available.");
            }
        }
    }

    function onXAPIEvent(event) {
        console.log("xAPI event received:", event);
        const jsonData = JSON.stringify(event);
        DotNet.invokeMethodAsync('H5pPlayer.BusinessLogic.Api.JavaScript', 'ReceiveJsonData', jsonData);
    }


    function releaseEventListeners() {
        if (externalDispatcher && xAPICompletedListener) {
            externalDispatcher.off("xAPI", xAPICompletedListener);
            console.log("xAPI Event listeners successfully removed.");
        }
        externalDispatcher = null;
        xAPICompletedListener = null;
    }
}

