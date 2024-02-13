using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _business = Attendance.Helpers;

namespace Attendance.VM
{
    class BusinessURLVM : ViewModelBase
    {
        string _busURL;
        public string _BusinessURL
        {
            get { return _busURL; }
            set
            {
                _busURL = value;
                OnPropertyChange();
            }
        }

        public Command Tapped_For_Business_Command
        {
            get;
            set;
        }

        public BusinessURLVM()
        {
            InitVM();
        }

        private void InitVM()
        {
            Tapped_For_Business_Command = new Command(Tapped_For_Business);
            CleanData();
        }

        private void CleanData()
        {
            _busURL = string.Empty;
        }

        private async void Tapped_For_Business(object sender)
        {
            try
            {                
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (!string.IsNullOrWhiteSpace(_BusinessURL))
                    {
                        _business.BusinessURL.SaveText(_BusinessURL);
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Saved URL", "OK");
                            await App.Current.MainPage.Navigation.PushModalAsync(new Pages.LoginA());
                            //await Shell.Current.GoToAsync("//LoginA");
                        });
                        
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Missing URL", "OK");
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsBusy = false;                
            }
        }
    }
}
