import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
  loginModel: ILoginModel;
  authorized: boolean;
  appUser?: IAppUser;
}

export interface ILoginModel {
  email: string;
  password: string;
  rememberMe: boolean;
  [key: string]: string | boolean;
}

export interface IAppUser {
  email: string;
  userName: string;
  twoFactorEnabled: boolean;
}

export interface ILoginWith2faViewModel {
  twoFactorCode: string;
  rememberMe: boolean;
  rememberMachine: boolean;
}

export function getInitialState(): IState {

  return {
    loginModel: {
      email: "yurapuk452@gmail.com",
      password: "123Qaz-",
      rememberMe: false,
    },
    authorized: false,
    errors: [],
    isFetching: false,
  };
}
