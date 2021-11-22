import { baseController } from './baseController.js';
import { definitionController } from './definitionController.js';
import { app } from '../app.js';

export class meaningController extends baseController {
    constructor (meaning, parentElement) {
        super();
        this.bindElements(meaning, parentElement);
        const definitions = meaning.definitions;
        this.renderChildren(definitions, parentElement);
    }

    renderChildren (definitions, parentElement) {
        let definitionsElement = '';
        for (let i = 0; i < definitions.length; i++) {
            const definition = definitions[i];
            const container = document.createElement('div');
            container.id = `definitionContainer${i}`;
            container.className = 'definition-flex-item';
            app.renderView('definition', container);
            new definitionController(definition, container);
            definitionsElement += container.outerHTML;
        }

        for (const child of parentElement.children) {
            if (child.id === 'definitionCollection') {
                child.innerHTML = definitionsElement;
            }
        }
    }
}
