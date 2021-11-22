export class baseController {
    bindElements (data, parentElement) {
        if (data && parentElement) {
            for (const child of parentElement.children) {
                if (child.getAttribute('data-bind') && child.getAttribute('data-bind') !== '' && data[child.getAttribute('data-bind')]) {
                    child.innerHTML = data[child.getAttribute('data-bind')];
                }
            }
        }
    }
}
