import constants from '../constants.js';
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
const createTicketModal = document.querySelector('.create-dialog-overlay');
const createBtn = document.querySelector('.show-create-btn');
const cancelBtn = document.querySelector('.cancel-btn');

if (departmentSelect) departmentSelect.addEventListener('change', changeDepartment);
if (assignToSelect) assignToSelect.addEventListener('change', changeEmplyee);
if (statusSelect) statusSelect.addEventListener('change', changeStatus);
if (editTicketContentBtn) editTicketContentBtn.addEventListener('click', editContent)
if (assignToMeBtn) assignToMeBtn.addEventListener('click', assignToMe)
if (editTicketTitleBtn) editTicketTitleBtn.addEventListener('click', editTitle)
if (createDepartmentSelect) createDepartmentSelect.addEventListener('change', showEmployeeSelect)
createBtn.addEventListener('click', showModal);
cancelBtn.addEventListener('click', cancelModal);

function showModal() {
    createTicketModal.style.display = 'block';
}

function cancelModal() {
    createTicketModal.style.display = 'none';
}

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
    option.text = constants.SELECT_EMPL_DEF;
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
    const currStatus = statusSelect.options[statusSelect.selectedIndex].text;

    if (currStatus != 'New reply') {
        const openStatusIndex = [...statusSelect.options].find(x => x.textContent == "Open");
        statusSelect.value = openStatusIndex.value;
        changeStatusIcon('Open');
    }

    changeTextContent('.assigned-to-username .name-prefix', 'unassigned');
    changeTextContent('.assigned-to-username .username', '');
    displayMessage(constants.CHANGE_DEP_MSG(departmentName));

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
    displayMessage(constants.TICKET_ASSIGN_MSG(currUserName));
}

async function changeStatus() {
    const ticketId = getTicketId();
    const token = getRequestToken();
    const statusId = statusSelect.value;
    const statusName = statusSelect.options[statusSelect.selectedIndex].text;
    await editTicket(token, { id: ticketId, statusId: statusId});
    changeStatusIcon(statusName);
    displayMessage(constants.CHANGE_STATUS_MSG(statusName));
}

async function changeEmplyee() {
    const ticketId = getTicketId();
    const token = getRequestToken();
    const assignToId = assignToSelect.value;
    const assignedToUsername = assignToSelect.options[assignToSelect.selectedIndex].text;
    await editTicket(token, { id: ticketId, assignedToId: assignToId });
    changeTextContent('.assigned-to-username .name-prefix', "Assigned to");
    changeTextContent('.assigned-to-username .username', assignedToUsername);
    displayMessage(constants.TICKET_ASSIGN_MSG(assignedToUsername));
}

async function editTitle(event) {
    event.preventDefault();
    const currTitle = document.querySelector('.ticket-title').textContent.trim();
    renderTitle(currTitle, false, true);

    async function request() {
        const input = document.querySelector('.title-edit-input').value;
        if (input.length < constants.TITLE_MIN_LENGTH
           || input.length > constants.TITLE_MAX_LENGTH) {
            renderTitle(input, false, false);
            return;
        }

        if (currTitle !== input) {
            const ticketId = getTicketId();
            const token = getRequestToken();
            await editTicket(token, { id: ticketId, title: input });
            renderTitle(input, true, true);
            displayMessage(constants.TICKET_TITLE_EDIT_MSG);
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
        if (input.length < constants.TICKET_DESCR_MIN_LENGTH
           || input.length > constants.TICKET_DESCR_MAX_LENGTH) {
            renderContent(input, false, false);
            return;
        }

        if (currContent !== input) {
            const ticketId = getTicketId();
            const token = getRequestToken();
            await editTicket(token, { id: ticketId, content: input });
            renderContent(input, true, true);
            displayMessage(constants.TICKET_DESCR_EDIT_MSG);
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