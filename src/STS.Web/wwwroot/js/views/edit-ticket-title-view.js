import { html, render } from '../node_modules/lit-html/lit-html.js';

const titleTemplate = (title, isTitleView, requestFunc, isInputValid, editTitle) => html`
${isTitleView
    ? html`<p class="ticket-title">${title}
                ${isTitleView
                    ? html`<a  @click="${editTitle}" class="edit-t-title" href="javascript.void(0)">
                                <i class="fa fa-edit"></i>
                           </a>
                    `
                    : null
                }
          </p>`
    : html`<input
                class="form-control title-edit-input"
                type="text"
                value=${title} 
                @blur="${requestFunc}"
                autofocus
            > `
}
${isInputValid
    ? null
    : html`<p class="text-danger">Title should be between 2 and 100 characters.</p>`
}
`;

export default function getTitleView(title, isTitleView, requestFunc, isInputValid, editTitle) {
    const container = document.querySelector(".t-title-section-edited");
    const oldView = document.querySelector(".t-title");
    oldView.replaceChildren();

    render(titleTemplate(title, isTitleView, requestFunc, isInputValid, editTitle), container);
};








