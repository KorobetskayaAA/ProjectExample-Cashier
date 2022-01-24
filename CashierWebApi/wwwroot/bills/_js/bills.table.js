import api from "/utils/api.js";
import { createRublesFormat } from "/utils/format.js";
import "/components/alert/alert.js";
import "/components/tables/th.sortable.js";

export function getBillsRequest({ statusId, orderBy, orderAsc }) {
    const searchParams = new URLSearchParams();
    if (statusId) {
        searchParams.append("statusId", statusId);
    }
    if (orderBy) {
        searchParams.append("orderBy", orderBy);
    }
    if (orderAsc) {
        searchParams.append("orderAsc", orderAsc);
    }
    return api.get("bills?" + searchParams.toString());
}

function cancelBillRequest(billNumber) {
    return api.put(`bills/${billNumber}/cancel`);
}

const rowTemplate = document.querySelector("#row-template");
const table = document.querySelector("#bills-table");
const costFormat = createRublesFormat(false);
const createdHeader = table.tHead.querySelector('th[name="created-header"]');
const costHeader = table.tHead.querySelector('th[name="cost-header"]');
const statusHeader = table.tHead.querySelector('th[name="status-header"]');
getOrderBy();
loadBills();

function createTableRow(bill) {
    const newRow = rowTemplate.content.cloneNode(true);
    fillTableRow(newRow, bill);

    const removeButton = newRow.querySelector('button[name="remove"]');
    if (removeButton) {
        removeButton.onclick = () => {
            cancelBillRequest(bill.number).then((cancelledBill) =>
                fillTableRow(newRow, cancelledBill)
            );
        };
    }

    return newRow;
}

function fillTableRow(row, bill) {
    row.querySelector('td[name="number"]').innerHTML = bill.number;
    row.querySelector('td[name="created"]').innerHTML =
        bill.created.toLocaleString();
    row.querySelector('td[name="creator"]').innerHTML = bill.creator;
    row.querySelector('td[name="itemsCount"]').innerHTML = bill.items.length;
    row.querySelector('td[name="cost"]').innerHTML = costFormat.format(
        bill.cost
    );
    row.querySelector('td[name="status"]').innerHTML = bill.status.name;
    const removeButton = row.querySelector('button[name="remove"]');
    removeButton.disabled = bill.status.statusId != 1;
}

function fillTableBody(tableBody, bills) {
    for (let bill of bills) {
        bill.created = new Date(bill.created);
        tableBody.append(createTableRow(bill));
    }
}

export async function loadBills() {
    const tableBody = table.tBodies[0];

    tableBody.innerHTML =
        '<tr><td colspan="6" class="text-center"><span class="spinner-border text-primary"></span></td></tr>';

    const searchParams = new URLSearchParams(location.search);
    try {
        const bills = await getBillsRequest({
            statusId: searchParams.get("statusId"),
            orderBy: searchParams.get("orderBy"),
            orderAsc: searchParams.get("orderAsc"),
        });

        tableBody.innerHTML = "";
        fillTableBody(tableBody, bills);
    } catch {
        const alert = document.createElement("alert-message");
        alert.innerText = "Не удалось загрузить список чеков";
        tableBody.replaceChildren(alert);
    }
}

function setOrderBy(field, sortOrder) {
    const url = new URL(location.href);
    if (url.searchParams.has("orderBy")) {
        url.searchParams.set("orderBy", field);
    } else {
        url.searchParams.append("orderBy", field);
    }
    if (url.searchParams.has("orderAsc")) {
        url.searchParams.set("orderAsc", sortOrder == "asc" ? "true" : "false");
    } else {
        url.searchParams.append(
            "orderAsc",
            sortOrder == "asc" ? "true" : "false"
        );
    }
    location.replace(url);
}

function getOrderBy() {
    const searchParams = new URLSearchParams(location.search);
    switch (searchParams.get("orderBy")) {
        case "date":
            createdHeader.sortDirection =
                searchParams.get("orderAsc") == "true"
                    ? "asc"
                    : searchParams.get("orderAsc") == "false"
                    ? "desc"
                    : undefined;
            break;
        case "cost":
            costHeader.sortDirection =
                searchParams.get("orderAsc") == "true"
                    ? "asc"
                    : searchParams.get("orderAsc") == "false"
                    ? "desc"
                    : undefined;
            break;
        case "status":
            statusHeader.sortDirection =
                searchParams.get("orderAsc") == "true"
                    ? "asc"
                    : searchParams.get("orderAsc") == "false"
                    ? "desc"
                    : undefined;
            break;
    }
}

createdHeader.onsorting = (sortOrder) => {
    costHeader.sortDirection = undefined;
    statusHeader.sortDirection = undefined;
    setOrderBy("date", sortOrder);
};
costHeader.onsorting = (sortOrder) => {
    createdHeader.sortDirection = undefined;
    statusHeader.sortDirection = undefined;
    setOrderBy("cost", sortOrder);
};
statusHeader.onsorting = (sortOrder) => {
    costHeader.sortDirection = undefined;
    createdHeader.sortDirection = undefined;
    setOrderBy("status", sortOrder);
};
