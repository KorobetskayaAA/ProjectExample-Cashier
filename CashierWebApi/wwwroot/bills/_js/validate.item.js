import { validateBarcode } from "/products/_js/validate.product.js";
export { validateBarcode } from "/products/_js/validate.product.js";

export function validateAmount(amount) {
    amount = +amount;
    if (isNaN(amount) || amount <= 0) {
        return "Введите положительное число";
    }
}

const form = document.getElementById("form-item");
if (form) {
    const barcodeField = form.querySelector('form-field[name="barcode"]');
    const amountField = form.querySelector('form-field[name="amount"]');
    barcodeField.validation = validateBarcode;
    amountField.validation = validateAmount;
}
