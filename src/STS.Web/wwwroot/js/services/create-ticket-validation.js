import constants from '../constants.js';

const titleErr = document.querySelector('.title-err');
const descriptionErr = document.querySelector('.description-err');
const createTicketForm = document.querySelector('.create-form-wrapper form');
const cancelBtn = document.querySelector('.cancel-btn');
const titleInput = document.querySelector('.t-tile input');
const descriptionInput = document.querySelector('.t-description textarea');

cancelBtn.addEventListener('click', clearForm);
createTicketForm.addEventListener('submit', validateTask);

function validateTask(event) {
    const title = titleInput.value.trim();
    const description = descriptionInput.value.trim();

    const isTitleValid = title.length >= constants.TITLE_MIN_LENGTH
                      && title.length <= constants.TITLE_MAX_LENGTH;

    const isDescriptionValid = description.length >= constants.TICKET_DESCR_MIN_LENGTH
                            && description.length <= constants.TICKET_DESCR_MAX_LENGTH;

    if (!isTitleValid || !isDescriptionValid) {
        event.preventDefault();

        if (!isTitleValid) {
            titleErr.textContent = constants.TITLE_ERR_MSG;
        } else {
            titleErr.textContent = '';
        }
        if (!isDescriptionValid) {
            descriptionErr.textContent = constants.TICKET_DESCRIPTION_ERR_MSG;
        } else {
            descriptionErr.textContent = '';
        }

    } else {
        clearErrors();
    }
}

function clearErrors() {
    titleErr.textContent = '';
    descriptionErr.textContent = '';
}

function clearForm() {
    clearErrors();
    titleInput.value = '';
    descriptionInput.value = '';
}

