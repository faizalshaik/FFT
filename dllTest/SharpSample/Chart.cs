using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Geared;


namespace SharpSample
{
    public partial class Chart : Form
    {
        GearedValues<double> _values = new GearedValues<double>().WithQuality(Quality.High);
        public Chart()
        {
            InitializeComponent();
            Global g = Global.instance();            
            var values = g.reader.GetChannelVoltage(0);
            _values.AddRange(values);
        }

        private void init()
        {
            cartesianChart1.ScrollMode = ScrollMode.X;
            cartesianChart1.DisableAnimations = true;
            cartesianChart1.Hoverable = false;
            cartesianChart1.DataTooltip = null;

            cartesianChart1.AnimationsSpeed = TimeSpan.FromMilliseconds(0);
            cartesianChart1.Zoom = ZoomingOptions.X;

            var series = new LineSeries()
            {
                Values = _values,
                DataLabels = false,
                //Values = new ChartValues<double>(values),
                StrokeThickness = 1.2,
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 142, 196)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 1,
                PointGeometry = null,
                //PointGeometrySize = 10,
                //PointForeground =
                //    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49))
            };

            cartesianChart1.Series.Clear();
            cartesianChart1.Series.Add(series);

            List<string> labels = new List<string>();
            DateTime dt = DateTime.Now;
            for (int i=0; i< _values.Count/3; i++)
            {
                dt = dt.AddSeconds(1);
                labels.Add(dt.ToLongTimeString());
                labels.Add("");
                labels.Add("");
            }

            cartesianChart1.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49));
            cartesianChart1.AxisX.Add(new Axis
            {
                MinValue = 10,
                MaxValue = 30,
                DisableAnimations = true,
                IsMerged = false,
                Title = "Time",
                Labels = labels,
                LabelsRotation = -60,
                Separator = new Separator
                {
                    Step = 3,
                    StrokeThickness = 1,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 2 }),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }
            }); ;

            cartesianChart1.AxisY.Add(new Axis
            {
                DisableAnimations = true,
                Title = "Volts",
                //LabelFormatter = value => value.ToString("C")
                IsMerged = false,
                Separator = new Separator
                {
                    StrokeThickness = 1.5,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 4 }),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }
            });
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            init();
        }
    }
}
