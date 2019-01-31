import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import autobind from 'autobind-decorator';

import { IRegisterModel } from './logic/signUpState';
import * as actions from './logic/signUpActions';
import { IApplicationState } from '../../store/index';
import { IModel } from '../../common/Identity';

interface IStateToProps extends IModel {
    registerModel: IRegisterModel;
    authorized: boolean;
}

const dispatchProps = {
    register: actions.Register,
    setFormData: actions.SetFormData,
};

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class SignUp extends React.Component<IProps, any> {
    constructor(props: IProps) {
        super(props);
    }

    @autobind
    async submitForm(event: any) {
        event.preventDefault();

        const succeeded = await this.props.register();

        if (succeeded) {
            this.props.history.goBack();
        }
    }

    @autobind
    setRegisterData(event: React.FormEvent<HTMLInputElement>) {
        const key: string = 'prop';
        const prop: string = event.currentTarget.dataset[key] as string;
        const value: string = event.currentTarget.value;
        this.props.setFormData(prop, value);
    }

    public render() {
        return <div className="sign-up">
            <form className="form-horizontal" onSubmit={this.submitForm}>
                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="email">Email:</label>
                    <div className="col-sm-10">
                        <input
                            data-prop="email"
                            value={this.props.registerModel.email}
                            type="email"
                            className="form-control"
                            id="email"
                            placeholder="Enter email"
                            onChange={this.setRegisterData}
                            autoComplete="off"
                        />
                    </div>
                </div>
                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="pwd">Password:</label>
                    <div className="col-sm-10">
                        <input
                            data-prop="password"
                            value={this.props.registerModel.password}
                            type="password"
                            className="form-control"
                            id="pwd"
                            placeholder="Enter password"
                            onChange={this.setRegisterData}
                            autoComplete="off"
                        />
                    </div>
                </div>

                <div className="form-group">
                    <label className="control-label col-sm-2" htmlFor="conf">Confirm password:</label>
                    <div className="col-sm-10">
                        <input
                            data-prop="passwordConfirm"
                            value={this.props.registerModel.passwordConfirm}
                            type="password"
                            className="form-control"
                            id="conf"
                            placeholder="Confirm password"
                            autoComplete="off"
                            onChange={this.setRegisterData}
                        />
                    </div>
                </div>

                <div className="form-group">
                    <div className="col-sm-offset-2 col-sm-10">
                        <button disabled={this.props.isFetching} type="submit" className="btn btn-default">Submit</button>
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
        </div>;
    }

}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
    return {
        registerModel: state.signUp.registerModel,
        errors: state.signUp.errors,
        isFetching: state.signUp.isFetching,
        authorized: state.signIn.authorized
    };
};

export default connect(
    mapStateToProps, // Selects which state properties are merged into the component's props
    dispatchProps // Selects which action creators are merged into the component's props
)(SignUp);
