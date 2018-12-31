using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WPF_Client.Commands;
using WPF_Client.Helpers;
using WPF_Client.Models;
using WPF_Client.Services;

namespace WPF_Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private PlotModel _plotModel;
        private readonly GlobalObject _globalSynchroObject;
        private readonly IHttpClient _client;

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { _plotModel = value; OnPropertyChanged("PlotModel"); }
        }


        public MainViewModel(GlobalObject globalSynchroObject, IHttpClient client)
        {
            _globalSynchroObject = globalSynchroObject;
            _client = client;

        }

        private DelegateCommand loadedCommand;

        public DelegateCommand LoadedCommand
        {
            get
            {
                return loadedCommand ??
                  (loadedCommand = new DelegateCommand(obj =>
                 {

                     Task.Run(async () =>
                     {
                         while (true)
                         {
                             Thread.Sleep(1000);
                           

                             var response = await _client.GetAsync<Response>(_globalSynchroObject.GetChartDataUrl());
                             _globalSynchroObject.IsIFirstStart = false;

                             if (response.StatusCode == (int)HttpStatusCode.OK)
                             {
                                 CreateChart(response.ChartData);
                             }
                         }
                     });
                 }));
            }
        }

        private void CreateChart(DeviceLogsInChartFormat chartData)
        {
            PlotModel plotModel = new PlotModel();

            plotModel.Title = chartData.ChartName;

            LinearAxis xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Title = chartData.AxesNames[0].ToLower();

            plotModel.Axes.Add(xAxis);

            for (var y = 1; y < chartData.AxesNames.Length; y++)
            {
                var seria = new LineSeries();

                var points = chartData.Logs.Select(log =>
                {
                    return new DataPoint(log.Values[0], log.Values[y]);
                });

                seria.Points.AddRange(points);
                seria.Title = chartData.AxesNames[y].ToLower();

                plotModel.Series.Add(seria);
            }

            PlotModel = plotModel;
            PlotModel.InvalidatePlot(true);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
