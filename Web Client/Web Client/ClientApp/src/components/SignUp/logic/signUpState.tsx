import { IModel } from '../../../common/Identity';

export interface IState extends IModel {
    registerModel: IRegisterModel;
}

export interface IRegisterModel {
    email: string;
    password: string;
    confirmPassword: string;
    [key: string]: string;
}

export const getInitialState = (): IState => {
    return {
      registerModel: {
        email: "yurapuk452@gmail.com",
        password: "123Qaz-",
        confirmPassword: "123Qaz-",
      },
      errors: [],
      isFetching: false
    };
};
