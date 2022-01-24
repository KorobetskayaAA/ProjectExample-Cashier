import api from "/utils/api.js";
import { createRublesFormat } from "/utils/format.js";
import "/components/alert/alert.js";

function getProductsRequest({ search, sortPrice }) {
    const searchParams = new URLSearchParams();
    if (search) {
        searchParams.append("search", search);
    }
    if (sortPrice) {
        searchParams.append("sortPrice", sortPrice);
    }
    return api.get("products?" + searchParams.toString());
}

function deleteProductsRequest(barcode) {
    return api.delete("products/" + barcode);
}

const list = document.querySelector("#products-container");
const template = document.querySelector("#products-template");
const priceFormat = createRublesFormat(true);

fillProducts();

async function fillProducts() {
    list.innerHTML =
        '<div class="text-center my-3"><span class="spinner-border text-primary"></span></div>';
    try {
        const products = await getProductsRequest({ sortPrice: "asc" });
        list.innerHTML = "";

        for (let product of products) {
            list.append(createProductCard(product));
        }
    } catch (err) {
        console.error(err);
        const alert = document.createElement("alert-message");
        alert.innerText = "Не удалось загрузить список товаров";
        list.replaceChildren(alert);
    }
}
function createProductCard(product) {
    const card = template.content.cloneNode(true);
    const cardName = card.querySelector('[name="name"]');
    const cardBarcode = card.querySelector('[name="barcode"]');
    const cardPrice = card.querySelector('[name="price"]');
    const cardEdit = card.querySelector('[name="edit"]');
    const cardRemove = card.querySelector('[name="remove"]');
    if (cardName) {
        cardName.innerHTML = product.name;
    }
    if (cardBarcode) {
        cardBarcode.innerHTML = product.barcode;
    }
    if (cardPrice) {
        cardPrice.innerHTML = priceFormat.format(product.price);
    }

    cardEdit.href = "./edit.html?barcode=" + product.barcode;
    cardRemove.onclick = () => {
        deleteProductsRequest(product.barcode)
            .then(() => fillProducts())
            .catch((err) => {
                console.error(err);
                const alert = document.createElement("alert-message");
                alert.innerText = `Не удалось удалить товар ${product.barcode} (${product.name})`;
                alert.scrollIntoView();
                list.after(alert);
            });
    };
    return card;
}
