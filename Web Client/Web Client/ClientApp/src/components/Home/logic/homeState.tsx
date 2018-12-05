import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    devicesLogs: IDeviceLogsUIFormat[];
}

export interface IDeviceLogsUIFormat {
    deviceName: string,
    logs: ILog[]
}

export interface ILog {
    hour: string,
    temperature: number,
    humidity: number,
}

export const getInitialState = (): IState => {
    return {
        devicesLogs: [],
        errors: [],
        isFetching: false
    };
};
