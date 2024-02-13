//using Android.SE.Omapi;
using Attendance.API;
using Attendance.Entities;
using Attendance.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.VM
{
    class SchoolGradeVM : ViewModelBase
    {
        AccountService _accountService;
        //SchoolGrade ltsGrade;
        public Command Tapped_For_Add_Command
        {
            get;
            set;
        }
        private List<SchoolGrade> _ltsGrade {  get; set; }
        public List<SchoolGrade> ltsGrade
        {
            get { return _ltsGrade; }
            set
            {
                if (_ltsGrade != value)
                {
                    _ltsGrade = value;
                    OnPropertyChange();
                }
                
            }
        }

        public SchoolGradeVM()
        {            
            InitVM();
            _accountService = new AccountService();
            Get_Information();
        }

        public void getInfo()
        {
            InitVM();
            _accountService = new AccountService();
            Get_Information();
        }

        void CleanData()
        {
            _ltsGrade = new List<SchoolGrade>();
        }

        private void InitVM()
        {
            Tapped_For_Add_Command = new Command(Tapped_For_Add);            
            CleanData();
        }

        private async void Get_Information()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    if (accessType == NetworkAccess.Internet)
                    {
                        var schoolGradeInfo = await _accountService.GetSchoolGradeInfo(Session._IdUser);
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(schoolGradeInfo);
                        if (jsonResult.status == "400")
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                        }
                        else if (jsonResult.status == "200")
                        {
                            //var _obj = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Result);
                            ltsGrade = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolGrade>>(jsonResult.Result.ToString());

                            //_ltsGrade
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                throw;
            }
        }

        private async void Tapped_For_Add(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    if (accessType == NetworkAccess.Internet)
                    {
                        //var dataInfoStatus = await _accountService.GetSchoolGradeInfo(Session._IdUser);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
