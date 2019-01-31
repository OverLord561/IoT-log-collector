import axios from 'axios';

import * as globalTypes from '../../../constants/constants';
import * as types from './navMenuConstants';
import * as signInTypes from '../../SignIn/logic/signInConstants';

import * as globalConstants from '../../../constants/constants';

export const LogOut = () => async (dispatch: any, getStore: any) => {

    const URL = globalTypes.BASE_URL.concat(types.LOG_OUT_URL);

    dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: true,
    });

    await axios.post(URL)
        .then(response => {
            dispatch({
                type: globalConstants.IS_FETCHING,
                isFetching: false,
            });

            dispatch({
                type: signInTypes.AUTHORIZED,
                authorized: false,
            });

        }).catch(error => {
            console.log(error);
        });
};
