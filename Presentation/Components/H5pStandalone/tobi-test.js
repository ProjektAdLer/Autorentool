


import { H5PStandalone } from "./h5p-standalone.js";

export function javaScriptHalloWorldFunction() {
    console.log("hello java script world");
    return "hallo java script world";
}

export function test(containerId) {
    const el = document.getElementById(containerId);
    const options = {
        frameJs: './frame.bundle.js',
        frameCss: './styles/h5p.css',
    };
    new H5PStandalone(el, options);
}

// function javaScriptHalloWorldFunction()
// {
//     console.log("hello java script world");
//     return "hallo java script world";
// }
//
// function test(containerId)
// {
//
//     const el = document.getElementById(containerId);
//     const options = {
//         // h5pJsonPath:  '/h5p-folder',
//         frameJs: '../wwwroot/H5pStandalone/frame.bundle.js',
//         frameCss: '../wwwroot/H5pStandalone/styles/h5p.css',
//     }
//     new H5PStandalone(el, options);
// }
//
// window.javaScriptHalloWorldFunction = javaScriptHalloWorldFunction;
// window.test = test;