import api, { HttpStatusError } from "../../utils/api.js";

function loginRequest(userName, password, rememberMe) {
    return api.post("account/login", {
        userName,
        password,
        rememberMe,
    });
}

function goBack() {
    const fromUrl = new URLSearchParams(location.search).get("from");
    location.assign(fromUrl || "/");
}

const loginForm = document.getElementById("login-form");
let isSubmitting = false;

loginForm.addEventListener("submit", async (evt) => {
    evt.preventDefault();

    if (isSubmitting) return;

    const form = evt.target;
    const userName = form["userName"].value;
    const password = form["password"].value;
    const rememberMe = form["rememberMe"].checked;

    setSubmitting(form, true);
    setError(form, "");
    try {
        await loginRequest(userName, password, rememberMe);
        goBack();
    } catch (err) {
        if (err instanceof HttpStatusError && err.status == 401) {
            setError(
                form,
                "Не удалось выполнить вход. Проверьте правильность ввода логина и пароля."
            );
        } else {
            setError(
                form,
                "Произошла неизвестная ошибка при попытке входа. Попробуйте позже или обратитесь к администратору"
            );
        }
    } finally {
        setSubmitting(form, false);
    }
});
function setSubmitting(form, submitting) {
    isSubmitting = submitting;
    const submitButton = form.querySelector('button[type="submit"]');
    if (isSubmitting) {
        submitButton.disabled = true;
        const spinner = document.createElement("span");
        spinner.classList.add("spinner-border");
        spinner.classList.add("spinner-border-sm");
        submitButton.prepend(spinner);
    } else {
        submitButton.disabled = undefined;
        const spinner = submitButton.querySelector(".spinner-border");
        spinner.remove();
    }
}
function setError(form, message) {
    errorMessage = message;
    let alertElement = form.querySelector(".alert");
    if (!alertElement) {
        alertElement = document.createElement("div");
        alertElement.classList.add("alert");
        alertElement.classList.add("alert-danger");
        form.append(alertElement);
    }
    if (message) {
        alertElement.classList.remove("visually-hidden");
        alertElement.innerText = message;
    } else {
        alertElement.classList.add("visually-hidden");
        alertElement.innerText = "";
    }
}
