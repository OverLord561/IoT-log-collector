export const GenerateRandomHex = () => {
    return "#" + Math.random().toString(16).slice(2, 8);
};

export function getCookie(cname: string) {
    const name = cname + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');

    ca.forEach((element, index) => {

        while (element.charAt(0) === ' ') {
            element = element.substring(1);
        }
        if (element.indexOf(name) === 0) {
            return element.substring(name.length, element.length);
        }
    });

    return '';
}

export const IsAuthorized = () => {
    const authCookie: string = getCookie('.AspNetCore.Identity.Application');

    if (!authCookie) {
        return true;
    }

    return false;
};
