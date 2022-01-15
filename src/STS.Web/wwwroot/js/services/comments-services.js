import constants from '../constants.js';
import appendComment from '../views/comment-view.js';
import { getTicketId, getRequestToken, modal, } from './util.js';
import { createComment, deleteCommentById } from '../data/data.js';
import { changeStatusIcon } from './tickets-services.js';

modal('.t-comment-btn', '.comment-dialog', '.cancel-c-btn');

const commentForm = document.querySelector('.comment-form');
const submitBtn = commentForm.querySelector('button.c-btn');
const cancelBtn = commentForm.querySelector('button.cancel-c-btn');
const comments = document.querySelector('.ticket-comments');
const commentValidationMsg = commentForm.querySelector('.comment-validation');

submitBtn.addEventListener('click', postCommen);
cancelBtn.addEventListener('click', cleanForm);
comments.addEventListener('click', deleteComment);

async function postCommen(event) {
    event.preventDefault();

    const form = new FormData(commentForm);
    const comment = form.get('comment').trim();
    const checkbox = document.getElementById('send-email-check');
    const sendEmail = checkbox.checked;

    if (comment.length < constants.COMMENT_MIN_LENGTH
        || comment.length > constants.COMMENT_MAX_LENGTH) {
        commentValidationMsg.textContent = constants.COMMENT_ERR_MSG;
        return;
    } else {
        commentValidationMsg.textContent = '';
    }

    const ticketId = getTicketId();
    const token = getRequestToken();

    const commentDto = {
        content: comment,
        ticketId: ticketId,
        sendEmail: sendEmail,
    }

    const data = await createComment(token, commentDto);

    const statusSelect = document.querySelector('.t-status-select');
    const replyStatusIndex = [...statusSelect.options].find(x => x.textContent == "New reply");
    statusSelect.value = replyStatusIndex.value;
    changeStatusIcon('New reply');
    appendComment(token, data);
    const commentSection = document.querySelector('.new-comment');
    const commentChildren = [...commentSection.children];
    const commentCopy = commentChildren[commentChildren.length - 1].cloneNode(true);
    comments.appendChild(commentCopy);
    commentForm.reset();
    closeDialog();
};

async function deleteComment(event) {
    const currTaget = event.target;

    if (currTaget.tagName === 'BUTTON'
        || (currTaget.tagName === 'I' && currTaget.parentElement.tagName === 'BUTTON')) {

        if (confirm(constants.DEL_COMMENT_MSG) == true) {
            let commentWrapper = currTaget.parentElement.parentElement.parentElement;
            let commentId;

            if (currTaget.tagName === 'I') {
                commentId = currTaget.parentElement.dataset.commentId;
                commentWrapper = commentWrapper.parentElement;
            } else {
                commentId = currTaget.dataset.commentId;
            }

            let token = getRequestToken();
            await deleteCommentById(commentId, token);
            commentWrapper.remove();
        }
    }
}

function cleanForm() {
    commentForm.reset();
    commentValidationMsg.textContent = '';
    closeDialog();
}

function closeDialog() {
    document.querySelector('.comment-dialog').close('Cancel');
}