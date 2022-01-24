import api from "/utils/api.js";
import "/components/form.field/form.field.js";
import { setLoading, setAlert, setSubmitting } from "/utils/forms.js";
import { validate } from "./validate.product.js";

function getProductsRequest(barcode) {
    return api.get("products/" + barcode);
}

function updateProductsRequest(product) {
    return api.put("products/" + product.barcode, product);
}

const form = document.getElementById("form-product");
formLoad(form);

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
    updateProductsRequest({
        barcode: form["barcode"].value,
        name: form["name"].value,
        price: +form["price"].value,
    })
        .then(() => {
            setAlert(form, "Товар успешно изменен", "success");
            setTimeout(() => (location = "/products/index.html"));
        })
        .catch(() => setAlert(form, "Не удалось изменить товар"))
        .finally(() => setSubmitting(form, false));
});

function formLoad(form) {
    const barcode = new URLSearchParams(location.search).get("barcode");
    setLoading(form, true);
    setAlert(form, "");
    getProductsRequest(barcode)
        .then((product) => {
            console.log(product);
            form["barcode"].value = product.barcode;
            form["name"].value = product.name;
            form["price"].value = product.price;
        })
        .catch(() =>
            setAlert(
                form,
                `Не удалось загрузить данные о товаре со штрих-кодом ${barcode}`
            )
        )
        .finally(() => setLoading(form, false));
}
