using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using OxyPlot;
using System.Threading;

namespace WPF
{
    public class ViewData
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] String propertyName = "") =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int cityN
        {
            get { return cityN; }
            set
            {
                cityN = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("cityN"));
            }
        }
        public int gencount
        {
            get { return gencount; }
            set
            {
                gencount = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("gencount"));
            }
        }
        public int maxpop
        {
            get { return maxpop; }
            set
            {
                maxpop = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("maxpop"));
            }
        }
        public List<double> OutputData
        {
            get { return OutputData; }
            set
            {
                OutputData = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OutputData"));
            }
        }
        public List<string> Res
        {
            get { return Res; }
            set
            {
                Res = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Res"));
            }
        }
        public void procces(CancellationToken cts, double[,] matrix)
        {
            OutputData = new List<double>();
            var gen = new Genetic.Genetic(matrix);
            var bestPath = gen.Calculate(maxpop, gencount);

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
