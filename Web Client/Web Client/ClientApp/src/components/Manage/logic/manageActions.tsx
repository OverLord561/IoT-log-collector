import axios from 'axios';
import * as types from './manageConstants';
import * as globalTypes from '../../../constants/constants';
import { IEnableAuthenticatorViewModel } from './manageState';

export const LoadQrCodeURI = () => (dispatch: any, getStore: any) => {
    const URL = globalTypes.BASE_URL.concat(types.QRCODE_URL);

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
                type: types.LOAD_QRCODE_URI,
                qrCodeURI: response.data.qrCodeURI,
                sharedKey: response.data.sharedKey,
            });

        }).catch(error => {

            console.log(error);
            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
        });
};

export const Verify2FA = (model: IEnableAuthenticatorViewModel) => (dispatch: any, getStore: any) => {
    const URL = globalTypes.BASE_URL.concat(types.VERIFY_2FA_URL);

    dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: true,
    });

    axios.post(URL, model)
        .then(response => {

            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            dispatch({
                type: types.VERIFY_2FA,
                _2faverified: response.data._2faverified,
            });

        }).catch(error => {

            console.log(error);
            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
        });
};
