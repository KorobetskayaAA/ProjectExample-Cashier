import { getUsersRequest, deleteUserRequest } from "./user.api.js";
import "/components/alert/alert.js";

const table = document.querySelector("#table-users");
const rowTemplate = document.querySelector("#template-users");
const spinnerInTable = `<tr>
      <td colspan="6" class="text-center">
        <span class="spinner-border text-primary"></span>
      </td>
    </tr>`;
const alert = document.createElement("alert-message");

loadUsers();

async function loadUsers() {
    const tableBody = table.tBodies[0];

    tableBody.innerHTML = spinnerInTable;
    hideAlert();

    try {
        const users = await getUsersRequest();
        tableBody.innerHTML = "";
        for (let user of users) {
            tableBody.append(createTableRow(user));
        }
    } catch (err) {
        console.error(err);
        tableBody.innerHTML = "";
        showAlert(
            "Не удалось загрузить список пользователей",
            table.parentNode
        );
    }
}

function createTableRow(user) {
    const row = rowTemplate.content.cloneNode(true);
    for (let key in user) {
        const cell = row.querySelector(`[name="${key}"]`);
        if (cell) {
            cell.innerHTML = user[key];
        }
    }

    const editButton = row.querySelector('[name="edit"]');
    if (editButton) {
        editButton.href = "./edit.html?username=" + user.userName;
    }
    const deleteButton = row.querySelector('[name="delete"]');
    if (deleteButton) {
        deleteButton.onclick = () => deleteUser(user);
    }
    return row;
}

function deleteUser(user) {
    deleteUserRequest(user)
        .then(() => loadUsers())
        .catch(() =>
            showAlert(
                `Не удалось удалить пользователя ${user.userName}`,
                table.parentNode
            )
        );
}

function hideAlert() {
    alert.remove();
}

function showAlert(message, root) {
    alert.innerHTML = message;
    root.prepend(alert);
}
