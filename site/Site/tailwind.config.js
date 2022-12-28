/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: 'class',
    content: ["./**/*.{razor,html,cshtml,cs}"],
    theme: {
        extend: {
            colors: {
                'palette': { main: "#40BABD" },
            }
        }
    },
  plugins: [],
}