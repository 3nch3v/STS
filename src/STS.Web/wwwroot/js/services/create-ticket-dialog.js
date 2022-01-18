import dialogPolyfill from '../node_modules/dialog-polyfill/dist/dialog-polyfill.esm.js';
import { modal } from './util.js';

const createTicketDialog = document.querySelector('.create-dialog');
const commentDialog = document.querySelector('.comment-dialog');

dialogPolyfill.registerDialog(createTicketDialog);
if (commentDialog) dialogPolyfill.registerDialog(commentDialog);

modal('.show-create-btn', '.create-dialog', '.cancel-btn');