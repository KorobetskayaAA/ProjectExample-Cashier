const form = document.getElementById("form-product");
const barcodeField = form.querySelector('form-field[name="barcode"]');
const priceField = form.querySelector('form-field[name="price"]');

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

export function validate(form) {
    return (
        validateBarcode(form["barcode"].value) ||
        validatePrice(form["price"].value)
    );
}
