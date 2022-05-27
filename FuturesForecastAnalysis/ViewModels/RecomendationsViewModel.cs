using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace FuturesForecastAnalysis.ViewModels
{
    [Serializable]
    class RecomendationsViewModel : DefaultViewModelBase
    {
        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        private int _recomendationCount;
        public int RecomendationCount
        {
            get
            {
                return _recomendationCount;
            }
            set
            {
                _recomendationCount = value;
                OnPropertyChanged("RecomendationCount");
            }
        }

        private int _successCount;
        public int SuccessCount
        {
            get
            {
                return _successCount;
            }
            set
            {
                _successCount = value;
                OnPropertyChanged("SuccessCount");
            }
        }

        private int _failCount;
        public int FailCount
        {
            get
            {
                return _failCount;
            }
            set
            {
                _failCount = value;
                OnPropertyChanged("FailCount");
            }
        }
        
        private string _successRatio;
        public string SuccessRatio
        {
            get
            {
                return _successRatio;
            }
            set
            {
                _successRatio = value;
                OnPropertyChanged("SuccessRatio");
            }
        }

        private string _failRatio;
        public string FailRatio
        {
            get
            {
                return _failRatio;
            }
            set
            {
                _failRatio = value;
                OnPropertyChanged("FailRatio");
            }
        }

        private double _total;
        public double Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }
        public RecomendationsViewModel()
        { 
        }
        public RecomendationsViewModel(string code, int recomendationCount, int successCount, int failCount, double total)
        {
            Code = code;
            RecomendationCount = recomendationCount;
            SuccessCount = successCount;
            FailCount = failCount;
            Total = total;
            SuccessRatio = RecomendationCount == 0 ? "0%" : String.Format("{0:0.00}%", ((double)successCount) / ((double)recomendationCount) * 100);
            FailRatio = RecomendationCount == 0 ? "0%" : String.Format("{0:0.00}%", ((double)failCount) / ((double)recomendationCount) * 100);
        }
    }
}
