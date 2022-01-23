import { attributeValueToFunction } from "/utils/forms.js";

class SortableTableCol extends HTMLTableCellElement {
    connectedCallback() {
        this.classList.add("bi");
        this.role = "button";
        this.onmouseup = () => {
            if (!this.sortDirection || this.sortDirection == "desc") {
                this.sortDirection = "asc";
            } else {
                this.sortDirection = "desc";
            }
        };
    }

    static get observedAttributes() {
        return ["sortdirection", "onsorting"];
    }

    get sortDirection() {
        return this.getAttribute("sortdirection");
    }
    set sortDirection(value) {
        this.setAttribute("sortdirection", value);
    }

    get onsorting() {
        return this._onsorting;
    }
    set onsorting(value) {
        this._onsorting = attributeValueToFunction(value);
        if (this._onsorting) {
            this.style.cursor = "pointer";
        } else {
            this.style.cursor = "default";
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue == newValue) return;
        if (name == "sortdirection") {
            newValue = newValue.toLowerCase();
            if (newValue == "asc") {
                this.classList.add("bi-arrow-down-short");
            } else {
                this.classList.remove("bi-arrow-down-short");
            }
            if (newValue == "desc") {
                this.classList.add("bi-arrow-up-short");
            } else {
                this.classList.remove("bi-arrow-up-short");
            }
            if (this.onsorting) this._onsorting(newValue);
        } else if (name == "onsorting") {
            this.onsorting = newValue;
        }
    }
}

customElements.define("th-sortable", SortableTableCol, { extends: "th" });
