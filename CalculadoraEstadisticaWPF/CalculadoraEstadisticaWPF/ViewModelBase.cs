using LiveCharts;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace CalculadoraEstadisticaWPF
{
    class ViewModelBase: INotifyPropertyChanged
    {

        #region Attributes
        string txtInput = "0;1;2;3;4;5;6;7;8;9;0;1;2;3;4;5;6;7;8;9;0;1;2;3;4;5;6;7;8;9;0;1;2;3;4;5;6;7;8;9";
        string txtOutput = "";
        SeriesCollection series;
        #endregion

        #region Constructor
        public ViewModelBase()
        {
            series = new SeriesCollection();
        }
        #endregion

        #region Properties
        public string TxtInput
        {
            get
            {
                return txtInput;
            }

            set
            {
                txtInput = value;
                RaisePropertyChanged("TxtInput");
            }
        }

        public string TxtOutput
        {
            get
            {
                return txtOutput;
            }

            set
            {
                txtOutput = value;
                RaisePropertyChanged("TxtOutput");
            }
        }

        public SeriesCollection Series
        {
            get
            {
                return series;
            }

            set
            {
                series = value;
                RaisePropertyChanged("Series");
            }
        }

       
        #endregion

        #region Buttons

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), CanExecuteAction()));
            }
        }

        private bool CanExecuteAction()
        {
            return true;
        }

        public void MyAction()
        {
            try
            {
                #region Update TxtOutput
                string txtSalida = string.Empty;
                string txtEntrada = String.Copy(TxtInput);
                string salto = "\n";
                txtEntrada.Replace(salto, ""); 
                double[] data = TxtInput.Split(';').Select(n => Convert.ToDouble(n)).ToArray();
                txtSalida += "Minimum: " + data.Minimum();
                txtSalida += salto + "Maximum: " + data.Maximum();
                txtSalida += salto + "Count: " + data.Count();
                txtSalida += salto + "Mean: " + data.Mean();
                txtSalida += salto + "Median: " + data.Median();
                txtSalida += salto + "Variance: " + data.Variance();
                txtSalida += salto + "StandardDeviation: " + data.StandardDeviation();
                txtSalida += salto + "MaximumAbsolute: " + data.MaximumAbsolute();
                txtSalida += salto + "MinimumAbsolute: " + data.MinimumAbsolute();              
                TxtOutput = txtSalida;
                #endregion

                #region Update Chart Line
                ChartValues<double> cv = new ChartValues<double>();
                cv.AddRange(data);

                var lineSerie = new LineSeries
                {
                    Title = "Values",
                    Values = cv,
                };

                //Bug Series.clear()
                while (series.Count > 0)
                {
                    series.RemoveAt(series.Count - 1);
                }

                series.Add(lineSerie);
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
