import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
  loginModel: ILoginModel;
  authorized: boolean;
}

export interface ILoginModel {
  email: string;
  password: string;
  rememberMe: boolean;
  [key: string]: string | boolean;
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
