import axios from 'axios';

import * as globalTypes from '../../../constants/constants';
import * as types from './navMenuConstants';
import * as signInTypes from '../../SignIn/logic/signInConstants';

import * as globalConstants from '../../../constants/constants';

export const LogOut = (goToPrevPage: any) => (dispatch: any, getStore: any) => {

    const URL = globalTypes.BASE_URL.concat(types.LOG_OUT_URL);

    dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: true,
    });

    axios.post(URL)
        .then(response => {
            dispatch({
                type: globalConstants.IS_FETCHING,
                isFetching: false,
            });

            dispatch({
                type: signInTypes.LOGIN,
                authorized: false,
              });

            // goToPrevPage();
        }).catch(error => {
            console.log(error);
        });
};
