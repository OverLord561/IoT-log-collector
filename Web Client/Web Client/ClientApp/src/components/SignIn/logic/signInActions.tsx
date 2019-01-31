import * as types from './signInConstants';
import { ILoginModel, ILoginWith2faViewModel } from './signInState';
import axios from 'axios';
import * as globalConstants from '../../../constants/constants';
import { IsAuthorized } from '../../../features/commonFeature';

export const Login = () => async (dispatch: any, getStore: any) => {
  const URL = globalConstants.BASE_URL.concat(types.LOGIN_URL);

  const model: ILoginModel = getStore().signIn.loginModel;
  dispatch({
    type: globalConstants.IS_FETCHING,
    isFetching: true,
  });
  await axios.post(URL, model)
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

export const GetLoginProviders = () => (dispatch: any, getStore: any) => {
  const URL = globalConstants.BASE_URL.concat(types.EXTERNAL_LOGIN_PROVIDERS_URL);

  dispatch({
    type: globalConstants.IS_FETCHING,
    isFetching: true,
  });
  axios.get(URL)
    .then(response => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });

      dispatch({
        type: types.EXTERNAL_LOGIN_PROVIDERS,
        loginProviders: response.data.loginProviders,
      });

    }).catch(error => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });

      console.log(error);
    });
};

export const ExternalLogin = (provider: string) => (dispatch: any, getStore: any) => {
  const URL = globalConstants.BASE_URL.concat(types.EXTERNAL_LOGIN_URL);

  dispatch({
    type: globalConstants.IS_FETCHING,
    isFetching: true,
  });
  axios.post(URL, { provider })
    .then(response => {

      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });

    }).catch(error => {
      dispatch({
        type: globalConstants.IS_FETCHING,
        isFetching: false,
      });

      //window.location.href = 'https://accounts.google.com/signin/oauth/identifier?client_id=353095679689-evv6orl38sktkbgjtm0ojj60p23719br.apps.googleusercontent.com&as=qRaK0WJv5IhranviLtBtew&destination=http%3A%2F%2Flocalhost%3A60366&approval_state=!ChRGNV9QaklwTHk2VjhhRHFSNTNPWBIfWV9BZk96VUhFbjBmY0t0V0xtY192ZG9JRmFjdmhCWQ%E2%88%99APNbktkAAAAAXDtsv3-ddlVorZO9x94Kp81MKMmys3wG&oauthgdpr=1&xsrfsig=ChkAeAh8T3V26MXVRfeM40_Oau0_23ye__BmEg5hcHByb3ZhbF9zdGF0ZRILZGVzdGluYXRpb24SBXNvYWN1Eg9vYXV0aHJpc2t5c2NvcGU&flowName=GeneralOAuthFlow';
      console.log(error);
    });
};
