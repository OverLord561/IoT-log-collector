import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import autobind from 'autobind-decorator';

import { IState, ILoginModel, IAppUser, ILoginWith2faViewModel, ILoginProvider } from './logic/signInState';
import * as actions from './logic/signInActions';
import { connect } from 'react-redux';
import { IApplicationState } from 'src/store';

interface IStateProps {
    loginModel: ILoginModel;
    authorized: boolean;
    errors: string[];
    isFetching: boolean;
    appUser: IAppUser;
    loginProviders: ILoginProvider[];
}

const dispatchProps = {
    login: actions.Login,
    setFormData: actions.SetFormData,
    authorizeByCookie: actions.AuthorizeByCookie,
    loginWith2fa: actions.LoginWith2fa,
    getLoginProviders: actions.GetLoginProviders,
    externalLogin: actions.ExternalLogin
};

interface IInnerState {
    code: string;
}

type IProps = IState & IStateProps & RouteComponentProps<{}> & typeof dispatchProps;

class SignIn extends React.Component<IProps, IInnerState> {

    constructor(props: IProps) {
        super(props);

        this.state = {
            code: ''
        };
    }

    componentDidMount() {
        this.props.getLoginProviders();
    }

    @autobind
    setLoginData(event: React.FormEvent<HTMLInputElement>) {
        const key: string = 'prop';
        const prop: string = event.currentTarget.dataset[key] as string;
        const value: string = event.currentTarget.value;

        if (prop === 'rememberMe') {
            this.props.setFormData(prop, !this.props.loginModel.rememberMe);
        } else {
            return this.props.setFormData(prop, value);
        }
    }

    @autobind
    logIn(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        this.props.login(() => {
            this.props.history.push('/');
        });
    }

    @autobind
    logInWith2FA(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const model: ILoginWith2faViewModel = {
            twoFactorCode: this.state.code,
            rememberMe: false,
            rememberMachine: false
        };

        this.props.loginWith2fa(model);
    }

    @autobind
    onCodeChanged(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ code: event.currentTarget.value });
    }

    @autobind
    useExternalLoginProvider(provider: string, event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        this.props.externalLogin(provider);
    }

    @autobind
    render2FASection() {
        return <div>
            <h2>Two-factor authentication</h2>
            <hr />
            <p>Your login is protected with an authenticator app. Enter your authenticator code below.</p>
            <div className="row">
                <div className="col-md-4">
                    <form onSubmit={this.logInWith2FA}>
                        <input asp-for="RememberMe" type="hidden" />
                        <div asp-validation-summary="All" className="text-danger"></div>
                        <div className="form-group">
                            <label asp-for="TwoFactorCode"></label>
                            <input asp-for="TwoFactorCode" className="form-control" autoComplete="off" onChange={this.onCodeChanged} />
                            <span asp-validation-for="TwoFactorCode" className="text-danger"></span>
                        </div>
                        <div className="form-group">
                            <button type="submit" className="btn btn-default">Log in</button>
                        </div>
                    </form>
                    <div className="form-group has-error">
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-10">
                            {this.props.errors.map((error, index) => {
                                return <span key={index} className="help-block">{error}</span>;
                            })}
                        </div>
                    </div>
                </div>
            </div>
        </div>;
    }

    @autobind
    renderLoginProviders() {
        return <div className="col-md-6">
            <section>
                <h4>Use another service to log in.</h4>
                {!this.props.loginProviders ?

                    <div>
                        <p> There are no external authentication services configured
                            for details on setting up this ASP.NET application to support logging in via external services.
                            </p>
                    </div>
                    :

                    <div>
                        {this.props.loginProviders.map((provider, index) => {
                            return <form key={index} className="form-horizontal" onSubmit={(e) => this.useExternalLoginProvider(provider.name, e)}>
                                <button type="submit"
                                    className="btn btn-default"
                                    name="provider"
                                    value={provider.name}
                                    title={`Log in using your ${provider.DisplayName} account`}
                                >
                                    {provider.name}
                                </button>
                            </form>;
                        })}
                    </div>
                }
            </section>
        </div>;
    }

    @autobind
    renderSimpleLoginSection() {
        return <div>
            <div className="col-md-6">
                <form className="form-horizontal" onSubmit={this.logIn}>
                    <div className="form-group">
                        <label className="control-label col-sm-2" htmlFor="email">Email:</label>
                        <div className="col-sm-10">
                            <input
                                data-prop="email"
                                value={this.props.loginModel.email} type="email" className="form-control" id="email" placeholder="Enter email" onChange={this.setLoginData} />
                        </div>
                    </div>
                    <div className="form-group">
                        <label className="control-label col-sm-2" htmlFor="pwd">Password:</label>
                        <div className="col-sm-10">
                            <input
                                data-prop="password"
                                value={this.props.loginModel.password} type="password" className="form-control" id="pwd" placeholder="Enter password" onChange={this.setLoginData} />
                        </div>
                    </div>
                    <div className="form-group">
                        <div className="col-sm-offset-2 col-sm-10">
                            <div className="checkbox">
                                <label><input
                                    data-prop="rememberMe"
                                    type="checkbox" onChange={this.setLoginData} checked={this.props.loginModel.rememberMe} /> Remember me</label>
                            </div>
                        </div>
                    </div>
                    <div className="form-group">
                        <div className="col-sm-offset-2 col-sm-10">
                            <button type="submit" className="btn btn-default">Submit</button>
                        </div>
                    </div>

                    <div className="form-group has-error">
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-10">
                            {this.props.errors.map((error, index) => {
                                return <span key={index} className="help-block">{error}</span>;
                            })}
                        </div>
                    </div>
                </form>
            </div>
            {this.renderLoginProviders()}
        </div>;
    }

    public render() {
        return <div className="sign-in">
            <div className="row">
                {(!this.props.appUser || !this.props.appUser.twoFactorEnabled) ?
                    this.renderSimpleLoginSection()
                    :
                    this.render2FASection()
                }
            </div>
        </div>;
    }
}

function mapStateToProps(state: IApplicationState): IStateProps {
    return {
        loginModel: state.signIn.loginModel,
        authorized: state.signIn.authorized,
        errors: state.signIn.errors,
        isFetching: state.signIn.isFetching,
        appUser: state.signIn.appUser as IAppUser,
        loginProviders: state.signIn.loginProviders,
    };
}

export default connect(
    mapStateToProps,
    dispatchProps
)(SignIn);
