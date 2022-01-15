import constants from '../constants.js';

const titleErr = document.querySelector('.title-err');
const descriptionErr = document.querySelector('.description-err');
const deadlineErr = document.querySelector('.deadline-err');
const titleInput = document.querySelector('.t-tile input');
const descriptionInput = document.querySelector('.t-description textarea');
const deadlineInput = document.querySelector('.t-deadline input');
const dialogCancelBtn = document.querySelector('.cancel-btn');
const createTaskForm = document.querySelector('.create-form-wrapper form');

createTaskForm.addEventListener('submit', validateTask);
dialogCancelBtn.addEventListener('click', clearForm);

function validateTask(event) {
    const title = titleInput.value.trim();;
    const description = descriptionInput.value.trim();
    const deadline = deadlineInput.value;
    
    const isTitleValid = title.length >= constants.TITLE_MIN_LENGTH
                      && title.length <= constants.TITLE_MAX_LENGTH;

    const isDescriptionValid = description.length >= constants.TASK_DESCR_MIN_LENGTH
                            && description.length <= constants.TASK_DESCR_MAX_LENGTH;

    const timeDifference = Date.now() - Date.parse(deadline);
    const isDeadlineValid = timeDifference < 0;

    if (!isTitleValid || !isDescriptionValid || !isDeadlineValid) {
        event.preventDefault();

        if (!isTitleValid) {
            titleErr.textContent = constants.TITLE_ERR_MSG;
        } else {
            titleErr.textContent = '';
        }
        if (!isDescriptionValid) {
            descriptionErr.textContent = constants.DESCRIPTION_ERR_MSG;
        } else {
            descriptionErr.textContent = '';
        }
        if (!isDeadlineValid) {
            deadlineErr.textContent = constants.DEADLINE_ERR_MSG;
        } else {
            deadlineErr.textContent = '';
        }
    } else {
        clearErrors();
    }
}

function clearErrors() {
    titleErr.textContent = '';
    descriptionErr.textContent = '';
    deadlineErr.textContent = '';
}

function clearForm() {
    clearErrors();
    titleInput.value = '';
    descriptionInput.value = '';
}