import api from "/utils/api.js";
import "/components/form.field/form.field.js";
import { setAlert, setSubmitting } from "/utils/forms.js";
import { validate } from "./validate.product.js";

function createProductsRequest(product) {
    return api.post("products", product);
}

const form = document.getElementById("form-product");

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
    createProductsRequest({
        barcode: form["barcode"].value,
        name: form["name"].value,
        price: +form["price"].value,
    })
        .then(() => {
            setAlert(form, "Товар успешно создан.", "success");
            setTimeout(() => (location = "/products/index.html"));
        })
        .catch(() => setAlert(form, "Не удалось создать товар."))
        .finally(() => setSubmitting(form, false));
});
