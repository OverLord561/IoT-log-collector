import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/index";
import { RouteComponentProps } from "react-router-dom";
import QRCode from "qrcode.react";
import * as actions from './logic/manageActions';
import autobind from "autobind-decorator";
import { IEnableAuthenticatorViewModel } from "./logic/manageState";

interface IStateToProps {
    authorized: boolean;
    isFetching: boolean;
    qrCodeURI: string;
    sharedKey: string;
    _2faverified: boolean;
}

const dispatchProps = {
    loadQqCodeURI: actions.LoadQrCodeURI,
    verify2FA: actions.Verify2FA
};

interface IInnerState {
    code: string;
}

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class Manage extends React.Component<IProps, IInnerState> {
    constructor(props: IProps) {
        super(props);

        this.state = {
            code: ''
        };
    }

    componentDidMount() {
        this.props.loadQqCodeURI();
    }

    @autobind
    onSubmitForm(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const model: IEnableAuthenticatorViewModel = {
            code: this.state.code,
            sharedKey: this.props.sharedKey,
            authenticatorUri: this.props.qrCodeURI
        };

        this.props.verify2FA(model);
    }

    @autobind
    onCodeChanged(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ code: event.currentTarget.value });
    }

    public render() {
        const _state = this.state;
        return <div className="manage">
            <h3>Two Factor enabled: {this.props._2faverified.toString()}</h3>
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
                            <form onSubmit={this.onSubmitForm}>
                                <div className="form-group">
                                    <label asp-for="Code" className="control-label">Verification Code</label>
                                    <input asp-for="Code" className="form-control" autoComplete="off" value={_state.code} onChange={this.onCodeChanged} />
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
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
    return {
        authorized: state.signIn.authorized,
        isFetching: state.home.isFetching,
        qrCodeURI: state.manage.qrCodeURI,
        sharedKey: state.manage.sharedKey,
        _2faverified: state.manage._2faverified
    };
};

export default connect(
    mapStateToProps, // Selects which state properties are merged into the component's props
    dispatchProps // Selects which action creators are merged into the component's props
)(Manage);
