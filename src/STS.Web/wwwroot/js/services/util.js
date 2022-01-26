import constants from '../constants.js';
const ticket = document.querySelector('.ticket');

export function getTicketId()
{
    const ticketId = ticket.dataset.ticketId;
    return ticketId;
}

export function getRequestToken() {
    const token = ticket.dataset.requestToken;
    return token;
}

export function createHtmlElement(type, attributes, ...content) {
    const result = document.createElement(type);

    for (let [attr, value] of Object.entries(attributes || {})) {
        if (attr.substring(0, 2) == 'on') {
            result.addEventListener(attr.substring(2).toLocaleLowerCase(), value);
        } else {
            result[attr] = value;
        }
    }

    content = content.reduce((a, c) => a.concat(Array.isArray(c) ? c : [c]), []);

    content.forEach(e => {
        if (typeof e == 'string' || typeof e == 'number') {
            const node = document.createTextNode(e);
            result.appendChild(node);
        } else {
            result.appendChild(e);
        }
    });

    return result;
};

export function displayMessage(message) {
    const msgDiv = document.querySelector('#msgBox');
    const spanElement = msgDiv.querySelector('span');
    spanElement.textContent = message;
    msgDiv.style.display = 'block';
    setTimeout(() => msgDiv.style.display = 'none', constants.MSG_TIMEOUT);
}