import * as api from "../api/api.js";

const host = "https://localhost:5001";
api.settings.host = host;

const router = {
    comment: '/api/comment',
    byId: (id) => `/api/comment/${id}`,
};

export async function create(data, token) {
    return await api.post(router.comment, data, token);
}

export async function edit(id, data, token) {
    return await api.put(router.byId(id), data, token);
}

export async function deleteById(id, token) {
    return await api.del(router.byId(id), token);
}

export async function getById(id) {
    return await api.get(router.byId(id));
}
