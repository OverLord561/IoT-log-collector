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

            dispatch({
                type: globalTypes.ADD_VALIDATION_ERROR,
                errors: [],
            });

        }).catch(error => {

            console.log(error);

            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            if (error.response.status === 400) {
                dispatch({
                    type: globalTypes.ADD_VALIDATION_ERROR,
                    errors: error.response.data.errors,
                });
            }
        });
};

export const Verify2FA = (model: IEnableAuthenticatorViewModel) => async (dispatch: any, getStore: any): Promise<boolean> => {
    const URL = globalTypes.BASE_URL.concat(types.VERIFY_2FA_URL);

    dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: true,
    });

    return await axios.post(URL, model)
        .then(response => {

            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            dispatch({
                type: globalTypes.ADD_VALIDATION_ERROR,
                errors: [],
            });

            return true;

        }).catch(error => {

            console.log(error);
            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            if (error.response.status === 400) {
                dispatch({
                    type: globalTypes.ADD_VALIDATION_ERROR,
                    errors: error.response.data.errors,
                });
            }

            return false;
        });
};

export const Load2FAData = () => async (dispatch: any, _getStore: any): Promise<boolean> => {
    const URL = globalTypes.BASE_URL.concat(types.LOAD_2FA_DATA_URL);

    dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: true,
    });

    return await axios.get(URL)
        .then(response => {

            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            dispatch({
                type: types.LOAD_2FA_DATA,
                _2faData: response.data._2faData,
            });
            dispatch({
                type: globalTypes.ADD_VALIDATION_ERROR,
                errors: [],
            });

            return true;
        }).catch(error => {

            console.log(error);
            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            if (error.response.status === 400) {
                dispatch({
                    type: globalTypes.ADD_VALIDATION_ERROR,
                    errors: error.response.data.errors,
                });
            }

            return false;
        });
};

export const Disable2fa = () => async (dispatch: any, _getStore: any): Promise<boolean> => {
    const URL = globalTypes.BASE_URL.concat(types.DISABLE_2FA_URL);

    dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: true,
    });

    return await axios.post(URL)
        .then(response => {

            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });

            dispatch({
                type: globalTypes.ADD_VALIDATION_ERROR,
                errors: [],
            });

            return true;
        }).catch(error => {

            console.log(error);
            dispatch({
                type: globalTypes.IS_FETCHING,
                isFetching: false,
            });
            if (error.response.status === 400) {
                dispatch({
                    type: globalTypes.ADD_VALIDATION_ERROR,
                    errors: error.response.data.errors,
                });
            }

            return false;
        });
};
