export const LOAD_DEVICE_LOGS = 'LOAD_DEVICE_LOGS';
export const LOAD_LOGS_BY_DATE = (date: string, pluginName: string, isInitial: boolean) => {
    //return `get-logs?utcDate=${date}&isInitial=${isInitial}`;
    return `get-logs?utcDate=&isInitial=${isInitial}&deviceName=${pluginName}`;
};
export const SERVER_SETTINGS = 'SERVER_SETTINGS';
export const GET_SERVER_SETTINGS_URL = 'get-sever-settings';
export const DATASTORAGE_PLUGINS = 'DATASTORAGE_PLUGINS';
export const GET_DATASTORAGE_PLUGINS_URL = 'get-datastorageplugins-settings';
export const UPDATE_SERVER_SETTINGS_URL = 'update-sever-settings';
export const UPDATE_DATASTORAGE_PLUGINS_URL = 'update-datastorageplugins-settings';
export const DEVICE_PLUGINS = 'DEVICE_PLUGINS';
export const GET_DEVICE_PLUGINS_URL = 'get-deviceplugins';
