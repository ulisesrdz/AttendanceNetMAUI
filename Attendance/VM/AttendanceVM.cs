using Attendance.API;
using Attendance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.VM
{
    class AttendanceVM : ViewModelBase
    {
        List<AttendanceEnt> _lts;
        AccountService _accountService;
        public List<AttendanceEnt> _LtsAttendace
        {
            get { return _lts; }
            set 
            { 
                _lts = value;
                OnPropertyChange();
            }
        }
        public Command Tapped_Save_Command
        {
            get;
            set;
        }

        public AttendanceVM()
        {
            InitVM();
            _accountService = new AccountService();
        }

        private void InitVM()
        {
            Tapped_Save_Command = new Command(Tapped_For_Business);
            CleanData();
        }

        private void CleanData()
        {
           
            _lts = new List<AttendanceEnt>();
        }

        private async void Tapped_For_Business(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (_lts.Count > 0)
                        {
                            var attendanceStatus = await _accountService.SaveAttendanceData(_lts);
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await Application.Current.MainPage.DisplayAlert("Success", "Saved URL", "OK");
                                await Shell.Current.GoToAsync("//LoginA");
                            });

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Missing URL", "OK");
                        }
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
