import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    chartData?: IDeviceLogsInChartFormat;
    isInitial: boolean;
    serverSettings: IServerSettingViewModel[],
    dataStoragePlugins: string[];
}

export interface IDeviceLogsInChartFormat {
    chartName: string,
    axesNames: string[],
    logs: ILog[]
}

export interface ILog {
    values: number[];
}

export interface IServerSettingViewModel {
    name: string;
    value: number;
    displayName: string;
    isEditable: boolean;
}

export interface IDataStoragePlugin {
    displayName: string;
    value: string;
    isSelected: boolean;
}

export const getInitialState = (): IState => {
    return {
        isInitial: true,
        errors: [],
        isFetching: false,
        serverSettings: [],
        dataStoragePlugins: []
    };
};
