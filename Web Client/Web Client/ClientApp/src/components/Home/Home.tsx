import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/index";
import { RouteComponentProps } from "react-router-dom";
import * as actions from "./logic/homeActions";
import { IDeviceLogsInChartFormat, IServerSettingViewModel, IDataStoragePlugin } from './logic/homeState';
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
  serverSettings: IServerSettingViewModel[];
  dataStoragePlugins: string[];
}

const dispatchProps = {
  loadLogData: actions.LoadLogData,
  getServerSettings: actions.GetServerSettings,
  updateServerSettings: actions.UpdateServerSettings,
  updateDataStoragePlugin: actions.UpdateDataStoragePlugin,
  setServerSettings: actions.SetServerSettings,
  setDataStoragePlugins: actions.SetDataStoragePlugins,
  getDataStoragePlugins: actions.GetDataStoragePlugins
};

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class Home extends React.Component<IProps, any> {
  constructor(props: IProps) {
    super(props);
  }

  componentDidMount() {
    const Utc = moment().format("X");

    this.props.getServerSettings();
    this.props.getDataStoragePlugins();
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
      <div className="row home">
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

  @autobind
  updateServerSettings(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    this.props.updateServerSettings(this.props.serverSettings);
  }

  @autobind
  setServerSetting(setting: IServerSettingViewModel, index: number, event: React.FormEvent<HTMLInputElement>) {
    const copy = this.props.serverSettings.slice();

    copy[index].value = event.currentTarget.value;

    this.props.setServerSettings(copy);
  }

  @autobind
  updateDataStoragePlugin(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const selectedPlugin = this.props.dataStoragePlugins.find(plugin => plugin.isSelected);

    this.props.updateDataStoragePlugin(selectedPlugin);
  }

  @autobind
  setDataStoragePlugins(plugin: IDataStoragePlugin, index: number, event: React.FormEvent<HTMLInputElement>) {
    const copy = this.props.dataStoragePlugins.slice();

    copy.forEach(element => {
      element.isSelected = false;
    });

    copy[index].isSelected = true;

    this.props.setDataStoragePlugins(copy);

  }

  @autobind
  renderServerSettings() {

    return <div className="row home">
      <form className="form-horizontal" onSubmit={this.updateServerSettings}>

        {this.props.serverSettings.map((setting, index) => {
          return <div className="form-group" key={index}>
            <label className="control-label col-sm-2" >{setting.displayName}:</label>
            <div className="col-sm-10">
              <input type="text"
                className="form-control"
                placeholder={setting.name}
                value={setting.value}
                readOnly={!setting.isEditable}
                pattern="^[0-9]+$"
                onChange={(e) => { this.setServerSetting(setting, index, e); }}
              />
            </div>
          </div>;
        })}
        <div className="form-group">
          <div className="col-sm-offset-2 col-sm-10">
            <button type="text" className="btn btn-default">Submit server settings</button>
          </div>
        </div>
      </form>
    </div>;
  }

  @autobind
  renderDataStoragePlugins() {

    return <div className="row home">
      <form className="form-horizontal" onSubmit={this.updateDataStoragePlugin}>

        {this.props.dataStoragePlugins.map((plugin, index) => {
          return <div className="radio" key={index}>
            <label>
              <input
                type="radio"
                name="datastorageplugin"
                checked={plugin.isSelected}
                onChange={(e) => { this.setDataStoragePlugins(plugin, index, e); }}
              />

              {plugin.displayName}
            </label>
          </div>;
        })}
        <div className="form-group">
          <div className="col-sm-offset-2 col-sm-10">
            <button type="text" className="btn btn-default">Submit data storage plugin</button>
          </div>
        </div>
      </form>
    </div>;
  }

  public render() {
    if (this.props.chartData) {
      return <div>
        {this.props.chartData &&
          this.renderChart()
        }
        <hr />
        {this.props.serverSettings &&
          this.renderServerSettings()
        }
        <hr />
        {this.props.dataStoragePlugins &&
          this.renderDataStoragePlugins()
        }
      </div>;
    }

    return null;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
    isFetching: state.home.isFetching,
    chartData: state.home.chartData as IDeviceLogsInChartFormat,
    serverSettings: state.home.serverSettings,
    dataStoragePlugins: state.home.dataStoragePlugins,
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  dispatchProps // Selects which action creators are merged into the component's props
)(Home);
