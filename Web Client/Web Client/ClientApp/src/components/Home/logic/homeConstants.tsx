export const LOAD_DEVICE_LOGS = 'LOAD_DEVICE_LOGS';
export const LOAD_LOGS_BY_DATE = (date: string, isInitial: boolean) => {
    //return `get-logs?utcDate=${date}&isInitial=${isInitial}`;
    return `get-logs?utcDate=&isInitial=${isInitial}`;

};
