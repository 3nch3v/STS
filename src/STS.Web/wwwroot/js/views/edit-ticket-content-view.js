import { html, render } from '../../lib/lit-html/lit-html.js';

const ticketContentTemplate = (content, isContentView, requestFunc, isInputValid, editContent, rows) => html`
    ${isContentView
    ? html`<p class="ticket-content">${content}
                ${isContentView
                    ? html`<a @click="${editContent}" class="edit-t-content" href="javascript.void(0)">
                                <i class="fa fa-edit"></i>
                           </a>`
            : null }
          </p>`
        : html`<textarea
                    class="form-control content-edit-ta"
                    @blur="${requestFunc}"
                    rows="${rows}"
                    autofocus
                >${content}</textarea>`
    }
    
    ${isInputValid
        ? null
        : html`<p class="err-msg">Ticket content should be between 5 and 2000 characters.</p>`
     }`;

export default function getContentView(content, isContentView, requestFunc, isInputValid, editContent) {
    const container = document.querySelector(".t-content-section-edited");
    const oldView = document.querySelector(".ticket-body-section");
    oldView.replaceChildren();
    const rows = Math.round(content.length / 115);
    render(ticketContentTemplate(content, isContentView, requestFunc, isInputValid, editContent, rows), container);
};