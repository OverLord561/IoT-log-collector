import axios from 'axios';
import * as types from './homeConstants';
import * as globalTypes from '../../../constants/constants';

export const LoadLogData = () => (dispatch: any, getStore: any) => {
    const URL = types.LOG_DATA_URL;

    dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: true,
    });

    axios.get(URL)
        .then(response => {
            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            dispatch({
                type: types.LOAD_DEVICE_LOGS,
                deviceLogs: response.data
            });

        }).catch(error => {
            console.log(error);
        });

};
