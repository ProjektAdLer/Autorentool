
/*

import {H5PStandalone} from "h5p-standalone";

export function javaScriptHalloWorldFunction() {
    console.log("hello java script world");
    return "hallo java script world";
}

 export function test(containerId) {
    const el = document.getElementById(containerId);
    const options = {
        h5pJsonPath: './h5p-folder',
        frameJs: './frame.bundle.js',
        frameCss: './styles/h5p.css',
   };
    new H5PStandalone(el, options);
}

*/

/*
window.javaScriptHalloWorldFunction = function () {
    console.log("hello java script world");
    return "hallo java script world";
}
*/
window.testH5P = function (containerId)
{

    const el = document.getElementById("h5p-container");
    const options = {
       // h5pJsonPath:  './H5pStandalone/h5p-folder/1_Start/h5p.json',
        h5pJsonPath:  '/localhost/H5pStandalone/h5p-folder/AbfrageDefinitionen/h5p.json',
        frameJs: '/H5pStandalone/frame.bundle.js',
        frameCss: '/H5pStandalone/styles/h5p.css',
    }
    new H5PStandalone.H5P(el, options);
    return "hallo java script world";
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