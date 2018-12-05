import React from 'react';
import { connect } from 'react-redux';
import { IApplicationState } from '../../store/index';
import { RouteComponentProps } from 'react-router-dom';
import * as actions from './logic/homeActions';
import { IDeviceLogsUIFormat, ILog } from './logic/homeState';

import {
  LineChart, Line, CartesianGrid,
  XAxis, YAxis, Tooltip, Legend
} from 'recharts';

interface IStateToProps {
  authorized: boolean;
  isFetching: boolean;
  devicesLogs: IDeviceLogsUIFormat[];
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
    this.props.loadLogData(1544006919);
  }

  public render() {
    const samsungLogs: IDeviceLogsUIFormat = this.props.devicesLogs[0];
    let data: ILog[] = [];
    let title: string = '';

    if (samsungLogs) {
      title = samsungLogs.deviceName;

      data = samsungLogs.logs.map(log => {
        return log;
      });
    }

    return <div className="home">
      <h1>{title}</h1>
      <LineChart width={600} height={300} data={data}
            margin={{top: 5, right: 30, left: 20, bottom: 5}}>
       <XAxis dataKey="hour"/>
       <YAxis/>
       <CartesianGrid strokeDasharray="3 3"/>
       <Tooltip/>
       <Legend />
       <Line type="monotone" dataKey="temperature" stroke="#8884d8" activeDot={{r: 8}}/>
       <Line type="monotone" dataKey="humidity" stroke="#82ca9d" />
      </LineChart>
    </div>;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
    isFetching: state.home.isFetching,
    devicesLogs: state.home.devicesLogs
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  dispatchProps // Selects which action creators are merged into the component's props
)(Home);
