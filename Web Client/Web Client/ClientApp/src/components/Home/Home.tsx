import React from 'react';
import { connect } from 'react-redux';
import { IApplicationState } from '../../store/index';
import { RouteComponentProps } from 'react-router-dom';
import * as actions from './logic/homeActions';

interface IStateToProps {
  authorized: boolean;
  isFetching: boolean;
}

const dispatchProps = {
  loadLogData: actions.LoadLogData,
};

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class Home extends React.Component<IProps, any> {
  constructor(props: IProps) {
    super(props);
  }

  componentDidMount() {
    this.props.loadLogData();
  }

  public render() {
    return <div className="home">
      <h1>Hello, IoT log collector!!</h1>
    </div>;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
    isFetching: state.home.isFetching,
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  dispatchProps // Selects which action creators are merged into the component's props
)(Home);
