using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThread
{
    public partial class Form1 : Form
    {
        BackgroundWorker bgw;
        int res;

        private const int PARAM1 = 55;
        private const int PARAM2 = 66;

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = true;
            button2.Enabled = false;
            progressBar1.Maximum = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            progressBar1.Value = 0;
            bgw.DoWork += bgw_DoWork;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.WorkerSupportsCancellation = true;
            button1.Enabled = false;
            button2.Enabled = true;
            label1.Text = "Calculando...";
            bgw.RunWorkerAsync();
        }

        private void FinBackgroundWorker()
        {
            bgw.DoWork -= bgw_DoWork;
            bgw.ProgressChanged -= bgw_ProgressChanged;
            bgw.RunWorkerCompleted -= bgw_RunWorkerCompleted;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                progressBar1.Value = 100;
                label1.Text = res.ToString();
            }

            FinBackgroundWorker();
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            DoSomething doS = new DoSomething(bgw, e);
            res = doS.DoCalculations(DoSomething.TipoDeThreading.BackgroundWorker, PARAM1 , PARAM2 );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bgw.CancelAsync();
            label1.Text = "Cancelado";
            progressBar1.Value = 0;
            FinBackgroundWorker();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DoSomething ds = new DoSomething();
            
            DoSomething.GiveMeAsyncDelegate dsAsyncCaller = new MultiThread.DoSomething.GiveMeAsyncDelegate(ds.GiveMeDelegate);

            IAsyncResult asyncDelegateResult = dsAsyncCaller.BeginInvoke(PARAM1, PARAM2, null, null);

            while (!asyncDelegateResult.IsCompleted)
            {
                Thread.Sleep(5);
                if (progressBar1.Value < progressBar1.Maximum)
                {
                    progressBar1.Value = progressBar1.Value + 10;
                    progressBar1.Refresh();
                }
            }

            int res = dsAsyncCaller.EndInvoke(asyncDelegateResult);

            label2.Text = res.ToString();
        }
    }



}
