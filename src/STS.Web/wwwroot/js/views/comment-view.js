import { html, render } from '../node_modules/lit-html/lit-html.js';

const commentTemplate = (comment, token) => html`
<section class="t-comment">
    <p>${comment.content}</p>  
    <div class="comment-actions"> 
        <div>
            <span>
                from
                <span class="username">${comment.userUserName}</span>
                on
                <span class="date">${comment.createdOn}</span>
            </span>
        </div>
        <div>
            <button
                class="c-del-btn t-delete-btn"
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

export default function appendComment(token, comment) {
    const container = document.querySelector(".ticket-comments");
    render(commentTemplate(comment, token), container);
};

