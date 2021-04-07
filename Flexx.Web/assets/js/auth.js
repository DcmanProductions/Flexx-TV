import("/assets/js/user.js")

const firebaseConfig = {
    apiKey: "AIzaSyB2rYS67WC8WX3jCaK7xu3aTQSNMITWP5M",
    authDomain: "flexx-tv.firebaseapp.com",
    projectId: "flexx-tv",
    storageBucket: "flexx-tv.appspot.com",
    messagingSenderId: "80239777901",
    appId: "1:80239777901:web:9037e9409221e339684764",
    measurementId: "G-WMZP245N3R"
};
// Initialize Firebase
if (firebase.apps.length == 0)
    firebase.initializeApp(firebaseConfig);
// firebase.analytics();

firebase.auth().onAuthStateChanged(user => {
    if (user) {
        // Has User
        userName = user["email"];
        // console.log(user)
        try {
            isLoggedIn = true;
            // Showing Elements
            document.getElementById("LoginNavItem").hidden = true;
            document.getElementById("RegisterNavItem").hidden = true;
            document.getElementById("AboutNavItem").hidden = true;

            // Hiding Elements
            document.getElementById("LogOutNavItem").hidden = false;
            document.getElementById("MyProfileNavItem").hidden = false;
            document.getElementById("MovieNavItem").hidden = false;
            document.getElementById("TVNavItem").hidden = false;

            // Setting User Specific Properties
            document.getElementById("accountProfile").innerHTML = userName
            if (window.location.pathname == "/" || window.location.pathname == "/About/") {
                window.location.href = "/Library/";
            }

            // Updating Links
            document.getElementById("brandLink").href = "/Library/"
            document.getElementById("flexxRedCTA").href = "/FlexxRED/"

            document.getElementById("MovieNavItem").href = "/Library/Movies/"
            document.getElementById("TVNavItem").href = "#"

            document.getElementById("LogOutNavItem").onclick = event => { firebase.auth().signOut() }
            
        } catch { }
    } else {
        // No User
        try {
            isLoggedIn = false;
            // Showing Elements
            document.getElementById("LoginNavItem").hidden = false;
            document.getElementById("RegisterNavItem").hidden = false;
            
            // Hiding Elements
            document.getElementById("LogOutNavItem").hidden = true;
            document.getElementById("MyProfileNavItem").hidden = true;
            document.getElementById("MovieNavItem").hidden = true;
            document.getElementById("TVNavItem").hidden = true;
            
            // Setting User Specific Properties
            document.getElementById("accountProfile").innerHTML = "My Account"
            if (window.location.pathname.toString().includes("/Library/") || window.location.pathname.toString().includes("/FlexxRED/") ) {
                window.location.href = "/";
            }
            document.getElementById("LogOutNavItem").onclick = event => { alert('Well, well, well... What do we have here.\nA curious one!') }
        } catch { }
    }
})