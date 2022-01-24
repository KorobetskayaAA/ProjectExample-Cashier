import {
    addUserRoleRequest,
    deleteUserRoleRequest,
    getUserRequest,
    resetUserPasswordRequest,
    updateUserRequest,
} from "./user.api.js";
import { setLoading, setSubmitting, setAlert } from "../../utils/forms.js";
import {
    validatePassword,
    validatePasswordConfirmation,
} from "./user.validation.js";
import "/components/form.field/form.field.js";

const formProfile = document.querySelector("#form-userProfile");
const formChangePassword = document.querySelector("#form-changePassword");
const formRoles = document.querySelector("#form-userRoles");
const userName = new URLSearchParams(location.search).get("username");
document.getElementById("title-userName").innerHTML = userName;
loadUser();

function loadUser() {
    if (formProfile.loading) return;
    setLoading(formProfile, true);
    setAlert(formProfile, "");
    getUserRequest(userName)
        .then((user) => fillForms(user))
        .catch(() => {
            setAlert(formProfile, "Не удалось загрузить профиль пользователя");
        })
        .finally(() => setLoading(formProfile, false));
}

function fillForms(user) {
    for (let key in user) {
        if (formProfile[key]) {
            formProfile[key].value = user[key];
        }
    }
    for (let role of user.roles) {
        formRoles.querySelector(`input[value="${role}"]`).checked = true;
    }
    formProfile.addEventListener("submit", (evt) => {
        evt.preventDefault();
        submitProfile();
    });
    formChangePassword.addEventListener("submit", (evt) => {
        evt.preventDefault();
        submitPassword(user);
    });
    formRoles.addEventListener("submit", (evt) => {
        evt.preventDefault();
        submitRoles(user);
    });
}

function submitProfile() {
    if (formProfile.submitting) return;

    setSubmitting(formProfile, true);
    const user = {
        userName,
        email: formProfile["email"].value,
        firstName: formProfile["firstName"].value,
        middleName: formProfile["middleName"].value,
        lastName: formProfile["lastName"].value,
    };
    updateUserRequest(user)
        .then(() => {
            setAlert(formProfile, "Пользователь успешно обновлен!", "success");
        })
        .catch((err) => {
            console.error(err);
            setAlert(formProfile, "Не удалось обновить профиль пользователя");
        })
        .finally(() => setSubmitting(formProfile, false));
}

const passwordField = formChangePassword.querySelector(
    'form-field[name="password"]'
);
const passwordConfirmationField = formChangePassword.querySelector(
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
        formChangePassword["password"].value,
        formChangePassword["passwordConfirmation"].value
    );
};

function submitPassword(user) {
    if (formChangePassword.submitting) return;

    const password = formChangePassword["password"].value;
    const passwordConfirmation =
        formChangePassword["passwordConfirmation"].value;
    const validationResult =
        validatePassword(password) ||
        validatePasswordConfirmation(password, passwordConfirmation);
    if (validationResult) {
        setAlert(formChangePassword, validationResult);
        return;
    }

    setSubmitting(formChangePassword, true);
    resetUserPasswordRequest(user, password)
        .then(() => {
            setAlert(formChangePassword, "Пароль успешно обновлен!", "success");
        })
        .catch((err) => {
            console.error(err);
            setAlert(
                formChangePassword,
                "Не удалось обновить пароль пользователя"
            );
        })
        .finally(() => setSubmitting(formChangePassword, false));
}

function submitRoles(user) {
    if (formRoles.submitting) return;

    setSubmitting(formRoles, true);

    const checkedRoles = Array.from(formRoles["role"].values())
        .filter((v) => v.checked)
        .map((v) => v.value);
    let rolesRequests = [
        ...user.roles
            .filter((role) => !checkedRoles.includes(role))
            .map((role) => deleteUserRoleRequest(user, role)),
        ...checkedRoles
            .filter((role) => !user.roles.includes(role))
            .map((role) => addUserRoleRequest(user, role)),
    ];

    Promise.all(rolesRequests)
        .then(() => {
            setAlert(
                formRoles,
                "Роли пользователя успешно устанолены!",
                "success"
            );
        })
        .catch((err) => {
            console.error(err);
            setAlert(formRoles, "Не удалось установить роли пользователя");
        })
        .finally(() => setSubmitting(formRoles, false));
}
