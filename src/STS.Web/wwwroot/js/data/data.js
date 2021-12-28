import * as api from "../api/api.js";

const host = "https://localhost:5001/api";
api.settings.host = host;

const router = {
    comment: '/comments',
    commentById: (id) => `/comments/${id}`,
    editTicket: '/tickets',
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

//export async function getById(id) {
//    return await api.get(router.byId(id));
//}

//export async function edit(id, token, data) {
//    return await api.put(router.byId(id), token, data);
//}

