using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace WPF
{
    public class ViewData : INotifyPropertyChanged
    {
        private int _cityN;
        private int _gencount;
        private int _maxpop;
        private List<double> _outputData;
        private List<string> _res;
        private double _bestFitness;
        private string _bestPath;
        private CancellationTokenSource _cts;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int cityN
        {
            get { return _cityN; }
            set
            {
                _cityN = value;
                RaisePropertyChanged();
            }
        }

        public int gencount
        {
            get { return _gencount; }
            set
            {
                _gencount = value;
                RaisePropertyChanged();
            }
        }

        public int maxpop
        {
            get { return _maxpop; }
            set
            {
                _maxpop = value;
                RaisePropertyChanged();
            }
        }

        public List<double> OutputData
        {
            get { return _outputData; }
            set
            {
                _outputData = value;
                RaisePropertyChanged();
            }
        }

        public List<string> Res
        {
            get { return _res; }
            set
            {
                _res = value;
                RaisePropertyChanged();
            }
        }

        public double BestFitness
        {
            get { return _bestFitness; }
            set
            {
                _bestFitness = value;
                RaisePropertyChanged();
            }
        }

        public string BestPath
        {
            get { return _bestPath; }
            set
            {
                _bestPath = value;
                RaisePropertyChanged();
            }
        }

        public ViewData()
        {
            OutputData = new List<double>();
            Res = new List<string>();
        }

        public async Task ProcessAsync(CancellationToken cts, double[,] matrix)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cts);
            OutputData = new List<double>();
            var gen = new Genetic.Genetic(matrix);
            var bestPath = await Task.Factory.StartNew(() =>  gen.Calculate(_cts.Token, maxpop, gencount), _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            // StartNew flag LongRunning
            BestPath = bestPath.ToString();
            BestFitness = gen.Metric(bestPath);
            RaisePropertyChanged(nameof(Res));

            
        }

        public void StopProcess()
        {
            _cts?.Cancel();
        }
    }

    public class Result
    {
        public int curgen;
        public string curbestpath;
        public string metric;

        public Result(int curg, string cur, string m)
        {
            curgen = curg;
            curbestpath = cur;
            metric = m;
        }

        public override string ToString()
        {
            return "Generation:" + curgen + " " + metric + " " + curbestpath;
        }
    }
}
