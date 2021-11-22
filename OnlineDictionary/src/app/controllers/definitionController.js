import { baseController } from './baseController.js';

export class definitionController extends baseController {
    constructor (definition, parentElement) {
        super();
        this.bindElements(definition, parentElement);
    }
}
