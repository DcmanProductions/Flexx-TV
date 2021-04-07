import("/assets/js/auth.js")


document.getElementById("loginForm").addEventListener("submit", e => {
    e.preventDefault();
    let feedbackArea = document.getElementById("LoginFeedbackAlert")
    let email = document.getElementById("email-mbr-popup-q")
    let password = document.getElementById("password-mbr-popup-q")

    let auth = firebase.auth();
    let promise = auth.signInWithEmailAndPassword(email.value, password.value)
    promise.catch(e => {
        LoginError(e.message)
    });
    function LoginError(error) {
        feedbackArea.classList.remove("alert-success")
        feedbackArea.classList.add("alert-danger")
        console.error(error);
        feedbackArea.innerHTML = error;
        feedbackArea.hidden = false;
        password.value = "";
        setTimeout(() => {
            feedbackArea.hidden = true;
        }, 5000)
    }
    
    function LoginSuccess(message) {
        feedbackArea.classList.remove("alert-danger")
        feedbackArea.classList.add("alert-success")
        feedbackArea.innerHTML = message;
        feedbackArea.hidden = false;
        email.value = "";
        password.value = "";
        setTimeout(() => {
            feedbackArea.hidden = true;
        }, 5000)
    }
});

