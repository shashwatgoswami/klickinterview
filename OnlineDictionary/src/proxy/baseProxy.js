export class baseProxy {
    async fetchData (url, method) {
        const responseAsJson = await new Promise((resolve, reject) => fetch(url, {
            method: method
        }).then((response) => {
            if (response.status === 200) {
                resolve(response.json());
            } else {
                resolve('error');
            }
        }));

        return responseAsJson;
    }
}
