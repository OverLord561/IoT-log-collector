import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/index";
import { RouteComponentProps } from "react-router-dom";
import * as actions from "./logic/homeActions";
import { IDeviceLogsInChartFormat, IServerSettingViewModel, IDataStoragePlugin, IDevicePlugin } from './logic/homeState';
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

const devicePluginDropdownId = 'devicePluginsDropdown';
interface IState {
  pluginsDropDownOpened: boolean;
  selectedDevicePlugin: IDevicePlugin;
}
interface IStateToProps {
  authorized: boolean;
  isFetching: boolean;
  chartData: IDeviceLogsInChartFormat;
  serverSettings: IServerSettingViewModel[];
  dataStoragePlugins: string[];
  devicePlugins: IDevicePlugin[];
}

const dispatchProps = {
  loadLogData: actions.LoadLogData,
  getServerSettings: actions.GetServerSettings,
  updateServerSettings: actions.UpdateServerSettings,
  updateDataStoragePlugin: actions.UpdateDataStoragePlugin,
  setServerSettings: actions.SetServerSettings,
  setDataStoragePlugins: actions.SetDataStoragePlugins,
  getDataStoragePlugins: actions.GetDataStoragePlugins,
  getDevicePlugins: actions.GetDevicePlugins,
};

type IProps = IStateToProps & RouteComponentProps<{}> & typeof dispatchProps;

class Home extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);

    this.state = {
      pluginsDropDownOpened: false,
      selectedDevicePlugin: {
        value: '',
        displayName: 'Select plugin'
      }
    };
  }

  @autobind
  loadSettings() {
    this.props.getServerSettings();
    this.props.getDataStoragePlugins();
  }

  @autobind
  refreshDataAndSettings() {
    this.loadSettings();

    const Utc = moment().format("X");
    this.props.loadLogData(Utc, this.state.selectedDevicePlugin.value, true);
  }

  async componentDidMount() {
    this.loadSettings();
    const succeeded = await this.props.getDevicePlugins();

    if (succeeded) {

      const firstPlugin: IDevicePlugin = this.props.devicePlugins[0];
      this.setState({
        selectedDevicePlugin: firstPlugin
      }, () => {
        const Utc = moment().format("X");
        this.props.loadLogData(Utc, this.state.selectedDevicePlugin.value, true);
      });
    }

  }

  componentDidUpdate(prevProps: IProps, prevState: any) {

    if (JSON.stringify(this.props.devicesLogs) !== JSON.stringify(prevProps.devicesLogs)) {
      const Utc = moment().format("X");
      this.props.loadLogData(Utc, this.state.selectedDevicePlugin.value, false);
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
  togooglePluginsDropDown(e) {
    e.persist();
    if (e.target && e.target.id === devicePluginDropdownId) {
      this.setState(
        { pluginsDropDownOpened: !this.state.pluginsDropDownOpened }
      );
    } else {
      this.setState(
        { pluginsDropDownOpened: false }
      );
    }

  }

  @autobind
  changeDevicePluginDataRepresentation(plugin: IDevicePlugin, e) {

    this.setState({
      selectedDevicePlugin: plugin
    }, () => {
      const Utc = moment().format("X");
      this.togooglePluginsDropDown(e);
      this.props.loadLogData(Utc, plugin.value, true);
    });
  }

  @autobind
  renderDevicePlugins() {
    let dropdownClassName = "dropdown";

    if (this.state.pluginsDropDownOpened) {

      dropdownClassName = dropdownClassName.concat(' ').concat('open');
    }

    let colClassName = " col-sm-4";

    if (!this.props.chartData) {
      colClassName = "col-sm-offset-8".concat(colClassName);
    }

    return <div className={colClassName}>
      <div className={dropdownClassName} >
        <button
          className="btn btn-primary dropdown-toggle"
          type="button"
          data-toggle="dropdown"
          id={devicePluginDropdownId}
          onClick={this.togooglePluginsDropDown}
        >
          {this.state.selectedDevicePlugin.displayName}
          <span className="caret"></span>
        </button>
        <ul className="dropdown-menu">
          {this.props.devicePlugins.map((plugin, index) => {
            return <li key={index} onClick={(e) => {
              this.changeDevicePluginDataRepresentation(plugin, e);
            }}>
              <a href="#">{plugin.displayName}</a>
            </li>;
          })}

        </ul>
      </div>
    </div >;
  }

  @autobind
  renderChart() {

    const chartData: IDeviceLogsInChartFormat = this.props.chartData;
    const data = this.prepareDataForChartLibrary();

    return <div className="col-sm-8">
      <h3 className="purpose">{chartData.chartName}</h3>
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
    </div>;
  }

  @autobind
  async updateServerSettings(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const succeeded = await this.props.updateServerSettings(this.props.serverSettings);

    if (succeeded) {

      this.refreshDataAndSettings();
    }
  }

  @autobind
  setServerSetting(setting: IServerSettingViewModel, index: number, event: React.FormEvent<HTMLInputElement>) {
    const copy = this.props.serverSettings.slice();

    copy[index].value = event.currentTarget.value;

    this.props.setServerSettings(copy);
  }

  @autobind
  async updateDataStoragePlugin(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const selectedPlugin = this.props.dataStoragePlugins.find(plugin => plugin.isSelected);

    const succeeded = await this.props.updateDataStoragePlugin(selectedPlugin);

    if (succeeded) {
      this.refreshDataAndSettings();
    }
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

    return <div className="col-sm-6">
      <fieldset>
        <legend>Server settings</legend>
        <form className="form-horizontal" onSubmit={this.updateServerSettings}>

          {this.props.serverSettings.map((setting, index) => {
            return <div className="form-group" key={index}>
              <label className="control-label col-sm-4" >{setting.displayName}:</label>
              <div className="col-sm-8">
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
            <div className="col-sm-offset-4 col-sm-8">
              <button type="text" className="btn btn-default">Submit</button>
            </div>
          </div>
        </form>
      </fieldset>
    </div>;
  }

  @autobind
  renderDataStoragePlugins() {

    return <div className="col-sm-6">
      <fieldset>
        <legend>Data storage settings</legend>
        <form className="form-horizontal" onSubmit={this.updateDataStoragePlugin}>

          {this.props.dataStoragePlugins.map((plugin, index) => {
            return <div className="radio" key={index}>
              <label className="col-sm-offset-2 col-sm-10">
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
          <br />
          <div className="form-group">
            <div className="col-sm-offset-2 col-sm-10">
              <button type="text" className="btn btn-default">Submit</button>
            </div>
          </div>
        </form>
      </fieldset>
    </div>;
  }

  public render() {

    return <div>

      <div className="row home chart" onClick={this.togooglePluginsDropDown} >

        {this.props.chartData ?
          this.renderChart()
          :
          this.props.devicePlugins &&
          <h1>No logs for {this.state.selectedDevicePlugin.displayName}</h1>
        }

        {this.props.devicePlugins &&
          this.renderDevicePlugins()
        }
      </div>
      <hr />
      <div className="row home">
        {this.props.serverSettings &&
          this.renderServerSettings()
        }

        {this.props.dataStoragePlugins &&
          this.renderDataStoragePlugins()
        }
      </div>
      <hr />

    </div>;
  }
}

const mapStateToProps = (state: IApplicationState): IStateToProps => {
  return {
    authorized: state.signIn.authorized,
    isFetching: state.home.isFetching,
    chartData: state.home.chartData as IDeviceLogsInChartFormat,
    serverSettings: state.home.serverSettings,
    dataStoragePlugins: state.home.dataStoragePlugins,
    devicePlugins: state.home.devicePlugins
  };
};

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  dispatchProps // Selects which action creators are merged into the component's props
)(Home);
