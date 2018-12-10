import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/index";
import { RouteComponentProps } from "react-router-dom";
import * as actions from "./logic/homeActions";
import { IDeviceLogsUIFormat, ILog } from "./logic/homeState";
import * as moment from "moment";

import {
  LineChart,
  Line,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Legend
} from "recharts";
import autobind from "autobind-decorator";
import { GenerateRandomHex } from "../../features/commonFeature";

interface IStateToProps {
  authorized: boolean;
  isFetching: boolean;
  devicesLogs: IDeviceLogsUIFormat[];
}

const dispatchProps = {
  loadLogData: actions.LoadLogData
};

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class Home extends React.Component<IProps, any> {
  constructor(props: IProps) {
    super(props);
  }

  componentDidMount() {
    const Utc = moment().format("X");

    this.props.loadLogData(Utc, true);
  }

  componentDidUpdate(prevProps: IProps, prevState: any) {

    if (JSON.stringify(this.props.devicesLogs) !== JSON.stringify(prevProps.devicesLogs)) {
      const Utc = moment().format("X");
      this.props.loadLogData(Utc, false);
    }
  }

  @autobind
  renderLines(log: ILog) {
    const lines: Line[] = [];

    Object.keys(log).forEach((element, iterator) => {
      if (iterator > 0) {
        const color: string = GenerateRandomHex();
        lines.push(
          <Line
            key={iterator}
            type="monotone"
            dataKey={element}
            stroke={color}
            activeDot={{ r: 8 }}
          />
        );
      }
    });

    return lines;
  }

  @autobind
  renderCharts() {
    return this.props.devicesLogs.map((deviceLogs, index) => {
      return this.renderChart(deviceLogs, index);
    });
  }

  @autobind
  renderChart(deviceLogs: IDeviceLogsUIFormat, index?: number) {
    let XAxisName: string = "";
    const logs: ILog[] = deviceLogs.logs;
    let lines: Line[] = [];

    if (logs.length) {
      const firstLog = logs[0];
      XAxisName = Object.keys(firstLog)[0];

      lines = this.renderLines(logs[0]);
    }

    return (
      <div key={index}>
        <h1>{deviceLogs.deviceName}</h1>
        <LineChart
          width={600}
          height={300}
          data={logs}
          margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
        >
          <XAxis dataKey={XAxisName} />
          <YAxis />
          <CartesianGrid strokeDasharray="3 3" />
          <Tooltip />
          <Legend />
          {lines}
        </LineChart>
      </div>
    );
  }

  public render() {
    return <div className="home">{this.renderCharts()}</div>;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
    isFetching: state.home.isFetching,
    devicesLogs: state.home.devicesLogs,
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  dispatchProps // Selects which action creators are merged into the component's props
)(Home);
