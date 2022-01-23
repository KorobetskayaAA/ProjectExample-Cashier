import api from "/utils/api.js";
import "/components/form.field/form.field.js";
import { setLoading, setAlert, setSubmitting } from "/utils/forms.js";

function getProfileRequest() {
    return api.get("account");
}

function updateProfileRequest(newProfile) {
    return api.put("account", newProfile);
}

const form = document.getElementById("form-editProfile");
loadProfile(form);

function loadProfile() {
    if (form.loading) return;
    setLoading(form, true);
    getProfileRequest()
        .then((profile) => {
            console.log(profile);
            form.userName = profile.userName;
            form.roles = profile.roles;
            form["firstName"].value = profile.firstName;
            form["middleName"].value = profile.middleName;
            form["lastName"].value = profile.lastName;
            form["email"].value = profile.email;
            form.onsubmit = submitForm; // заменит обработчик, если он уже был назначен
        })
        .catch(() => setAlert(form, "Не удалось загрузить данные профиля"))
        .finally(() => setLoading(form, false));
}

function submitForm(evt) {
    evt.preventDefault();
    const form = evt.target;

    if (form.submitting) return;

    setSubmitting(form, true);

    updateProfileRequest({
        username: form.userName,
        roles: form.roles,
        firstName: form["firstName"].value,
        middleName: form["middleName"].value,
        lastName: form["lastName"].value,
        email: form["email"].value,
    })
        .then(() => setAlert(form, "Профиль успешно обновлен!", "success"))
        .catch(() => setAlert(form, "Не удалось обновить данные профиля"))
        .finally(() => setSubmitting(form, false));
}
