const createCommentModal = document.querySelector('.create-task-overlay');
const createBtn = document.querySelector('.create-task-btn.management');
const cancelCommentDialogBtn = document.querySelector('.cancel-btn');

createBtn.addEventListener('click', showModal);
cancelCommentDialogBtn.addEventListener('click', cancelModal);

function showModal() {
    createCommentModal.style.display = 'block';
}

function cancelModal() {
    createCommentModal.style.display = 'none';
}