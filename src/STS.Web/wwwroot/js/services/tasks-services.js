import constants from '../constants.js';
import { editTask, createTaskComment } from '../data/data.js';
import { createHtmlElement, displayMessage } from '../services/util.js';

const taskData = document.querySelector('.task-details');
const submitBtn = document.querySelector('.c-btn.task-btn');
const selectStatus = document.querySelector('.task-status-select');
const employeeSelect = document.querySelector('.t-employee-select');
const status = document.querySelector('p.status');

selectStatus.addEventListener('change', changeStatus);
submitBtn.addEventListener('click', replyTask);
if (employeeSelect) employeeSelect.addEventListener('change', changeEmplyee);

async function changeEmplyee() {
    const taskId = taskData.dataset.taskId;
    const token = taskData.dataset.requestToken;
    const employeeId = employeeSelect.value;
    const employeeName = employeeSelect.options[employeeSelect.selectedIndex].text;
    await editTask(token, { id: taskId, employeeId: employeeId });
    displayMessage(constants.TASK_ASSIGN_MSG(employeeName));
}

async function changeStatus() {
    const taskId = taskData.dataset.taskId;
    const token = taskData.dataset.requestToken;
    const statusId = selectStatus.value;
    const statusName = selectStatus.options[selectStatus.selectedIndex].text;
    await editTask(token, { id: taskId, statusId: statusId });
    changeStatusName(statusName);
    displayMessage(constants.CHANGE_STATUS_MSG(statusName));
}

async function replyTask() {
    const commentInput = document.querySelector('.comment-task-ta');
    const tasksComments = document.querySelector('.comments');
    const errMsg = document.querySelector('.reply-validation');
    const noTasks = document.querySelector('.no-task-comments');
    
    const comment = commentInput.value.trim();

    if (comment.length < constants.REPLY_MIN_LENGTH
        || comment.length > constants.REPLY_MAX_LENGTH) {

        errMsg.textContent = constants.TASK_REPLAY_ERR_MSG;
        return;
    } else {
        errMsg.textContent = '';
    }

    const token = taskData.dataset.requestToken;
    const taskId = taskData.dataset.taskId;
    const statusId = Array.from(selectStatus.options).find(x => x.textContent.trim() == 'Open').value;
    const result = await createTaskComment(token, { employeeTaskId: taskId, content: comment });
    await editTask(token, { id: taskId, statusId: statusId });
    const newComment = createCommentHtml(result.id, result.content, result.userUserName)
    tasksComments.appendChild(newComment);
    commentInput.value = '';
    const openStatusIndex = [...selectStatus.options].find(x => x.textContent == "Open");
    if (openStatusIndex) {
        selectStatus.value = openStatusIndex.value;
    }
    changeStatusName('Open')
    if (noTasks) {
        noTasks.remove();
    }
}

function changeStatusName(statusName) {
    status.classList.remove(...status.classList);
    status.classList.add('status', statusName.toLowerCase().replace(/\s/g, ''));
    status.textContent = statusName;
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