import axios from 'axios';

import * as types from './signUpConstants';
import * as globalTypes from '../../../constants/constants';

import { IRegisterModel } from './signUpState';

export const Register = (goToPrevPage: any) => (dispatch: any, getStore) => {

  const URL = globalTypes.BASE_URL.concat(types.REGISTER_URL);

  const data: IRegisterModel = getStore().signUp.registerModel;

  dispatch({
    type: globalTypes.IS_FETCHING,
    isFetching: true,
  });

  axios.post(URL, data)
    .then(response => {
      dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: false,
      });
      dispatch({
        type: types.REGISTER,
        errors: []
      });

      goToPrevPage();
    }).catch(error => {
      dispatch({
        type: globalTypes.IS_FETCHING,
        isFetching: false,
      });

      if (error.response.data.statusCode === 409) {
        dispatch({
          type: globalTypes.ADD_VALIDATION_ERROR,
          errors: error.response.data.errors,
        });
      }
    });
};

export const SetFormData = (property: string, value: string) => (dispatch: any, getStore: any) => {
  const model: IRegisterModel = { ...getStore().signUp.registerModel };
  model[property] = value;
  dispatch({
    type: types.SET_REGISTER_DATA,
    registerModel: model,
  });
};
