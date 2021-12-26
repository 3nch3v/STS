export const settings = {
    host: ''
};

async function request(url, options) {
    try {
        const response = await fetch(settings.host + url, options);

        if (response.ok != true) {
            const error = await response.json();
            throw new Error(error.message);
        }

        if (response.status == 204) {
            return response;
        }

        return await response.json();

    } catch (err) {
        throw err;
    }
}

function getOptions(method = 'GET', body, token) {
    const options = {
        method,
        headers: {}
    };

    if (body != undefined) {
        options.headers['Content-Type'] = 'application/json; charset=utf-8';
        options.body = JSON.stringify(body);
    }

    if (token != undefined) {
        options.headers['RequestVerificationToken'] = token;
    }

    return options;
}

export async function get(url) {
    return request(url, getOptions());
}

export async function post(url, data, token) {
    return request(url, getOptions('POST', data, token));
}

export async function put(url, data, token) {
    return request(url, getOptions('PUT', data, token));
}

export async function del(url, token) {
    return request(url, getOptions('DELETE', token));
}