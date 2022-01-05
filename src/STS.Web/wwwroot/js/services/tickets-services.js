import { getTicketId, getRequestToken } from './util.js';
import { editTicket } from '../data/data.js';
import getTitleView from '../views/edit-ticket-title-view.js';
import getContentView from '../views/edit-ticket-content-view.js';

const status = document.querySelector('span.status');
const statusSelect = document.querySelector('.t-status-select');
const assignToSelect = document.querySelector('.t-employee-select');
const departmentSelect = document.querySelector('.t-department-select');
const assignToMeBtn = document.querySelector('.assign-to-me-btn');
const editTicketTitleBtn = document.querySelector('.edit-t-title');
const editTicketContentBtn = document.querySelector('.edit-t-content');

departmentSelect.addEventListener('change', changeDepartment);
statusSelect.addEventListener('change', changeStatus);
assignToSelect.addEventListener('change', changeEmplyee)

if (editTicketContentBtn) editTicketContentBtn.addEventListener('click', editContent)
if (assignToMeBtn) assignToMeBtn.addEventListener('click', assignToMe)
if (editTicketTitleBtn) editTicketTitleBtn.addEventListener('click', editTitle)

async function changeDepartment() {
    const ticketId = getTicketId();
    const token = getRequestToken();
    const departmentId = departmentSelect.value;

    await editTicket(token, { id: ticketId, departmentId: departmentId });
}

async function assignToMe(event) {
    event.preventDefault();

    const myId = event.target.dataset.myId;
    const ticketId = getTicketId();
    const token = getRequestToken();
    
    await editTicket(token, { id: ticketId, AssignedToId: myId });

    changeTextContent('.assigned-to-username .username', 'you');
}

async function changeStatus() {
    const ticketId = getTicketId();
    const token = getRequestToken();

    const statusId = statusSelect.value;
    const statusName = statusSelect.options[statusSelect.selectedIndex].text;

    await editTicket(token, { id: ticketId, statusId: statusId});

    changeStatusIcon(statusName);
}

async function changeEmplyee() {
    const ticketId = getTicketId();
    const token = getRequestToken();

    const assignToId = assignToSelect.value;
    const assignedToUsername = assignToSelect.options[assignToSelect.selectedIndex].text;

    await editTicket(token, { id: ticketId, assignedToId: assignToId });

    changeTextContent('.assigned-to-username .username', assignedToUsername);
}

export function changeStatusIcon(statusName) {
    status.classList.remove(...status.classList);
    status.classList.add('status', statusName.toLowerCase().replace(/\s/g, ''));
    status.textContent = statusName;
}

async function editTitle(event) {
    event.preventDefault();
    const currTitle = document.querySelector('.ticket-title').textContent;
    renderTitle(currTitle, false, true);

    async function request() {
        const input = document.querySelector('.title-edit-input').value;
        if (input.length < 2 || input.length > 100) {
            renderTitle(input, false, false);
            return;
        }

        if (currTitle !== input) {
            const ticketId = getTicketId();
            const token = getRequestToken();
            await editTicket(token, { id: ticketId, title: input });
            renderTitle(input, true, true);
        } else {
            renderTitle(input, true, true);
        }
    }

    function renderTitle(title, isTitleView, isInputValid) {
        titlePartialView(title, isTitleView, request, isInputValid)
    }
}

function titlePartialView(title, isTitleView, request, isInputValid) {
    getTitleView(title, isTitleView, request, isInputValid, editTitle);
}

async function editContent(event) {
    event.preventDefault();
    const currContent = document.querySelector('.ticket-content').textContent;
    renderContent(currContent, false, true);

    async function request() {
        const input = document.querySelector('.content-edit-ta').value;
        if (input.length < 5 || input.length > 2000) {
            renderContent(input, false, false);
            return;
        }

        if (currContent !== input) {
            const ticketId = getTicketId();
            const token = getRequestToken();
            await editTicket(token, { id: ticketId, content: input });
            renderContent(input, true, true);
        } else {
            renderContent(input, true, true);
        }
    }

    function renderContent(content, isContentView, isInputValid) {
        contentPatialView(content, isContentView, request, isInputValid)
    }
}

function contentPatialView(content, isTitleView, request, isInputValid) {
    getContentView(content, isTitleView, request, isInputValid, editContent);
}

function changeTextContent(element, value) {
    document.querySelector(element).textContent = `Assigned to ${value}`;
}