import * as types from './signInConstants';
import { ILoginModel, ILoginWith2faViewModel } from './signInState';
import axios from 'axios';
import * as globalConstants from '../../../constants/constants';
import { IsAuthorized } from '../../../features/commonFeature';

export const Login = (goToHome: any) => (dispatch: any, getStore: any) => {
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
        appUser: response.data.user,
        errors: [],
      });

      if (!response.data.user.twoFactorEnabled) {
        dispatch({
          type: types.AUTHORIZED,
          authorized: true,
        });

        goToHome();

      }
    }).catch(error => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });
      if (error.response.status === 400) {
        dispatch({
          type: globalConstants.ADD_VALIDATION_ERROR,
          errors: error.response.data.errors,
        });
      }
    });
};

export const AuthorizeByCookie = () => (dispatch: any, getStore: any) => {

  if (IsAuthorized()) {
    dispatch({
      type: types.AUTHORIZED,
      authorized: true,
    });
  }
};

export const SetFormData = (property: string, value: string | boolean) => (dispatch: any, getStore: any) => {
  const model: ILoginModel = { ...getStore().signIn.loginModel };
  model[property] = value;

  dispatch({
    type: types.SET_LOGIN_DATA,
    loginModel: model,
  });
};

export const LoginWith2fa = (model: ILoginWith2faViewModel) => (dispatch: any, getStore: any) => {
  const URL = globalConstants.BASE_URL.concat(types.LOGIN_WITH_2FA_URL);

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
        appUser: response.data.user,
        errors: [],
      });

      dispatch({
        type: types.AUTHORIZED,
        authorized: true,
      });

    }).catch(error => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });
      if (error.response.status === 400) {
        dispatch({
          type: globalConstants.ADD_VALIDATION_ERROR,
          errors: error.response.data.errors,
        });
      }
    });
};
