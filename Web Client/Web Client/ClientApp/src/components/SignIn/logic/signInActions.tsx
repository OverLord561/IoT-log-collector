import * as types from './signInConstants';
import { ILoginModel } from './signInState';
import axios from 'axios';
import * as globalConstants from '../../../constants/constants';

export const Authorize = (goToPrevPage: any) => (dispatch: any, getStore: any) => {
  const URL = globalConstants.BASE_URL.concat(types.LOGIN_URL);

  const model: ILoginModel = getStore().signIn.loginModel;
  dispatch({
    type: globalConstants.IS_FETCHING,
    isFetching: true,
  });
  axios.post(URL, model)
    .then(response => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });

      dispatch({
        type: types.LOGIN,
        authorized: true,
      });
      //data.userName = data.email;

      goToPrevPage();

    }).catch(error => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });
      if (error.response.status === 409) {
        dispatch({
          type: globalConstants.ADD_VALIDATION_ERROR,
          errors: error.response.data,
        });
      }
    });
};

export const LogOut = () => {

  const URL = globalConstants.BASE_URL.concat(types.LOGOUT_URL);

  return (dispatch: any, getStore: any) => {

    return axios.post(URL)
      .then(response => {
        if (response.status === 200) {
          window.location.href = '/';
        }
      }).catch(error => {
        console.log(error);
      });
  };
};

export const SetFormData = (property: string, value: string | boolean) => (dispatch: any, getStore: any) => {
  const model: ILoginModel = { ...getStore().signIn.loginModel };
  model[property] = value;

  dispatch({
    type: types.SET_LOGIN_DATA,
    loginModel: model,
  });
};
