import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    qrCodeURI: string;
    sharedKey: string;
    _2faverified: boolean;
}

export interface IEnableAuthenticatorViewModel {
    code: string;
    sharedKey: string;
    authenticatorUri: string;
}

export const getInitialState = (): IState => {
    return {
        errors: [],
        isFetching: false,
        qrCodeURI: '',
        sharedKey: '',
        _2faverified: false
    };
};
