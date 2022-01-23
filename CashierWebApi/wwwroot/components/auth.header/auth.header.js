import api from "../../utils/api.js";

function logoutRequest() {
    return api.post("account/logout");
}

function getProfile() {
    return api.get("account");
}

//<li><a href="..." class="dropdown-item" type="button">...</a></li>
function createMenuListItem(content, link, action) {
    const menuItem = document.createElement("li");
    const menuItemLink = document.createElement("a");
    menuItemLink.role = "button";
    menuItemLink.classList.add("dropdown-item");
    menuItemLink.innerHTML = content;
    menuItemLink.href = link;
    if (action) {
        menuItemLink.addEventListener("click", action);
    }
    menuItem.append(menuItemLink);
    return menuItem;
}

// <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="profileMenu">
//     <li><a href="profile/index.html" class="dropdown-item" type="button">Профиль</a></li>
//     <li><a href="profile/change_password.html" class="dropdown-item" type="button">Сменить пароль</a></li>
//     <li><a href="#" class="dropdown-item" type="button">Выйти</a></li>
// </ul>
function createUserMenu(logoutAction) {
    const menuList = document.createElement("ul");
    menuList.classList.add("dropdown-menu");
    menuList.classList.add("dropdown-menu-end");
    menuList.setAttribute("aria-labelledby", "profileMenu");
    menuList.append(createMenuListItem("Профиль", "/profile/index.html"));
    menuList.append(
        createMenuListItem("Сменить пароль", "/profile/change_password.html")
    );
    menuList.append(createMenuListItem("Выйти", "#", logoutAction));
    return menuList;
}

// <div class="dropdown">
// <button class="btn btn-primary" type="button" id="profileMenu" data-bs-toggle="dropdown" aria-expanded="false">
//   Иванов А.Е. <i class="bi bi-person-circle"></i>
// </button>
// <user-dropdown-menu></user-dropdown-menu>
// </div>
function createUserDropdown(userName, logoutAction) {
    const dropdown = document.createElement("div");
    dropdown.classList.add("dropdown");
    // TODO refactor
    dropdown.innerHTML = `<button class="btn btn-primary" type="button" id="profileMenu" data-bs-toggle="dropdown" aria-expanded="false">
         <span id="authHeader-userName">${userName}</span> <i class="bi bi-person-circle"></i>
        </button>`;
    dropdown.append(createUserMenu(logoutAction));
    return dropdown;
}

export class AuthHeader extends HTMLElement {
    constructor() {
        super();
        this.loading = document.createElement("div");
        this.loading.className = "text-secondary";
        this.loading.innerHTML =
            '<span class="spinner-border spinner-border-sm"></span>';
        this.login = document.createElement("div");
        const loginUrl =
            "/auth/login.html?from=" + encodeURIComponent(location.pathname);
        this.login.innerHTML = `<a class="btn btn-primary" href="${loginUrl}">Войти</a>`;
    }

    logout() {
        this.replaceChildren(this.loading);
        console.log("logout");
        logoutRequest().finally(() => {
            this.render();
        });
    }

    connectedCallback() {
        this.render();
    }

    render() {
        this.replaceChildren(this.loading);
        // TODO refactor
        getProfile()
            .then((profile) => {
                this.profile = profile;
                if (profile) {
                    this.replaceChildren(
                        createUserDropdown(profile.userName, () =>
                            this.logout()
                        )
                    );
                } else {
                    this.replaceChildren(this.login);
                }
            })
            .catch((err) => {
                this.replaceChildren(this.login);
            });
    }
}

customElements.define("auth-header", AuthHeader);
