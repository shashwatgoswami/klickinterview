import { baseController } from './baseController.js';
import { meaningController } from './meaningController.js';
import { app } from '../app.js';

export class wordController extends baseController {
    constructor (word, parentElement) {
        super();
        this.bindElements(word, parentElement);
    }

    bindElements (word, parentElement) {
        const meanings = word.meanings;
        for (const child of parentElement.children) {
            if (child.getAttribute('data-bind') && child.getAttribute('data-bind') !== '') {
                if (child.getAttribute('data-bind') === 'audio' && word.phonetics && word.phonetics.length > 0 && word.phonetics[0].audio) {
                    child.innerHTML = `<source src="${word.phonetics[0].audio}" type="audio/mpeg">`;
                } else {
                    child.innerHTML = word[child.getAttribute('data-bind')];
                }
            }
        }

        this.renderChildren(meanings, parentElement);
    }

    renderChildren (meanings, parentElement) {
        let meaningsElement = '';
        for (let i = 0; i < meanings.length; i++) {
            const meaning = meanings[i];
            const container = document.createElement('div');
            container.id = `meaningContainer${i}`;
            container.className = 'meaning-flex-item';
            app.renderView('meaning', container);
            new meaningController(meaning, container);
            meaningsElement += container.outerHTML;
        }

        for (const child of parentElement.children) {
            if (child.id === 'meaningCollection') {
                child.innerHTML = meaningsElement;
            }
        }
    }
}
