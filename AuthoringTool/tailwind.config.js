/** @type {import('tailwindcss').Config} */
const colors = require("tailwindcss/colors");
module.exports = {
    important: true,
    content: ["../**/*.razor", "../**/*.cshtml", "../**/*.html"],
    theme: {
        screens: {
            'sm': '640px',
            'md': '768px',
            'lg': '1024px',
            'xl': '1280px',
            '2xl': '1536px',
            '1080p': '1900px',
            '2500p': '2500px',
        },
        extend: {
            colors: {
                //Bei Custom Farben geben die Zahlenwerte die Saturation Prozentwerte wieder (700 = 70%)
                babylonbg: "#33334d", //Babylon Default Hintergrundfarbe
                blacktrans: {
                    100: "rgba(0,0,0,0.1)",
                    200: "rgba(0,0,0,0.2)",
                    300: "rgba(0,0,0,0.3)",
                    400: "rgba(0,0,0,0.4)",
                    500: "rgba(0,0,0,0.5)", //50% schwarz fÃ¼r Modal-Hintergrund
                    600: "rgba(0,0,0,0.6)",
                    700: "rgba(0,0,0,0.7)",
                    800: "rgba(0,0,0,0.8)",
                    900: "rgba(0,0,0,0.9)",
                },
                adlerblue: {
                    DEFAULT: "rgb(69,160,229)", // DEFAULT = 700
                    100: "rgb(207,220,229)", // hell
                    200: "rgb(184,210,229)",
                    300: "rgb(161,200,229)",
                    400: "rgb(138,190,229)",
                    500: "rgb(115,180,229)",
                    600: "rgb(92,170,229)",
                    700: "rgb(69,160,229)",
                    800: "rgb(46,150,229)",
                    900: "rgb(23,140,229)", // dunkel
                },
                adlerdarkblue: {
                    DEFAULT: "rgb(23,45,77)", //DEFAULT = 300
                    100: "rgb(8,35,77)", // hell
                    200: "rgb(15,40,77)",
                    300: "rgb(23,45,77)",
                    400: "rgb(31,49,77)",
                    500: "rgb(38,54,77)",
                    600: "rgb(46,58,77)",
                    700: "rgb(54,63,77)",
                    800: "rgb(61,67,77)",
                    900: "rgb(69,72,77)", // dunkel
                },
                adlergold: {
                    DEFAULT: "rgb(229,189,115)", //DEFAULT = 500
                    100: "rgb(229,221,207)", // hell
                    200: "rgb(229,213,184)",
                    300: "rgb(229,205,161)",
                    400: "rgb(229,197,138)",
                    500: "rgb(229,189,115)",
                    600: "rgb(229,181,92)",
                    700: "rgb(229,173,69)",
                    800: "rgb(229,165,46)",
                    900: "rgb(229,157,23)", //dunkel
                },
                adlerbrown: {
                    DEFAULT: "rgb(77,62,54)", //DEFAULT = 300
                    100: "rgb(77,32,8)", // hell
                    200: "rgb(77,37,15)",
                    300: "rgb(77,42,23)",
                    400: "rgb(77,47,31)",
                    500: "rgb(77,52,38)",
                    600: "rgb(77,57,46)",
                    700: "rgb(77,62,54)",
                    800: "rgb(77,67,61)",
                    900: "rgb(77,72,69)", // dunkel
                },
                adlergreen: {
                    DEFAULT: "#009900",
                },
                adlergrey: {
                    DEFAULT: "rgb(128,128,128)", //DEFAULT = 500
                    100: "rgb(230,230,230)", // hell
                    200: "rgb(204,204,204)",
                    300: "rgb(179,179,179)",
                    400: "rgb(153,153,153)",
                    500: "rgb(128,128,128)",
                    600: "rgb(102,102,102)",
                    700: "rgb(77,77,77)",
                    800: "rgb(51,51,51)",
                    900: "rgb(26,26,26)", // dunkel
                },
                adlergray: colors.adlergrey,
                adlerdeactivated: "#e9e9e9",
                buttonbgblue: "#e9f2fa",
                buttonpressedblue: "#ace8fc",
                adlertextgrey: "#111111",
                adlerbuttonlocked: "#b9bfc6",
                adlerdeactivatedtext: "#e9e9e9",
                adlerbggradientfrom: "#a1c8e5",
                adlerbggradientto: "#e2eaf2",
                nodehandlecolor: "#e9d6b3",
                
                //neue Farben
                adlerbgbright: "#f6f6f6",
            },
            fontFamily: {
                'sans': ['Roboto']
            }
        },
    },
    plugins: [],
    safelist: [
        'right-0',
        'left-0',
        "order-1",
        "order-2",
        "bg-adlergrey-100",
        "bg-adlergrey-300",
        "ml-1",
        "mr-1"
    ]
}