import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    deviceLogs: IDeviceLogs;
}

export interface IDeviceLogs {
    deviceName: string;
    logs: string[];
}

export const getInitialState = (): IState => {
    return {
        deviceLogs: {
            deviceName: '',
            logs: []
        },
        errors: [],
        isFetching: false
    };
};
