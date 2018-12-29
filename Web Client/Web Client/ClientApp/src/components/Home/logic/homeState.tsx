import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    chartData?: IDeviceLogsInChartFormat;
    isInitial: boolean;
}

export interface IDeviceLogsInChartFormat {
    chartName: string,
    axesNames: string[],
    logs: ILog[]
}

export interface ILog {
    values: number[];
}

export const getInitialState = (): IState => {
    return {
        isInitial: true,
        errors: [],
        isFetching: false
    };
};
