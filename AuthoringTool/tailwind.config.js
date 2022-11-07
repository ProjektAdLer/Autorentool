/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["../**/*.razor", "../**/*.cshtml", "../**/*.html"],
    theme: {
        extend: {
            colors: {
                //Bei Custom Farben geben die Zahlenwerte die Saturation Prozentwerte wieder (700 = 70%)
                blacktrans: {
                    100: "rgba(0,0,0,0.1)",
                    200: "rgba(0,0,0,0.2)",
                    300: "rgba(0,0,0,0.3)",
                    400: "rgba(0,0,0,0.4)",
                    500: "rgba(0,0,0,0.5)", //50% schwarz für Modal-Hintergrund
                    600: "rgba(0,0,0,0.6)",
                    700: "rgba(0,0,0,0.7)",
                    800: "rgba(0,0,0,0.8)",
                    900: "rgba(0,0,0,0.9)",
                },
                adlerblue: {
                    DEFAULT: "rgb(69,160,229)", // DEFAULT = 700
                    100: "rgb(207,220,229)",
                    200: "rgb(184,210,229)",
                    300: "rgb(161,200,229)",
                    400: "rgb(138,190,229)",
                    500: "rgb(115,180,229)",
                    600: "rgb(92,170,229)",
                    700: "rgb(69,160,229)",
                    800: "rgb(46,150,229)",
                    900: "rgb(23,140,229)",
                },
                adlerdarkblue: {
                    DEFAULT: "rgb(23,45,77)", //DEFAULT = 700
                    100: "rgb(69,72,77)",
                    200: "rgb(61,67,77)",
                    300: "rgb(54,63,77)",
                    400: "rgb(46,58,77)",
                    500: "rgb(38,54,77)",
                    600: "rgb(31,49,77)",
                    700: "rgb(23,45,77)",
                    800: "rgb(15,40,77)",
                    900: "rgb(8,35,77)",
                },
                adlergold: {
                    DEFAULT: "rgb(229,189,115)", //DEFAULT = 500
                    100: "rgb(229,221,207)",
                    200: "rgb(229,213,184)",
                    300: "rgb(229,205,161)",
                    400: "rgb(229,197,138)",
                    500: "rgb(229,189,115)",
                    600: "rgb(229,181,92)",
                    700: "rgb(229,173,69)",
                    800: "rgb(229,165,46)",
                    900: "rgb(229,157,23)",
                },
                adlerbrown: {
                    DEFAULT: "rgb(77,62,54)", //DEFAULT = 300
                    100: "rgb(77,72,69)",
                    200: "rgb(77,67,61)",
                    300: "rgb(77,62,54)",
                    400: "rgb(77,57,46)",
                    500: "rgb(77,52,38)",
                    600: "rgb(77,47,31)",
                    700: "rgb(77,42,23)",
                    800: "rgb(77,37,15)",
                    900: "rgb(77,32,8)",
                },
                adlergreen: {
                    DEFAULT: "#009900",
                },
                adlerred: {
                    DEFAULT: "#880000",
                },
                buttonpressedblue: "rgb(0,120,229)",
                adlertextgrey: "#111111",
            },
        },
    },
    plugins: [],
    safelist: [
        'right-0',
        'left-0'
    ]
}