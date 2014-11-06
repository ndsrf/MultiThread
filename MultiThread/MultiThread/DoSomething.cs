using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread
{
    class DoSomething
    {
        BackgroundWorker _bgw;
        DoWorkEventArgs _e;

        public enum TipoDeThreading { BackgroundWorker, Delegate }

        public delegate int GiveMeAsyncDelegate(int p1, int p2);

        public DoSomething()
        {
        }
        
        public DoSomething(BackgroundWorker bgw, DoWorkEventArgs e)
        {
            _bgw = bgw;
            _e = e;
        }

        public int DoCalculations(TipoDeThreading tipo, int param, int param2)
        {
            int x;

            switch (tipo)
            {
                case TipoDeThreading.BackgroundWorker: x = GiveMeBackgroundWorker(); break;
                case TipoDeThreading.Delegate : x = GiveMeDelegate(param, param2); break;
                default: x = 0; break;
            }

            return x;
        }

        public int GiveMeDelegate(int p1, int p2)
        {
            int res = 42;
            System.Threading.Thread.Sleep(2000);

            return res;
        }

        private int GiveMeBackgroundWorker()
        {
            System.Threading.Thread.Sleep(2000);
            if (_bgw.CancellationPending)
            {
                _e.Cancel = true;
                return 0;
            }
            _bgw.ReportProgress(25);
            System.Threading.Thread.Sleep(2000);
            if (_bgw.CancellationPending)
            {
                _e.Cancel = true;
                return 0;
            }
            _bgw.ReportProgress(50);
            System.Threading.Thread.Sleep(2000);
            if (_bgw.CancellationPending)
            {
                _e.Cancel = true;
                return 0;
            }
            _bgw.ReportProgress(75);
            System.Threading.Thread.Sleep(2000);
            if (_bgw.CancellationPending)
            {
                _e.Cancel = true;
                return 0;
            }
            return 42;
        }
    }
}
