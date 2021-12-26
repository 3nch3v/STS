import appendComment from './render-comment.js';

(function () {
    const showModal = document.querySelector('.t-comment-btn');
    const dialog = document.querySelector('.comment-dialog');
    const cancelBtn = document.querySelector('.cancel-c-btn');
    const commentForm = document.querySelector('.comment-form');

    showModal.addEventListener('click', openDialog);
    cancelBtn.addEventListener('click', closeDialog);
    commentForm.addEventListener('click', postCommen)

    async function postCommen(event) {
        event.preventDefault();

        const form = new FormData(commentForm);
        const comment = form.get("comment").trim();

        if (comment.length <= 2 || comment.length > 2000) {
            return;
        }

        const ticketId = commentForm.dataset.ticketId;
        const token = commentForm.dataset.requestToken;

        const url = "https://localhost:5001/api/comment"

        const res = await fetch(url, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "RequestVerificationToken": token
            },
            body: JSON.stringify({ content: comment, ticketId})
        });

        const data = await res.json();
        appendComment(data);
        commentForm.reset();
        dialog.close('Cancel');
    };

    function openDialog() {
        if (!dialog.open) {
            dialog.showModal();
        }
    };

    function closeDialog() {
        dialog.close('Cancel');
    };
})();

