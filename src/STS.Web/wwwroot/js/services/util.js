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