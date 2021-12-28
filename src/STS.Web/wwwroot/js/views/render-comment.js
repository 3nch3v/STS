import { html, render } from '../node_modules/lit-html/lit-html.js';

export default function appendComment(comment, token) {
    const container = document.querySelector(".ticket-comments");

    const commentTemplate = (comment) => html`
        <section class="t-comment">
            <p>${comment.content}</p>  
            <div class="comment-actions"> 
                <div>
                    <span>
                        From
                        <span class="username">${comment.userUserName}</span>
                        on
                        <span class="date">${comment.createdOn}</span>
                    </span>
                </div>
                <div>
                    <button
                        class="c-del-btn"
                        title="Delete comment."
                        data-comment-id=${comment.id}
                        data-request-token=${token}
                    >
                        <i class="fa fa-trash"></i>
                    </button>
                </div>
            </div>
        </section>
        `;

    render(commentTemplate(comment), container);
};

