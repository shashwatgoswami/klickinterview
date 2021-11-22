import { searchController } from './controllers/searchController.js';

export class app {
    static getRoutes () {
        return [
            { name: '/', view: 'search', controller: () => new searchController() }
        ];
    }

    static executeRoute (route) {
        const viewId = route.view;
        this.renderView(viewId, document.getElementById('app'));
        route.controller();
    }

    static renderView (viewId, container) {
        const template = document.getElementById(viewId);
        if (template && template.innerHTML && template.nodeName === 'SCRIPT') {
            container.innerHTML = template.innerHTML;
        }
    }
}
