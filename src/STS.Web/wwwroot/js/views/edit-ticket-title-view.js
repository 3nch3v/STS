import { html, render } from '../node_modules/lit-html/lit-html.js';

const titleTemplate = (title, isTitleView, requestFunc, isInputValid, editTitle) => html`
    ${isTitleView
        ? html`<p class="ticket-title">${title}</p>`
        : html`<input
                    class="form-control title-edit-input"
                    type="text"
                    value=${title} 
                    @blur="${requestFunc}"
                    autofocus
                >
                </input>`
    }

    ${isInputValid
        ? null
        : html`<p>Title should be between 2 and 100 characters.</p>`
    }

${
    isTitleView
        ? html`
            <section class="edit-title-section">
                <a @click="${editTitle}" class="edit-t-title" href="javascript.void(0)">
                    <i class="fa fa-edit"></i>
                </a>
            </section>
            `
        : null
}

`;

export default function getTitleView(title, isTitleView, requestFunc, isInputValid, editTitle) {
    const container = document.querySelector(".t-title-section-edited");
    const oldView = document.querySelector(".t-title-section");
    oldView.replaceChildren();
    render(titleTemplate(title, isTitleView, requestFunc, isInputValid, editTitle), container);
};








