import * as SignUp from '../components/SignUp/logic/signUpState';
import * as SignIn from '../components/SignIn/logic/signInState';
import * as Home from '../components/Home/logic/homeState';
import * as Manage from '../components/Manage/logic/manageState';

export interface IApplicationState {
    signUp: SignUp.IState;
    signIn: SignIn.IState;
    home: Home.IState;
    manage: Manage.IState;
}
