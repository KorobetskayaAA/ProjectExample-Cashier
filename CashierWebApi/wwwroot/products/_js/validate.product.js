const BARCODE_PATTERN = /^\d{13}$/;
export function validateBarcode(barcode) {
    if (!barcode.match(BARCODE_PATTERN)) {
        return "Штрих-код должен состоять из 13 цифр";
    }
}
export function validatePrice(price) {
    price = +price;
    if (isNaN(price) || price <= 0) {
        return "Введите положительное число";
    }
}

export function validate(form) {
    return (
        validateBarcode(form["barcode"].value) ||
        validatePrice(form["price"].value)
    );
}

const form = document.getElementById("form-product");
if (form) {
    const barcodeField = form.querySelector('form-field[name="barcode"]');
    const priceField = form.querySelector('form-field[name="price"]');
    barcodeField.validation = validateBarcode;
    priceField.validation = validatePrice;
}
