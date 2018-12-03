import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    registerModel: IRegisterModel;
}

export interface IRegisterModel {
    password: string;
    passwordConfirm: string;
    [key: string]: string;
}

export const getInitialState = (): IState => {
    return {
      registerModel: {
        email: "yurapuk452@gmail.com",
        password: "",
        passwordConfirm: "123Qaz-",
      },
      errors: [],
      isFetching: false
    };
};
