const titleErr = document.querySelector('.title-err');
const descriptionErr = document.querySelector('.description-err');
const createTicketForm = document.querySelector('.create-form-wrapper form');
const dialogCancelBtn = document.querySelector('.cancel-btn');
const titleInput = document.querySelector('.t-tile input');
const descriptionInput = document.querySelector('.t-description textarea');

dialogCancelBtn.addEventListener('click', clearForm);
createTicketForm.addEventListener('submit', validateTask);


function validateTask(event) {
    const title = titleInput.value.trim();
    const description = descriptionInput.value.trim();

    const isTitleValid = title.length >= 2 && title.length <= 100;
    const isDescriptionValid = description.length >= 5 && description.length <= 2000;

    if (!isTitleValid || !isDescriptionValid) {

        event.preventDefault();

        if (!isTitleValid) {
            titleErr.textContent = 'Title length should be between 2 and 100 characters.';
        } else {
            titleErr.textContent = '';
        }
        if (!isDescriptionValid) {
            descriptionErr.textContent = 'Description length should be between 5 and 2000 characters.';
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

