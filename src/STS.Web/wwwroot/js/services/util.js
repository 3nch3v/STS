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

export function modal(modalElement, dialogElement, cancelBtnElement) {
    const showModal = document.querySelector(modalElement);
    var dialog = document.querySelector(dialogElement);
    const cancelBtn = document.querySelector(cancelBtnElement);

    showModal.addEventListener('click', openDialog);
    cancelBtn.addEventListener('click', closeDialog);

    function openDialog() {
        if (!dialog.open) {
            dialog.showModal();
        }
    };

    function closeDialog() {
        dialog.close('Cancel');
    };
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

    setTimeout(() => msgDiv.style.display = 'none', 3000);
}