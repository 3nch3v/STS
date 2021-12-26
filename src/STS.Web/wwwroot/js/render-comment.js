import { html, render } from './node_modules/lit-html/lit-html.js';

export default function appendComment(comment) {
    const container = document.querySelector(".ticket-comments");

    const commentTemplate = (comment) => html`
        <section class="t-comment" data-commemt-id=${comment.id}>
            <p>${comment.content}</p>
        
            <div class="comment-actions">
                <div>
                    <span>Edit</span>
                    <span>Delete</span>
                </div>
                <div>
                    <span>
                        From
                        <span class="username">${comment.userUserName}</span>
                        on
                        <span class="date">${comment.createdOn}</span>
                    </span>
                </div>
            </div>
        </section>
        `;

    render(commentTemplate(comment), container);
};

