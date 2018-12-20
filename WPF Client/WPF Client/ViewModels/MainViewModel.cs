using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WPF_Client.Commands;
using WPF_Client.Helpers;
using WPF_Client.Models;
using WPF_Client.Services;

namespace WPF_Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private PlotModel _plotModel;
        private readonly GlobalSynchroObject _globalSynchroObject;

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { _plotModel = value; OnPropertyChanged("PlotModel"); }
        }


        public MainViewModel(GlobalSynchroObject globalSynchroObject)
        {
            _globalSynchroObject = globalSynchroObject;

        }

        private DelegateCommand loadedCommand;

        public DelegateCommand LoadedCommand
        {
            get
            {
                return loadedCommand ??
                  (loadedCommand = new DelegateCommand(obj =>
                 {
                     RestSharpHttpClient _client = new RestSharpHttpClient();


                     Task.Run(async () =>
                     {
                         while (true)
                         {

                             Thread.Sleep(1000);

                             PlotModel plotModel = new PlotModel();

                             var response = await _client.GetAsync<Response>($"api/log-collector/get-logs?utcDate=&isInitial={_globalSynchroObject.IsIFirstStart}");
                             _globalSynchroObject.IsIFirstStart = false;

                             PropertyInfo[] info = new PropertyInfo[100];

                             if (response.StatusCode == (int)HttpStatusCode.OK)
                             {
                                 var firstChartData = response.Logs.FirstOrDefault();

                                 // render base elements
                                 if (firstChartData != null)
                                 {
                                     plotModel.Title = firstChartData.DeviceName;

                                     var firstLog = firstChartData.Logs.FirstOrDefault();
                                     Type firstType = firstLog.GetType();

                                     PropertyInfo[] properties = firstType.GetProperties();
                                     info = properties;

                                     LinearAxis xAxis = new LinearAxis();
                                     xAxis.Position = AxisPosition.Bottom;
                                     xAxis.Title = properties[0].Name;

                                     LinearAxis yAxis = new LinearAxis();
                                     yAxis.Position = AxisPosition.Left;
                                     yAxis.Title = "Values";

                                     plotModel.Axes.Add(xAxis);
                                     plotModel.Axes.Add(yAxis);
                                 }

                                 plotModel.InvalidatePlot(true);

                                 var collections = info.Skip(1).Select(x =>
                                 {
                                     return new List<DataPoint>();
                                 }).ToArray();


                                 foreach (var log in response.Logs[0].Logs)
                                 {
                                     var type = log.GetType();
                                     var props = type.GetProperties();

                                     for (int i = 1; i < props.Length; i++)
                                     {
                                         var key = Convert.ToDouble(props[0].GetValue(log));
                                         var val = Convert.ToDouble(props[i].GetValue(log));
                                         var data = new DataPoint(key, val);

                                         collections[i - 1].Add(data);
                                     }
                                 }

                                 for (var i = 0; i < collections.Length; i++)
                                 {
                                     var seria = new LineSeries();

                                     seria.Points.AddRange(collections[i]);

                                     seria.Title = info[i + 1].Name;

                                     plotModel.Series.Add(seria);
                                 }
                             }

                             PlotModel = plotModel;

                             PlotModel.InvalidatePlot(true);
                         }
                     });
                 }));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
