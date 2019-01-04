import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    qrCodeURI: string;
    sharedKey: string;
    _2faData?: ITwoFactorAuthenticationViewModel;
}

export interface IEnableAuthenticatorViewModel {
    code: string;
    sharedKey: string;
    authenticatorUri: string;
}

export interface ITwoFactorAuthenticationViewModel {
    hasAuthenticator: boolean;
    is2faEnabled: boolean;
    recoveryCodesLeft: number;
}

export const getInitialState = (): IState => {
    return {
        errors: [],
        isFetching: false,
        qrCodeURI: '',
        sharedKey: '',
    };
};
