using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;


namespace SharpSample
{
    public partial class Form1 : Form
    {
        public delegate void onFinishedDelegate(bool bError, string strError);
        public delegate void onProcessingDelegate(int total, int processed);

        [DllImport("fftlib.dll")]
        public static extern void ConfigureFilter(int winType, int winSize);

        [DllImport("fftlib.dll")]
        public static extern void SetGain(double Ch1Gain, double Ch2Gain, double Ch3Gain, double Ch4Gain);

        [DllImport("fftlib.dll")]
        public static extern void SetScale(int chNo, double baseMin, double baseMax);

        [DllImport("fftlib.dll")]
        public static extern void SetFftFunction(int fnNo);

        [DllImport("fftlib.dll")]
        public static extern bool Start(string strFile, string strOutFile, int nThread);
        

        [DllImport("fftlib.dll")]
        public static extern void Pause();

        [DllImport("fftlib.dll")]
        public static extern bool GetStats(ref int errorCode, ref int total, ref int proccessed);


        int _total;
        int _processed;
        public Form1()
        {
            InitializeComponent();
        }
        private void _btn_start_Click(object sender, EventArgs e)
        {
            bool bRet = Start(@"d:\Binary File.bin", @"d:\1.bin", 400);
            if (bRet)
                timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int erroCode = 0;
            int nTotal = 0;
            int nProc = 0;
            bool bError = GetStats(ref erroCode, ref nTotal, ref nProc);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = nTotal;
            progressBar1.Value = nProc;

            if(nTotal > 0 && nProc >= nTotal)
            {
                timer1.Stop();
            }
        }

        private void onClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void _btnRead_Click(object sender, EventArgs e)
        {
            if (_txt_fileName.Text == "") return;
            Global g = Global.instance();                        
            g.reader.init(_txt_fileName.Text);

            //var block = g.reader.ReadBlock(4);
            //block = g.reader.ReadBlock(7);
            Chart dlg = new Chart();
            dlg.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            _txt_fileName.Text = dlg.FileName;
        }
    }
}
