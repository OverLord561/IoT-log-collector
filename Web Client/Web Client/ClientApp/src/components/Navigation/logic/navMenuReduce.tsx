import * as types from './navMenuConstants';

export const signUpReducer = (state, action) => {

    switch (action.type) {

        case types.LOG_OUT: {
            return {
                ...state
            };
        }
        default: { return state; }
    }
};
