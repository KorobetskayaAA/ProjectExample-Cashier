import api from "/utils/api.js";
import "/components/form.field/form.field.js";
import { setAlert, setSubmitting } from "/utils/forms.js";

function createProductsRequest(product) {
    return api.post("products", product);
}

const form = document.getElementById("form-product");
const barcodeField = form.querySelector('form-field[name="barcode"]');
const priceField = form.querySelector('form-field[name="price"]');

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
        .then(() => (location = "/products/index.html"))
        .catch(() => setAlert(form, "Не удалось создать товар"))
        .finally(() => setSubmitting(form, false));
});

const BARCODE_PATTERN = /^\d{13}$/;
function validateBarcode(barcode) {
    if (!barcode.match(BARCODE_PATTERN)) {
        return "Штрих-код должен состоять из 13 цифр";
    }
}
barcodeField.validation = validateBarcode;

function validatePrice(price) {
    price = +price;
    if (isNaN(price) || price <= 0) {
        return "Введите положительное число";
    }
}
priceField.validation = validatePrice;

function validate(form) {
    return (
        validateBarcode(form["barcode"].value) ||
        validatePrice(form["price"].value)
    );
}
