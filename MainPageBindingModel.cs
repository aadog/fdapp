using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fdapp
{
    public class MainPageBindingModel : INotifyPropertyChanged
    {
        public bool fridaIsStart=false;
        public bool executeIng;
        public string rootState = "(未root)";
      

        public string? FridaPort {
            get=>Preferences.Get("fridaPort","1234");
            set=>Preferences.Set("fridaPort",value);
        }
        public bool FridaUseNetwork {
            get => Preferences.Get("fridaUseNetwork", true);
            set => Preferences.Set("fridaUseNetwork", value);
        }
        public bool FridaStartIsEnable {
            get{ return !fridaIsStart&&!executeIng; }
            set { }
        }
        public bool FridaStopIsEnable
        {
            get { return fridaIsStart&&!executeIng; }
            set { }
        }
        public bool FridaClearIsEnable
        {
            get { return !executeIng; }
            set { }
        }

        public string RootState
        {
            get { return rootState; }
            set { }
        }

        //刷新字段
        public void Ref(string propertyName = null)
        {
            if (propertyName == null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FridaPort"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FridaUseNetwork"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FridaStartIsEnable"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FridaStopIsEnable"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FridaClearIsEnable"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RootState"));

            }
            else
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
