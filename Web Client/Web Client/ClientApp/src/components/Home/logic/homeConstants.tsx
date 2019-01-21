export const LOAD_DEVICE_LOGS = 'LOAD_DEVICE_LOGS';
export const LOAD_LOGS_BY_DATE = (date: string, isInitial: boolean) => {
    //return `get-logs?utcDate=${date}&isInitial=${isInitial}`;
    return `get-logs?utcDate=&isInitial=${isInitial}`;
};
export const SERVER_SETTINGS = 'SERVER_SETTINGS';
export const GET_SERVER_SETTINGS_URL = 'get-sever-settings';
export const SET_SERVER_SETTINGS_URL = 'update-sever-settings';
