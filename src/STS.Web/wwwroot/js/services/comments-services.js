import appendComment from '../views/render-comment.js';
import { getTicketId, getRequestToken, modal, } from './util.js';
import { createComment, deleteCommentById } from '../data/data.js';
import { changeStatusIcon } from './tickets-services.js';

modal('.t-comment-btn', '.comment-dialog', '.cancel-c-btn');

const commentForm = document.querySelector('.comment-form');
commentForm.addEventListener('click', postCommen);

const comments = document.querySelector('.ticket-comments');
comments.addEventListener('click', deleteComment);

async function postCommen(event) {
    event.preventDefault();

    const form = new FormData(commentForm);
    const comment = form.get("comment").trim();

    if (comment.length <= 2 || comment.length > 2000) {
        return;
    }

    const ticketId = getTicketId();
    const token = getRequestToken();
    const commentDto = {
        content: comment,
        ticketId: ticketId
    }

    const data = await createComment(token, commentDto);

    changeStatusIcon('Open');
    appendComment(data, token);
    commentForm.reset();
    document.querySelector('.comment-dialog').close('Cancel');
};

async function deleteComment(event) {
    const currTaget = event.target;

    if (currTaget.tagName === 'BUTTON'
        || (currTaget.tagName === 'I' && currTaget.parentElement.tagName === 'BUTTON')) {

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