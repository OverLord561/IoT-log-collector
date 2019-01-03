import axios from 'axios';
import * as types from './homeConstants';
import * as globalTypes from '../../../constants/constants';

export const LoadLogData = (date: string, isInitial: boolean) => (dispatch: any, getStore: any) => {
    const URL = globalTypes.IoTServer_BASE_URL.concat(types.LOAD_LOGS_BY_DATE(date, isInitial));

    dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: true,
    });

    console.time("Call back end");

    axios.get(URL)
        .then(response => {

            console.time("Call succeesfully back end ended");

            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            dispatch({
                type: types.LOAD_DEVICE_LOGS,
                chartData: response.data.chartData,
                isInitial,
            });

        }).catch(error => {
            console.time("Call back end ended with erorr");

            console.log(error);
            dispatch({
                type: types.LOAD_DEVICE_LOGS,
                // devicesLogs: data,
                isInitial,
            });
        });
};
