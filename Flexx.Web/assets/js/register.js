import("/assets/js/auth.js")

const registerForm = document.getElementById("registerForm")

// const firstName = document.getElementById("nameFirst-mbr-popup-t").value
// const lastName = document.getElementById("nameLast-mbr-popup-t").value



registerForm.addEventListener("submit", e => {
    let feedbackArea = document.getElementById("SignupFeedbackAlert")
    e.preventDefault();

    let email = document.getElementById("email-mbr-popup-t")
    let password = document.getElementById("password-mbr-popup-t")
    let confirmPassword = document.getElementById("confirm_password-mbr-popup-t")
    // alert(`${password} == ${confirmPassword}`)

    if (password.value != confirmPassword.value) {
        RegisterError("Password and Confirm Password fields MUST match")
    } else {
        let auth = firebase.auth();
        console.log(email.value)
        let promise = auth.createUserWithEmailAndPassword(email.value, password.value)

        promise.catch(e => {
            RegisterError(e.message)
        });
    }
    function RegisterError(error) {
        feedbackArea.classList.remove("alert-success")
        feedbackArea.classList.add("alert-danger")
        console.error(error);
        feedbackArea.innerHTML = error;
        feedbackArea.hidden = false;
        password.value = "";
        confirmPassword.value = "";
        setTimeout(() => {
            feedbackArea.hidden = true;
        }, 5000)
    }

    function RegisterSuccess(message) {
        feedbackArea.classList.remove("alert-danger")
        feedbackArea.classList.add("alert-success")
        feedbackArea.innerHTML = message;
        feedbackArea.hidden = false;
        setTimeout(() => {
            feedbackArea.hidden = true;
        }, 5000)
    }
});
