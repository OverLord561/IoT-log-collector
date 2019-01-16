import { getInitialState } from './signInState';
import * as types from './signInConstants';
import * as globalConstants from '../../../constants/constants';

const initialState = getInitialState();

export const signInReducer = (state = initialState, action) => {

    switch (action.type) {
        case globalConstants.ADD_VALIDATION_ERROR: {
            return {
                ...state,
                errors: action.errors,
            };
        }
        case types.SET_LOGIN_DATA: {
            return {
                ...state,
                loginModel: action.loginModel,
            };
        }
        case globalConstants.IS_FETCHING: {
            return {
                ...state,
                isFetching: action.isFetching,
            };
        }
        case types.LOGIN: {
            return {
                ...state,
                appUser: action.appUser,
                errors: action.errors
            };
        }

        case types.AUTHORIZED: {
            return {
                ...state,
                authorized: action.authorized,
            };
        }

        case types.EXTERNAL_LOGIN_PROVIDERS: {
            return {
                ...state,
                loginProviders: action.loginProviders,
            };
        }
        default: { return state; }
    }
};
