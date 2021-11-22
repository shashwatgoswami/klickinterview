import { baseController } from './baseController.js';
import { dictionaryProxy } from '../../proxy/dictionaryProxy.js';
import { wordController } from './wordController.js';
import { app } from '../app.js';

export class searchController extends baseController {
    constructor () {
        super();
        this.bindElements(undefined, undefined);
        const searchBtn = document.getElementById('btnSearch');
        searchBtn.addEventListener('click', async () => await this.searchMeaning());
    }

    async searchMeaning (eventArgs) {
        const word = document.getElementById('txtSearch').value;
        const proxy = new dictionaryProxy();
        const words = await proxy.getMeanings(word);
        if (words !== 'error') {
            this.renderChildren(words);
        } else {
            document.getElementById('wordCollection').innerHTML = 'No definitions found.';
        }
    }

    renderChildren (words) {
        let wordsElement = '';
        for (let i = 0; i < words.length; i++) {
            const word = words[i];
            const container = document.createElement('div');
            container.id = `container${i}`;
            container.className = 'word-flex-item';
            app.renderView('word', container);
            new wordController(word, container);
            wordsElement += container.outerHTML;
        }

        document.getElementById('wordCollection').innerHTML = wordsElement;
    }
}
