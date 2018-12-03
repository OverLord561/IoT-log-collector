import { getInitialState } from './signUpState';
import * as types from './signUpConstants';
import * as globalConstants from '../../../constants/constants';

const initialState = getInitialState();

export const signUpReducer = (state = initialState, action) => {

    switch (action.type) {
        case globalConstants.ADD_VALIDATION_ERROR: {
            return {
              ...state,
              errors: action.errors,
            };
          }
        case types.SET_REGISTER_DATA: {
            return {
                ...state,
                registerModel: action.registerModel,
            };
        }
        case globalConstants.IS_FETCHING: {
            return {
                ...state,
                isFetching: action.isFetching,
            };
        }
        case types.REGISTER: {
            return {
                ...state,
                errors: action.errors,
            };
        }
        default: { return state; }
    }
};
