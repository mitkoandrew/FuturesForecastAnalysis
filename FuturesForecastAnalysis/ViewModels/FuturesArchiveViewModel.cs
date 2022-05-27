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
    public class FuturesArchiveViewModel : DefaultViewModelBase
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
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _operation;
        public string Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
                OnPropertyChanged("Operation");
            }
        }

        private double _price;
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }
        private double? _buy;
        public double? Buy
        {
            get
            {
                return _buy;
            }
            set
            {
                _buy = value;
                OnPropertyChanged("Buy");
            }
        }
        private double? _sell;
        public double? Sell
        {
            get
            {
                return _sell;
            }
            set
            {
                _sell = value;
                OnPropertyChanged("Sell");
            }
        }
        private double? _volatility;
        public double? Volatility
        {
            get
            {
                return _volatility;
            }
            set
            {
                _volatility = value;
                OnPropertyChanged("Volatility");
            }
        }
        private int _method;
        public int Method
        {
            get
            {
                return _method;
            }
            set
            {
                _method = value;
                OnPropertyChanged("Method");
            }
        }
        private double? _min;
        public double? Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                OnPropertyChanged("Min");
            }
        }
        private double? _max;
        public double? Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                OnPropertyChanged("Max");
            }
        }

        private string _result;
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
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

        public FuturesArchiveViewModel()
        { 
        }
    }
}
