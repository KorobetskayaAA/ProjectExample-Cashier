import api from "/utils/api.js";
export { HttpStatusError } from "/utils/api.js";

export function getUsersRequest() {
    return api.get("users");
}

export function getUserRequest(userName) {
    return api.get("users/" + userName);
}

export function createUserRequest(user) {
    return api.post("users", user);
}

export function updateUserRequest(user) {
    return api.put("users", user);
}

export function deleteUserRequest(user) {
    return api.delete("users/" + user.userName);
}

export function addUserRoleRequest(user, role) {
    return api.post(`users/${user.userName}/role/${role}`);
}

export function deleteUserRoleRequest(user, role) {
    return api.delete(`users/${user.userName}/role/${role}`);
}

export function resetUserPasswordRequest(user, password) {
    const data = new FormData();
    data.append("password", password);
    return api.post(`users/${user.userName}/password`, data);
}
