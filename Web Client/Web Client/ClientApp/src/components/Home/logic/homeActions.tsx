import axios from 'axios';
import * as types from './homeConstants';
import * as globalTypes from '../../../constants/constants';
// import { DeviceLogsInChartFormat } from './homeState';

// const samsungLogs: DeviceLogsInChartFormat = {
//     deviceName: 'Samsung. Back end error',
//     logs: [
//         { hour: 'Page A', temperature: 4000, humidity: 2400, },
//         { hour: 'Page B', temperature: 3000, humidity: 1398 },
//         { hour: 'Page C', temperature: 2000, humidity: 9800 },
//         { hour: 'Page D', temperature: 2780, humidity: 3908 },
//         { hour: 'Page E', temperature: 1890, humidity: 4800 },
//         { hour: 'Page F', temperature: 2390, humidity: 3800 },
//         { hour: 'Page G', temperature: 3490, humidity: 4300 }
//     ]
// };

// const data = [samsungLogs];

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
