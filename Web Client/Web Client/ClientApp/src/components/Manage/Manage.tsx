import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/index";
import { RouteComponentProps } from "react-router-dom";
import QRCode from "qrcode.react";
import * as actions from './logic/manageActions';
import autobind from "autobind-decorator";
import { IEnableAuthenticatorViewModel, ITwoFactorAuthenticationViewModel } from "./logic/manageState";

interface IStateToProps {
    authorized: boolean;
    errors: string[];
    isFetching: boolean;
    qrCodeURI: string;
    sharedKey: string;
    _2faData: ITwoFactorAuthenticationViewModel;
}

const dispatchProps = {
    loadQqCodeURI: actions.LoadQrCodeURI,
    verify2FA: actions.Verify2FA,
    load2FAData: actions.Load2FAData,
    disable2fa: actions.Disable2fa
};

interface IInnerState {
    code: string;
    showDisable2faSection: boolean;
}

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class Manage extends React.Component<IProps, IInnerState> {
    constructor(props: IProps) {
        super(props);

        this.state = {
            code: '',
            showDisable2faSection: false
        };
    }

    async componentDidMount() {
        await this.onMount();
    }

    @autobind
    async onMount() {
        await this.props.load2FAData();
        this.props.loadQqCodeURI();
    }

    @autobind
    async verify2FA(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const model: IEnableAuthenticatorViewModel = {
            code: this.state.code,
            sharedKey: this.props.sharedKey,
            authenticatorUri: this.props.qrCodeURI
        };

        await this.props.verify2FA(model);
        await this.onMount();
    }

    @autobind
    onCodeChanged(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ code: event.currentTarget.value });
    }

    @autobind
    showDisableSection() {
        this.setState({
            showDisable2faSection: true
        });
    }

    @autobind
    async disable2fa(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        await this.props.disable2fa();
        await this.onMount();

        this.setState({
            showDisable2faSection: false
        });
    }

    @autobind
    renderDisable2faSection() {
        return <div>
            <div className="alert alert-warning" role="alert">
                <p>
                    <span className="glyphicon glyphicon-warning-sign"></span>
                    <strong>This action only disables 2FA.</strong>
                </p>
                <p>
                    Disabling 2FA does not change the keys used in authenticator apps. If you wish to change the key
            used in an authenticator app you should <a asp-action="ResetAuthenticatorWarning">reset your
            authenticator keys.</a>
                </p>
            </div>

            <div>
                <form asp-action="Disable2fa" method="post" className="form-group" onSubmit={this.disable2fa}>
                    <button className="btn btn-danger" type="submit" >Disable 2FA</button>
                </form>
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
        </div >;
    }

    @autobind
    renderEnableAutentificatorSection() {
        return <div>
            {(this.props._2faData && this.props._2faData.is2faEnabled) &&
                <button className="btn btn-primary" type="submit" onClick={this.showDisableSection}>Disable 2FA</button>
            }
            <h4>Enable authenticator</h4>
            <div className="form-group has-error">
                <div className="col-sm-2">
                </div>
                <div className="col-sm-10">
                    {this.props.errors.map((error, index) => {
                        return <span key={index} className="help-block">{error}</span>;
                    })}
                </div>
            </div>
            <p>To use an authenticator app go through the following steps:</p>
            <ol className="list">
                <li>
                    <p>
                        Download a two-factor authenticator app like Microsoft Authenticator for
                        <a href="https://go.microsoft.com/fwlink/?Linkid=825071">Windows Phone</a>,
                        <a href="https://go.microsoft.com/fwlink/?Linkid=825072">Android</a> and
                        <a href="https://go.microsoft.com/fwlink/?Linkid=825073">iOS</a> or
                                                                                                                                                                            Google Authenticator for
                        <a href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&hl=en">Android</a> and
                        <a href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8">iOS</a>.
                    </p>
                </li>
                <li>
                    <p>Scan the QR Code or enter this key <kbd>{this.props.sharedKey}</kbd> into your two factor authenticator app. Spaces and casing do not matter.</p>
                    <div className="alert alert-info">To enable QR code generation please read our <a href="https://go.microsoft.com/fwlink/?Linkid=852423">documentation</a>.</div>
                    {this.props.qrCodeURI &&
                        <QRCode value={this.props.qrCodeURI} />
                    }

                </li>
                <li>
                    <p>
                        Once you have scanned the QR code or input the key above, your two factor authentication app will provide you
                        with a unique code. Enter the code in the confirmation box below.
</p>
                    <div className="row">
                        <div className="col-md-6">
                            <form onSubmit={this.verify2FA}>
                                <div className="form-group">
                                    <label asp-for="Code" className="control-label">Verification Code</label>
                                    <input asp-for="Code" className="form-control" autoComplete="off" value={this.state.code} onChange={this.onCodeChanged} />
                                    <span asp-validation-for="Code" className="text-danger"></span>
                                </div>
                                <button type="submit" className="btn btn-default">Verify</button>
                                <div asp-validation-summary="ModelOnly" className="text-danger"></div>
                            </form>
                        </div>
                    </div>
                </li>
            </ol>
        </div>;
    }

    public render() {
        return <div className="manage">

            {!this.state.showDisable2faSection ?
                this.renderEnableAutentificatorSection()
                :
                this.renderDisable2faSection()
            }
        </div>;
    }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
    return {
        authorized: state.signIn.authorized,
        errors: state.manage.errors,
        isFetching: state.home.isFetching,
        qrCodeURI: state.manage.qrCodeURI,
        sharedKey: state.manage.sharedKey,
        _2faData: state.manage._2faData as ITwoFactorAuthenticationViewModel
    };
};

export default connect(
    mapStateToProps, // Selects which state properties are merged into the component's props
    dispatchProps // Selects which action creators are merged into the component's props
)(Manage);
