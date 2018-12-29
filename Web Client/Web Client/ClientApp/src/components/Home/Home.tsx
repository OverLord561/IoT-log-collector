import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/index";
import { RouteComponentProps } from "react-router-dom";
import * as actions from "./logic/homeActions";
import { IDeviceLogsInChartFormat } from './logic/homeState';
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
  chartData: IDeviceLogsInChartFormat;
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
  renderLines() {

    const lines: Line[] = [];

    this.props.chartData.axesNames.forEach((ax, iterator) => {
      if (iterator > 0) {
        const color: string = GenerateRandomHex();
        lines.push(
          <Line
            key={iterator}
            type="monotone"
            dataKey={ax.toLowerCase()}
            stroke={color}
            activeDot={{ r: 8 }}
          />
        );
      }
    });

    return lines;
  }

  @autobind
  prepareDataForChartLibrary() {
    const logs: any = [];
    const axesNames: string[] = this.props.chartData.axesNames;

    this.props.chartData.logs.forEach((log, iterator) => {
      const obj = {};

      axesNames.forEach((ax, it) => {
        obj[ax.toLowerCase()] = log.values[it];
      });

      logs.push(
        obj
      );
    });

    return logs;
  }

  @autobind
  renderChart() {

    const chartData: IDeviceLogsInChartFormat = this.props.chartData;
    const data = this.prepareDataForChartLibrary();

    return (
      <div>
        <h1>{chartData.chartName}</h1>
        <LineChart
          width={600}
          height={300}
          data={data}
          margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
        >
          <XAxis dataKey={chartData.axesNames[0].toLowerCase()} />
          <YAxis />
          <CartesianGrid strokeDasharray="3 3" />
          <Tooltip />
          <Legend />
            {this.renderLines()}
        </LineChart>
      </div>
    );
  }

  public render() {
    if (this.props.chartData) {
      return <div className="home">{this.renderChart()}</div>;
    }

    return null;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
    isFetching: state.home.isFetching,
    chartData: state.home.chartData as IDeviceLogsInChartFormat,
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  dispatchProps // Selects which action creators are merged into the component's props
)(Home);
