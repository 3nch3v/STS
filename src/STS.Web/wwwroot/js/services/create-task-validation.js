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
    
    const isTitleValid = title.length >= 2 && title.length <= 100;
    const isDescriptionValid = description.length >= 5 && description.length <= 1000;
    const timeDifference = Date.now() - Date.parse(deadline);
    const isDeadlineValid = timeDifference < 0;

    if (!isTitleValid || !isDescriptionValid || !isDeadlineValid) {

        event.preventDefault();

        if (!isTitleValid) {
            titleErr.textContent = 'Title length should be between 2 and 100 characters.';
        } else {
            titleErr.textContent = '';
        }
        if (!isDescriptionValid) {
            descriptionErr.textContent = 'Description length should be between 5 and 1000 characters.';
        } else {
            descriptionErr.textContent = '';
        }
        if (!isDeadlineValid) {
            deadlineErr.textContent = 'Deadline should be at a later time.';
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