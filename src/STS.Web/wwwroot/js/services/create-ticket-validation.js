const createTicketForm = document.querySelector('.create-form-wrapper form');
const title = createTicketForm.querySelector('input#Title');
const content = createTicketForm.querySelector('textarea#Content');
const titleWrapper = createTicketForm.querySelector('div.t-tile');
const contentWrapper = createTicketForm.querySelector('div.t-description');
const dialogCancelBtn = document.querySelector('.cancel-btn');

createTicketForm.addEventListener('submit', validateTicketInput)
dialogCancelBtn.addEventListener('click', clearErrors);

let titleError = null;
let contentError = null;

const isTitleValid = function () {
    return title.value.trim().length >= 2
        && title.value.trim().length <= 100;
};

const isContentValid = function () {
    return content.value.trim().length >= 5
        && content.value.trim().length <= 2000;
};

function validateTicketInput(event) {
    if (!isTitleValid() || !isContentValid()) {
        event.preventDefault();

        if (!isTitleValid()) {
            if (!titleWrapper.contains(titleError)) {
                titleError = createErr('p', 'Ticket title should be between 2 and 100 characters.');
                titleWrapper.appendChild(titleError);
            }
        }
        else if (titleWrapper.contains(titleError)) {
            titleWrapper.removeChild(titleError);
        }

        if (!isContentValid()) {
            if (!contentWrapper.contains(contentError)) {
                contentError = createErr('p', 'Ticket content should be between 5 and 2000 characters.');
                contentWrapper.appendChild(contentError);
            }    
        } else if (contentWrapper.contains(contentError)) {
            contentWrapper.removeChild(contentError);
        }

        return;
    }
}

function clearErrors() {
    clearTitleError();
    clearContentError();
}

function clearTitleError() {
    if (titleError) {
        titleWrapper.removeChild(titleError);
        titleError = null;
    }

    title.value = '';
}

function clearContentError() {
    if (contentError) {
        contentWrapper.removeChild(contentError);
        contentError = null;
    }

    content.value = '';
}

function createErr(element, message) {
    const errorElement = document.createElement(element);
    errorElement.style.color = 'red';
    errorElement.textContent = message;
    errorElement.classList.add('p-err-msg');

    return errorElement;
}