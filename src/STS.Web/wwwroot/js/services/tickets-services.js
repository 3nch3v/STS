import { getTicketId, getRequestToken, displayMessage } from './util.js';
import { editTicket, getEmployees } from '../data/data.js';
import getTitleView from '../views/edit-ticket-title-view.js';
import getContentView from '../views/edit-ticket-content-view.js';

const status = document.querySelector('span.status');
const statusSelect = document.querySelector('.t-status-select');
const assignToSelect = document.querySelector('.t-employee-select');
const departmentSelect = document.querySelector('.t-department-select');
const createDepartmentSelect = document.querySelector('.create-f-dep-select');
const assignToMeBtn = document.querySelector('.assign-to-me-btn');
const editTicketTitleBtn = document.querySelector('.edit-t-title');
const editTicketContentBtn = document.querySelector('.edit-t-content');

if (departmentSelect) departmentSelect.addEventListener('change', changeDepartment);
if (assignToSelect) assignToSelect.addEventListener('change', changeEmplyee);
if (statusSelect) statusSelect.addEventListener('change', changeStatus);
if (editTicketContentBtn) editTicketContentBtn.addEventListener('click', editContent)
if (assignToMeBtn) assignToMeBtn.addEventListener('click', assignToMe)
if (editTicketTitleBtn) editTicketTitleBtn.addEventListener('click', editTitle)
if (createDepartmentSelect) createDepartmentSelect.addEventListener('change', showEmployeeSelect)

async function showEmployeeSelect() {
    const employeeSelect = document.querySelector('.create-f-empl-select');
    const selectDiv = document.querySelector('.create-f-empl-select-div');
    const createForm = document.querySelector('.create-form-wrapper');
    const token = createForm.dataset.requestToken;
    selectDiv.style.display = 'block';
    await loadDepartmentEmployees(createDepartmentSelect, employeeSelect, token);
}

async function loadDepartmentEmployees(department, assignTo, token) {
    const departmentId = department.value;
    const employees = await getEmployees(departmentId, token);
    assignTo.options.length = 0;
    var option = document.createElement("option");
    option.text = "Select employee";
    assignTo.add(option, assignTo[0]);
    employees.forEach(({ id, userName }) =>
        assignTo.options[assignTo.options.length] = new Option(userName, id)
    );
}

async function changeDepartment() {
    const deleteBtn = document.querySelector('.t-delete-btn');
    const ticketId = getTicketId();
    const token = getRequestToken();
    const departmentId = departmentSelect.value;
    const departmentName = departmentSelect.options[departmentSelect.selectedIndex].text;
    await editTicket(token, { id: ticketId, departmentId: departmentId });

    if (deleteBtn) {
        deleteBtn.remove();
    }
    if (assignToMeBtn) {
        assignToMeBtn.remove();
    }
    if (editTicketTitleBtn) {
        editTicketTitleBtn.remove();
    }
    if (editTicketContentBtn) {
        editTicketContentBtn.remove();
    }
    statusSelect.disabled = true;
    changeStatusIcon('Open');
    changeTextContent('.assigned-to-username .name-prefix', 'unassigned');
    changeTextContent('.assigned-to-username .username', '');
    displayMessage(`Ticket department has been changed to ${departmentName}`);

    await loadDepartmentEmployees(departmentSelect, assignToSelect, token);
}

async function assignToMe(event) {
    event.preventDefault();
    const currUser = event.target.dataset.myId;
    const ticketId = getTicketId();
    const token = getRequestToken();  
    await editTicket(token, { id: ticketId, AssignedToId: currUser });
    assignToMeBtn.remove();
    assignToSelect.value = currUser;
    const currUserName = assignToSelect.options[assignToSelect.selectedIndex].text;
    changeTextContent('.assigned-to-username .username', currUserName);
    displayMessage(`Ticket has been assigned to you`);
}

async function changeStatus() {
    const ticketId = getTicketId();
    const token = getRequestToken();
    const statusId = statusSelect.value;
    const statusName = statusSelect.options[statusSelect.selectedIndex].text;
    await editTicket(token, { id: ticketId, statusId: statusId});
    changeStatusIcon(statusName);
    displayMessage(`Status has been chanded to ${statusName}`);
}

async function changeEmplyee() {
    const ticketId = getTicketId();
    const token = getRequestToken();
    const assignToId = assignToSelect.value;
    const assignedToUsername = assignToSelect.options[assignToSelect.selectedIndex].text;
    await editTicket(token, { id: ticketId, assignedToId: assignToId });
    changeTextContent('.assigned-to-username .name-prefix', "Assigned to");
    changeTextContent('.assigned-to-username .username', assignedToUsername);
    displayMessage(`Ticket has been assigned to ${assignedToUsername}`);
}

async function editTitle(event) {
    event.preventDefault();
    const currTitle = document.querySelector('.ticket-title').textContent.trim();
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
            displayMessage(`Ticket title has been edited`);
        } else {
            renderTitle(input, true, true);
        }
    }

    function renderTitle(title, isTitleView, isInputValid) {
        titlePartialView(title, isTitleView, request, isInputValid)
    }
}

async function editContent(event) {
    event.preventDefault();
    const currContent = document.querySelector('.ticket-content').textContent.trim();
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
            displayMessage(`Ticket description has been edited`);
        } else {
            renderContent(input, true, true);
        }
    }

    function renderContent(content, isContentView, isInputValid) {
        contentPatialView(content, isContentView, request, isInputValid)
    }
}

function titlePartialView(title, isTitleView, request, isInputValid) {
    getTitleView(title, isTitleView, request, isInputValid, editTitle);
}

function contentPatialView(content, isTitleView, request, isInputValid) {
    getContentView(content, isTitleView, request, isInputValid, editContent);
}

function changeTextContent(element, value) {
    document.querySelector(element).textContent = value;
}

export function changeStatusIcon(statusName) {
    status.classList.remove(...status.classList);
    status.classList.add('status', statusName.toLowerCase().replace(/\s/g, ''));
    status.textContent = statusName;
}