

window.testH5P2 = function (containerId)
{

    const el = document.getElementById("h5p-container");
    const options = {
       // h5pJsonPath:  './H5pStandalone/h5p-folder/1_Start/h5p.json',
        h5pJsonPath:  '/localhost/H5pStandalone/h5p-folder/AbfrageDefinitionen/h5p.json',
        frameJs: '/H5pStandalone/frame.bundle.js',
        frameCss: '/H5pStandalone/styles/h5p.css',
    }
    new H5PStandalone.H5P(el, options);
    
}


window.testH5P =async function (h5pJsonPath)
{
    const fs = require('fs');
    const el = document.getElementById("h5p-container");


    const options = {
        // h5pJsonPath:  './H5pStandalone/h5p-folder/1_Start/h5p.json',
        // h5pJsonPath:  '//localhost:8086/courses/3/h5p/6c324c62-1557-4e28-a1f7-c2df227c3c8c.h5p',
        // h5pJsonPath:  '//localhost:8001/H5pStandalone/h5p-folder/1_Start',
        //  h5pJsonPath:  '//localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen',
        h5pJsonPath:  h5pJsonPath,
        frameJs: '//localhost:8001/H5pStandalone/frame.bundle.js',
        frameCss: '//localhost:8001/H5pStandalone/styles/h5p.css',
    }
    await new H5PStandalone.H5P(el, options);

    H5P.externalDispatcher.on("xAPI", (event) => {
        console.log("xAPI event", event);

        // Konvertiere das JavaScript-Event in ein JSON-String
        const jsonData = JSON.stringify(event);

        // JSON-Daten an die .NET-Methode senden  
        DotNet.invokeMethodAsync('Presentation', 'ReceiveJsonData', jsonData);
    });

    H5P.xAPICompletedListener = xAPICompletedListener;
    
    
    return "123testH5P called123";
}

// from: https://github.com/ProjektAdLer/2D_3D_AdLer/blob/main/src/Components/Core/Presentation/React/LearningSpaceDisplay/LearningElementModal/LearningElementModalController.ts#L95
function xAPICompletedListener(t) {
    if ((t.getVerb() === "completed" || t.getVerb() === "answered") &&
        !t.getVerifiedStatementValue(["context", "contextActivities", "parent"])) 
    {
        let n = t.getScore(),
            r = t.getMaxScore(),
            i = t.getVerifiedStatementValue([
                "object",
                "definition",
                "extensions",
                "http://h5p.org/x-api/h5p-local-content-id",
            ]);

        e.setFinished(i, n, r);  
    }
}





/*
 function javaScriptHalloWorldFunction()
 {
     console.log("hello java script world");
     return "hallo java script world";
 }

 function test(containerId)
 {

     const el = document.getElementById(containerId);
     const options = {
         h5pJsonPath:  '/h5p-folder',
         frameJs: '../wwwroot/H5pStandalone/frame.bundle.js',
         frameCss: '../wwwroot/H5pStandalone/styles/h5p.css',
     }
     new H5PStandalone(el, options);
 }

 window.javaScriptHalloWorldFunction = javaScriptHalloWorldFunction;
 window.test = test;
 */