import { baseProxy } from './baseProxy.js';
import { methodType } from './methodType.js';

export class dictionaryProxy extends baseProxy {
    constructor (url = undefined) {
        super();
        if (url) {
            this.url = url;
        } else {
            this.url = 'https://api.dictionaryapi.dev/api/v2/entries/en_US/';
        }
    }

    /**
     * gets the meanings of the word
     * @param {string} word meaning to be searched
     */
    async getMeanings (word) {
        const request = this.url + word;
        const response = await this.fetchData(request, methodType.GET);
        return response;
    }
}
