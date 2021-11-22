import { app } from './app.js';
export class main {
    run () {
        const allRoutes = app.getRoutes();
        let route = allRoutes[0]; // default route
        if (window.location.search !== '' && window.location.search !== '/') {
            route = allRoutes.find(r => r.name === window.location.search);
        }
        app.executeRoute(route);
    }
}
