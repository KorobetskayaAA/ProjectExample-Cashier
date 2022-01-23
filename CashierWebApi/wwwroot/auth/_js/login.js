import api, { HttpStatusError } from "/utils/api.js";
import { setSubmitting, setAlert as setError } from "/utils/forms.js";
import "/components/auth.header/auth.header.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";

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

loginForm.addEventListener("submit", async (evt) => {
    evt.preventDefault();
    const form = evt.target;

    if (form.submitting) return;

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
