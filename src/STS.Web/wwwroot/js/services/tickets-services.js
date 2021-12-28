import { getTicketId, getRequestToken } from './util.js';
import { editTicket } from '../data/data.js';

const status = document.querySelector('span.status');
const statusSelect = document.querySelector('.t-status-select');
const assignToSelect = document.querySelector('.t-assign-select');
const departmentSelect = document.querySelector('.t-department-select');
const assignToMeBtn = document.querySelector('.assign-to-me-btn');
const editTicketTitleBtn = document.querySelector('.edit-t-title');
const editTicketContentBtn = document.querySelector('.edit-t-content');

departmentSelect.addEventListener('change', changeDepartment);
statusSelect.addEventListener('change', changeStatus);
assignToSelect.addEventListener('change', changeEmplyee)

if (editTicketTitleBtn != null) {
    editTicketTitleBtn.addEventListener('click', editTitle)
}
if (editTicketContentBtn != null) {
    editTicketContentBtn.addEventListener('click', editContent)
}

if (assignToMeBtn) assignToMeBtn.addEventListener('click', assignToMe)

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

    changeTextContent('li .assigned-to-username', 'you');
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

    changeTextContent('li .assigned-to-username', assignedToUsername);
}

export function changeStatusIcon(statusName) {
    status.classList.remove(...status.classList);
    status.classList.add('status', statusName.toLowerCase().replace(/\s/g, ''));
    status.textContent = statusName;
}

async function editTitle(event) {
    event.preventDefault();

    const titleSection = document.querySelector('.t-title-section');
    const pTitle = document.querySelector('.ticket-title')

    const ticketId = getTicketId();
    const token = getRequestToken();
    const currTitle = pTitle.textContent;

    const input = document.createElement('input');
    input.setAttribute('type', 'text');
    input.style.backgroundColor = "black";
    input.style.borderColor = "lightgoldenrodyellow";
    input.style.color = "white";
    input.autofocus = true;
    input.value = currTitle;
    input.addEventListener('blur', request);
    input.classList.add('form-control');

    pTitle.remove();
    editTicketTitleBtn.remove();
    titleSection.appendChild(input);

    async function request() {
        pTitle.textContent = input.value;
        input.remove();
        titleSection.appendChild(pTitle);
        document.querySelector('.edit-title-section').appendChild(editTicketTitleBtn);

        if (currTitle !== input.value) {
            await editTicket(token, { id: ticketId, title: pTitle.textContent });
        }
    }
}

async function editContent(event) {
    event.preventDefault();

    const contentSection = document.querySelector('.ticket-content');
    const pTicketContent = document.querySelector('.ticket-content p')

    const ticketId = getTicketId();
    const token = getRequestToken();
    const currContent = pTicketContent.textContent;

    const textarea = document.createElement('textarea');
    textarea.style.backgroundColor = "black";
    textarea.style.borderColor = "lightgoldenrodyellow";
    textarea.style.color = "white";
    textarea.autofocus = true;
    textarea.value = currContent;
    textarea.rows = Math.round(currContent.length / 70);
    textarea.classList.add('form-control');
    textarea.addEventListener('blur', request);

    pTicketContent.remove();
    editTicketContentBtn.remove();

    contentSection.appendChild(textarea);

    async function request() {

        pTicketContent.textContent = textarea.value;
        textarea.remove();
        contentSection.appendChild(pTicketContent);
        document.querySelector('.edit-content-section').appendChild(editTicketContentBtn);

        if (currContent !== textarea.value) {
            await editTicket(token, { id: ticketId, content: pTicketContent.textContent });
        }
    }
}

function changeTextContent(element, value) {
    document.querySelector(element).textContent = `Assigned to ${value}`;
}