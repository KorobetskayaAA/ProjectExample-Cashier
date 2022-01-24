import { createUserRequest, HttpStatusError } from "./user.api.js";
import { setLoading, setSubmitting, setAlert } from "../../utils/forms.js";
import {
    validateCreation,
    validatePassword,
    validatePasswordConfirmation,
} from "./user.validation.js";
import "/components/form.field/form.field.js";

const form = document.querySelector("#form-user");
form.addEventListener("submit", (evt) => {
    evt.preventDefault();
    submitForm();
});

function submitForm() {
    if (form.submitting) return;

    const validationResult = validateCreation(form);
    if (validationResult) {
        setAlert(validationResult);
        return;
    }

    setSubmitting(form, true);
    const user = {
        userName: form["userName"].value,
        email: form["email"].value,
        firstName: form["firstName"].value,
        middleName: form["middleName"].value,
        lastName: form["lastName"].value,
        roles: Array.from(form["role"].values())
            .filter((v) => v.checked)
            .map((v) => v.value),
        password: form["password"].value,
    };
    createUserRequest(user)
        .then(() => {
            setAlert(form, "Пользователь успешно создан!", "success");
            location.assign("./");
        })
        .catch((err) => {
            setAlert(
                form,
                err instanceof HttpStatusError
                    ? err.message
                    : "Не удалось создать пользователя"
            );
        })
        .finally(() => setSubmitting(form, false));
}

const passwordField = form.querySelector('form-field[name="password"]');
const passwordConfirmationField = form.querySelector(
    'form-field[name="passwordConfirmation"]'
);

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
