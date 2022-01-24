import api from "/utils/api.js";
import "/components/form.field/form.field.js";
import "/components/submit.button/submit.button.js";
import { validateBarcode, validateAmount } from "./validate.item.js";
import { createAmountFormat, createRublesFormat } from "/utils/format.js";
import { setAlert, setSubmitting } from "/utils/forms.js";

function createBillRequest(items) {
    return api.post("bills", items);
}

function getProductRequest(barcode) {
    return api.get("products/" + barcode);
}

const form = document.getElementById("form-item");
const table = document.getElementById("items-table");
const rowTemplate = document.querySelector("#row-template");
const saveButton = document.querySelector("#button-save");
const bill = {
    items: [],
    sum() {
        return this.items.reduce((sum, item) => (sum += +item.cost), 0);
    },
    add(item) {
        const index = this.items.findIndex((i) => i.barcode == item.barcode);
        if (index >= 0) {
            this.items[index].amount += item.amount;
            this.items[index].cost += item.cost;
        } else {
            this.items[this.items.length] = item;
        }
    },
    remove(barcode) {
        this.items = this.items.filter((i) => i.barcode != barcode);
    },
};
const rublesFormat = createRublesFormat(false);
const amountFormat = createAmountFormat();

form["barcode"].addEventListener("change", (evt) => {
    const barcode = evt.target.value;
    const submitButton = form.querySelector('button[type="submit"]');
    submitButton.disabled = true;

    if (!validateBarcode(barcode)) {
        getProductRequest(barcode)
            .then((product) => {
                form["name"].value = product.name;
                form["price"].value = product.price;
                form["cost"].value = rublesFormat.format(
                    product.price * form["amount"].value
                );
                submitButton.disabled = false;
            })
            .catch(() => {
                form["name"].value = "";
                form["price"].value = "0,00";
                form["cost"].value = "0,000";
            });
    }
});
form["amount"].addEventListener("input", (evt) => {
    const amount = +evt.target.value;
    form["cost"].value = rublesFormat.format(form["price"].value * amount);
});

form.addEventListener("submit", (evt) => {
    evt.preventDefault();
    const form = evt.target;

    const item = {
        barcode: form["barcode"].value,
        name: form["name"].value,
        price: +form["price"].value,
        amount: +form["amount"].value,
        cost: +form["price"].value * +form["amount"].value,
    };
    bill.add(item);
    saveButton.disabled = false;
    form.reset();
    const submitButton = form.querySelector('button[type="submit"]');
    submitButton.disabled = true;
    form["amount"].value = "1";
    fillItems();
});

saveButton.addEventListener("click", (evt) => {
    evt.stopPropagation();
    const button = evt.target;
    button.submitting = true;
    createBillRequest(bill.items)
        .then(() => {
            setAlert(table, "Чек успешно создан", "success");
            setTimeout(() => (location = "/index.html"));
        })
        .catch(() => setAlert(form, "Не удалось создать чек", "alert"))
        .finally(() => (button.submitting = false));
});

function fillItems() {
    console.log(bill);
    table.tBodies[0].innerHTML = "";
    for (let item of bill.items) {
        addTableRow(item);
    }
}

function addTableRow(item) {
    const row = rowTemplate.content.cloneNode(true);
    row.querySelector('[name="barcode"]').innerHTML = item.barcode;
    row.querySelector('[name="name"]').innerHTML = item.name;
    row.querySelector('[name="price"]').innerHTML = rublesFormat.format(
        item.price
    );
    row.querySelector('[name="amount"]').innerHTML = amountFormat.format(
        item.amount
    );
    row.querySelector('[name="cost"]').innerHTML = rublesFormat.format(
        item.cost
    );

    row.querySelector('[name="remove"]').onclick = () => {
        bill.remove(item.barcode);
        fillItems();
    };

    table.querySelector('[name="sum"]').innerHTML = rublesFormat.format(
        bill.sum()
    );
    table.tBodies[0].append(row);
}
