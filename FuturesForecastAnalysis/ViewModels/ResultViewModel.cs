using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace FuturesForecastAnalysis.ViewModels
{
    class ResultViewModel : DefaultViewModelBase
    {
        private string _date;
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }
        private string _sumTotal;
        public string SumTotal
        {
            get
            {
                return _sumTotal;
            }
            set
            {
                _sumTotal = value;
                OnPropertyChanged("SumTotal");
            }
        }
        private string _sumAll;
        public string SumAll
        {
            get
            {
                return _sumAll;
            }
            set
            {
                _sumAll = value;
                OnPropertyChanged("SumAll");
            }
        }
        private string _sumSuccess;
        public string SumSuccess
        {
            get
            {
                return _sumSuccess;
            }
            set
            {
                _sumSuccess = value;
                OnPropertyChanged("SumSuccess");
            }
        }
        private string _sumFail;
        public string SumFail
        {
            get
            {
                return _sumFail;
            }
            set
            {
                _sumFail = value;
                OnPropertyChanged("SumFail");
            }
        }
        private ObservableCollection<ResultViewModel> _children;
        public ObservableCollection<ResultViewModel> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }
        public ResultViewModel()
        {
        }
        public ResultViewModel(string date, string total, string all, string success, string fail)
        {
            Date = date;
            SumTotal = total;
            SumAll = all;
            SumSuccess = success;
            SumFail = fail;
        }
        public void AddChildren(IEnumerable<ResultViewModel> items)
        {
            Children = new ObservableCollection<ResultViewModel>(items);
        }
    }
}
