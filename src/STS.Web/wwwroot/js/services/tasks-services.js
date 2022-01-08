import { editTask, createTaskComment } from '../data/data.js';
import { createHtmlElement } from '../services/util.js';

const taskData = document.querySelector('.task-details');
const submitBtn = document.querySelector('.c-btn.task-btn');
const statusSelect = document.querySelector('.task-status-select');

statusSelect.addEventListener('change', changeStatus);
submitBtn.addEventListener('click', replayTask);

async function changeStatus() {
    const status = document.querySelector('p.status');
    const taskId = taskData.dataset.taskId;
    const token = taskData.dataset.requestToken;
    const statusId = statusSelect.value;
    const statusName = statusSelect.options[statusSelect.selectedIndex].text;

    await editTask(token, { id: taskId, statusId: statusId });

    status.classList.remove(...status.classList);
    status.classList.add('status', statusName.toLowerCase().replace(/\s/g, ''));
    status.textContent = statusName;
}

async function replayTask() {
    const commentInput = document.querySelector('.comment-task-ta');
    const tasksComments = document.querySelector('.comments');
    const errMsg = document.querySelector('.replay-validation');
    const comment = commentInput.value.trim();

    if (comment.length < 2 || comment.length > 1000) {
        errMsg.textContent = 'Task replay should be between 2 and 1000 characters.';
        return;
    } else {
        errMsg.textContent = '';
    }

    const token = taskData.dataset.requestToken;
    const taskId = taskData.dataset.taskId;
    const result = await createTaskComment(token, { employeeTaskId: taskId, content: comment });

    const newComment = createCommentHtml(result.id, result.content, result.userUserName)
    tasksComments.appendChild(newComment);
    commentInput.value = "";
}

function createCommentHtml(commentId, commentContent, username) {
    const section = createHtmlElement('section', { className: "task-comment" });
    section.dataset.taskId = commentId;
    section.appendChild(createHtmlElement('p', { className: 'task-content' }, commentContent));
    section.appendChild(createHtmlElement('p', { className: 'task-manager username comment' },
                            createHtmlElement('span', { className: 'from-user' }, 'from '),
                            username)
    );

    return section;
}