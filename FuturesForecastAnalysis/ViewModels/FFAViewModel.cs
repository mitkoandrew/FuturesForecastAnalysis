using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Net;
using System.Xml;
using System.Globalization;
using System.IO;
using System.Windows.Threading;
using System.Xml.Serialization;
using System.Net.Mail;
using System.Windows;
using Microsoft.Win32;

namespace FuturesForecastAnalysis.ViewModels
{
    internal class FFAViewModel : DefaultViewModelBase
    {
        const string Buy = "Buy";
        const string Success = "Success";
        const string Fail = "Fail";
        const string None = "None";
        private ObservableCollection<string> _archiveCodes = new ObservableCollection<string>();
        public ObservableCollection<string> ArchiveCodes
        {
            get
            {
                return _archiveCodes;
            }
            set
            {
                _archiveCodes = value;
                OnPropertyChanged("ArchiveCodes");
            }
        }
        private string _selectedArchiveCode = string.Empty;
        public string SelectedArchiveCode
        {
            get
            {
                return _selectedArchiveCode;
            }
            set
            {
                _selectedArchiveCode = value;
                UpdateArchiveContent(_selectedArchiveCode);
                OnPropertyChanged("SelectedArchiveCode");
            }
        }
        private ObservableCollection<FuturesArchiveViewModel> _futuresArchiveCollection = new ObservableCollection<FuturesArchiveViewModel>();
        public ObservableCollection<FuturesArchiveViewModel> FuturesArchiveCollection
        {
            get
            {
                return _futuresArchiveCollection;
            }
            set
            {
                _futuresArchiveCollection = value;
                OnPropertyChanged("FuturesArchiveCollection");
            }
        }
        private ObservableCollection<FuturesArchiveViewModel> _showFuturesArchiveCollection = new ObservableCollection<FuturesArchiveViewModel>();
        public ObservableCollection<FuturesArchiveViewModel> ShowFuturesArchiveCollection
        {
            get
            {
                return _showFuturesArchiveCollection;
            }
            set
            {
                _showFuturesArchiveCollection = value;
                OnPropertyChanged("ShowFuturesArchiveCollection");
            }
        }
        private ObservableCollection<RecomendationsViewModel> _recomendationsCollection = new ObservableCollection<RecomendationsViewModel>();
        public ObservableCollection<RecomendationsViewModel> RecomendationsCollection
        {
            get
            {
                return _recomendationsCollection;
            }
            set
            {
                _recomendationsCollection = value;
                OnPropertyChanged("RecomendationsCollection");
            }
        }
        private ObservableCollection<RecomendationsViewModel> _showRecomendationsCollection = new ObservableCollection<RecomendationsViewModel>();
        public ObservableCollection<RecomendationsViewModel> ShowRecomendationsCollection
        {
            get
            {
                return _showRecomendationsCollection;
            }
            set
            {
                _showRecomendationsCollection = value;
                OnPropertyChanged("ShowRecomendationsCollection");
            }
        }
        private RelayCommand _runAnalysisCommand;
        public RelayCommand RunAnalysisCommand
        {
            get
            {
                return _runAnalysisCommand ?? (_runAnalysisCommand = new RelayCommand(obj => RunAnalysis()));
            }
        }
        private RelayCommand _saveAnalysisCommand;
        public RelayCommand SaveAnalysisCommand
        {
            get
            {
                return _saveAnalysisCommand ?? (_saveAnalysisCommand = new RelayCommand(obj => SaveToXml()));
            }
        }
        private RelayCommand _updateFuturesArchive;
        public RelayCommand UpdateFuturesArchive
        {
            get
            {
                return _updateFuturesArchive ??
                    (_updateFuturesArchive = new RelayCommand(obj => UpdateArchive()));
            }
        }
        public FFAViewModel()
        {
        }

        private void RunAnalysis()
        {
            var codes = FuturesArchiveCollection.GroupBy(x => x.Code).Select(z => z.Key);
            foreach (var code in codes)
            {
                int successCounter = 0;
                int failCounter = 0;
                int allCounter = 0;
                var noneValue = FuturesArchiveCollection.FirstOrDefault(x => ((x.Code == code) && (x.Operation != None && string.IsNullOrEmpty(x.Result))));
                if (noneValue == null)
                    continue;
                var arch = FuturesArchiveCollection.Where(x => ((x.Code == code) && (Convert.ToDateTime(x.Date) >= Convert.ToDateTime(noneValue.Date))));
                foreach (var f in arch)
                {
                    if (f.Operation != None)
                    {
                        allCounter++;
                        var takeProfit = f.Operation == Buy ? f.Price + f.Volatility : f.Price - f.Volatility;
                        var stopLimit = f.Operation == Buy ? f.Price - f.Volatility : f.Price + f.Volatility;
                        var init_date = Convert.ToDateTime(f.Date);

                        var successValue = arch.Where(x => (Convert.ToDateTime(x.Date) > init_date) && (Convert.ToDateTime(x.Date) < init_date.AddDays(10))).FirstOrDefault(z => GetSuccess(f.Operation, z, takeProfit.Value));//z => z.Max >= takeProfit
                        var failValue = arch.Where(x => (Convert.ToDateTime(x.Date) > init_date) && (Convert.ToDateTime(x.Date) < init_date.AddDays(10))).FirstOrDefault(z => GetFail(f.Operation, z, stopLimit.Value));//(z => z.Min <= stopLimit);
                        if (successValue == null && failValue == null)
                        {
                            if (init_date.AddDays(10) >= GetCurrentDate())
                                f.Result = string.Empty;
                            else
                            {
                                var end = arch.FirstOrDefault(z => Convert.ToDateTime(z.Date) >= init_date.AddDays(10)) ??
                                    arch.Last();
                                var s = f.Operation == Buy ? (end.Price - f.Price) : (f.Price - end.Price);
                                f.Result = s >= 0 ? Success : Fail;
                                f.Total = s;
                            }
                            continue;
                        }
                        if (successValue != null && failValue == null)
                        {
                            successCounter++;
                            f.Result = Success;
                            f.Total = f.Volatility.Value;
                            continue;
                        }
                        if (successValue == null && failValue != null)
                        {
                            failCounter++;
                            f.Result = Fail;
                            f.Total = -f.Volatility.Value;
                            continue;
                        }
                        if (Convert.ToDateTime(successValue.Date) <= Convert.ToDateTime(failValue.Date))
                        {
                            successCounter++;
                            f.Result = Success;
                            f.Total = f.Volatility.Value;
                            continue;
                        }
                        else
                        {
                            failCounter++;
                            f.Result = Fail;
                            f.Total = -f.Volatility.Value;
                            continue;
                        }
                    }
                }
            }
            UpdateAnalysis();
            UpdateArchiveContent(SelectedArchiveCode);
        }
        private bool GetSuccess(string operation, FuturesArchiveViewModel f, double takeProfit)
        {
            return operation == Buy ? f.Max >= takeProfit : f.Min <= takeProfit;
        }
        private bool GetFail(string operation, FuturesArchiveViewModel f, double stopLoss)
        {
            return operation == Buy ? f.Min <= stopLoss : f.Max >= stopLoss;
        }
        private void UpdateAnalysis()
        {
            RecomendationsCollection.Clear();
            var codes = FuturesArchiveCollection.GroupBy(x => x.Code).Select(y => y.Key);
            foreach (var code in codes)
            {
                var arch = FuturesArchiveCollection.Where(x => x.Code == code);
                var allCounter = arch.Where(x => x.Operation != None) == null ? 0 : arch.Where(x => x.Operation != None).Count();
                var successCounter = arch.Where(x => x.Result == Success) == null ? 0 : arch.Where(x => x.Result == Success).Count();
                var failCounter = arch.Where(x => x.Result == Fail) == null ? 0 : arch.Where(x => x.Result == Fail).Count();
                var total = arch.Where(x => x.Operation != None) == null ? 0 : arch.Where(x => x.Operation != None).Sum(x => x.Total);
                RecomendationsCollection.Add(new RecomendationsViewModel(code, allCounter, successCounter, failCounter, total));
            }
        }
        private void UpdateArchiveContent(string code)
        {
            ShowFuturesArchiveCollection = new ObservableCollection<FuturesArchiveViewModel>(FuturesArchiveCollection.Where(x => (x.Code.Equals(code) && x.Price > 0)).OrderBy(z => Convert.ToDateTime(z.Date)));
            ShowRecomendationsCollection = new ObservableCollection<RecomendationsViewModel>(RecomendationsCollection.Where(z => z.Code.Equals(code)));
        }

        private DateTime GetCurrentDate()
        {
            return DateTime.Now.Date;
        }
        private void SaveToXml()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "trades_archive"; 
                saveFileDialog.DefaultExt = ".xml";
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                
                Nullable<bool> result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(FuturesArchiveViewModel[]));

                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        formatter.Serialize(fs, FuturesArchiveCollection.ToArray());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Unable to save data");
            }
        }
        private void UpdateArchive()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    FuturesArchiveCollection.Clear();
                    XmlSerializer formatter = new XmlSerializer(typeof(FuturesArchiveViewModel[]));

                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                    {
                        FuturesArchiveViewModel[] newpeople = (FuturesArchiveViewModel[])formatter.Deserialize(fs);

                        foreach (FuturesArchiveViewModel p in newpeople)
                        {
                            FuturesArchiveCollection.Add(p);
                        }
                    }

                    UpdateAnalysis();

                    ArchiveCodes = new ObservableCollection<string>(FuturesArchiveCollection.GroupBy(x => x.Code).Select(z => z.Key));
                    SelectedArchiveCode = ArchiveCodes.FirstOrDefault();
                }
            }
            catch
            {
                MessageBox.Show("Unable to Load Data for Analysis");
            }
        }
    }
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
