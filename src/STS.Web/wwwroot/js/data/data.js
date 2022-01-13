import * as api from "../api/api.js";

const host = "https://localhost:5001/api";
api.settings.host = host;

const router = {
    comment: '/comments',
    commentById: (id) => `/comments/${id}`,
    editTicket: '/tickets',
    editTask: '/tasks',
    replayTask: '/replay',
    employees: (id) => `/users/${id}`,
};

export async function createComment(token, data) {
    return await api.post(router.comment, token, data);
}

export async function deleteCommentById(id, token) {
    return await api.del(router.commentById(id), token);
}

export async function editTicket(token, data) {
    return await api.put(router.editTicket, token, data);
}

export async function editTask(token, data) {
    return await api.put(router.editTask, token, data);
}

export async function createTaskComment(token, data) {
    return await api.post(router.replayTask, token, data);
}

export async function getEmployees(id, token) {
    return await api.get(router.employees(id) , token);
}
