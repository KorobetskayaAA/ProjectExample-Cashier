import api, { HttpStatusError } from "/utils/api.js";
import { setSubmitting, setAlert } from "/utils/forms.js";
import "/components/auth.header/auth.header.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";
import "/components/form.field/form.field.js";

function changePasswordRequest(newPassword) {
    const formData = new FormData();
    formData.append("password", newPassword);
    return api.post("account/password", formData);
}

const PASSWORD_PATTERN =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/;
function validatePassword(password) {
    if (!password.match(PASSWORD_PATTERN)) {
        return (
            "Пароль должен содержать как минимум 8 символов, " +
            "одну строчную и одну заглавную латинскую букву, одну цифру и один специальный символ"
        );
    }
}

function validatePasswordConfirmation(password, passwordConfirmation) {
    if (password != passwordConfirmation) {
        return "Пароль и подтверждение пароля не совпадают";
    }
}

function validate(form) {
    return (
        validatePassword(form["password"].value) ||
        validatePasswordConfirmation(
            form["password"].value,
            form["passwordConfirmation"].value
        )
    );
}

const form = document.getElementById("form-changePassword");
const passwordField = form.querySelector('form-field[name="password"]');
const passwordConfirmationField = form.querySelector(
    'form-field[name="passwordConfirmation"]'
);

form.addEventListener("submit", (evt) => {
    evt.preventDefault();
    const form = evt.target;

    if (form.submitting) return;

    const validationResult = validate(form);
    if (validationResult) {
        setAlert(form, validationResult, "danger");
        return;
    }

    setSubmitting(form, true);
    setAlert(form, "");

    changePasswordRequest(form["password"].value)
        .then(() => {
            setAlert(form, "Пароль успешно изменен!", "success");
            form.reset();
        })
        .catch(() => setAlert(form, "Не удалось изменить пароль", "danger"))
        .finally(() => setSubmitting(form, false));
});

form.addEventListener("changed", () => {
    console.log("form changed");
    setAlert(form, "");
});

passwordField.validation = (password) => {
    return (
        validatePassword(password) ||
        (passwordConfirmationField.touched
            ? passwordConfirmationField.validate()
            : undefined)
    );
};

passwordConfirmationField.validation = () => {
    return validatePasswordConfirmation(
        form["password"].value,
        form["passwordConfirmation"].value
    );
};
