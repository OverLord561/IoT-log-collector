import { getInitialState } from './homeState';
import * as types from './homeConstants';
import * as globalConstants from '../../../constants/constants';

const initialState = getInitialState();

export const homeReducer = (state = initialState, action) => {

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
        case types.LOAD_DEVICE_LOGS: {
            return {
                ...state,
                chartData: action.chartData,
                isInitial: action.isInitial
            };
        }

        case types.SERVER_SETTINGS: {
            return {
                ...state,
                serverSettings: action.serverSettings,
            };
        }

        case types.DATASTORAGE_PLUGINS: {
            return {
                ...state,
                dataStoragePlugins: action.dataStoragePlugins,
            };
        }

        default: { return state; }
    }
};
