import { getInitialState } from './manageState';
import * as types from './manageConstants';
import * as globalConstants from '../../../constants/constants';

const initialState = getInitialState();

export const manageReducer = (state = initialState, action) => {

    switch (action.type) {
        case globalConstants.ADD_VALIDATION_ERROR: {
            return {
                ...state,
                errors: action.errors,
            };
        }
        case globalConstants.IS_FETCHING: {
            return {
                ...state,
                isFetching: action.isFetching,
            };
        }
        case types.LOAD_QRCODE_URI: {
            return {
                ...state,
                qrCodeURI: action.qrCodeURI,
                sharedKey: action.sharedKey
            };
        }
        case types.VERIFY_2FA: {
            return {
                ...state,
                _2faverified: action._2faverified,
            };
        }

        default: { return state; }
    }
};
