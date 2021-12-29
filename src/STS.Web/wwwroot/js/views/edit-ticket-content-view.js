import { html, render } from '../node_modules/lit-html/lit-html.js';

const titleTemplate = (content, isContentView, requestFunc, isInputValid, editTitle, rows) => html`
    ${isContentView
        ? html`<p class="ticket-content">${content}</p>`
        : html`<textarea
                    class="form-control content-edit-ta"
                    @blur="${requestFunc}"
                    rows="${rows}"
                    autofocus
                >${content}</textarea>`
    }

    ${isInputValid
        ? null
        : html`<p>Ticket content should be between 5 and 2000 characters.</p>`
    }

${
    isContentView
        ? html`
            <section class="edit-content-section">
                <a @click="${editTitle}" class="edit-t-content-2" href="javascript.void(0)">
                    <i class="fa fa-edit"></i>
                </a>
            </section>
            `
        : null
}

`;

export default function getContentView(content, isContentView, requestFunc, isInputValid, editTitle) {
    const container = document.querySelector(".t-content-section-edited");
    const oldView = document.querySelector(".ticket-body-section");
    oldView.replaceChildren();
    const rows = Math.round(content.length / 70);
    render(titleTemplate(content, isContentView, requestFunc, isInputValid, editTitle, rows), container);
};