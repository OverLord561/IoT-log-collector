import * as SignUp from '../components/SignUp/logic/signUpState';
import * as SignIn from '../components/SignIn/logic/signInState';

export interface IApplicationState {
    signUp: SignUp.IState;
    signIn: SignIn.IState;
}
