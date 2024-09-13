using Attendance.API;
using Attendance.Entities;
using Attendance.Helpers;

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using page = Attendance.Pages;

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
        private int _id_course;
        public int Id_Course
        {
            get { return _id_course; }
            set
            {
                _id_course = value;
                OnPropertyChange();
            }
        }

        private List<SchoolGrade> _ltsGrade { get; set; }
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

        private List<AttendanceEnt> _ltsAttendance { get; set; }
        public List<AttendanceEnt> ltsAttendance
        {
            get { return _ltsAttendance; }
            set
            {
                if (_ltsAttendance != value)
                {
                    _ltsAttendance = value;
                    OnPropertyChange();
                }

            }
        }

        private SchoolGrade _onItemSelected;

        public SchoolGrade ItemSelected
        {
            get { return _onItemSelected; }
            set
            {
                if (_onItemSelected != value)
                {
                    _onItemSelected = value;
                    OnPropertyChange("ItemSelected");
                }
            }
        }
        public Command Tapped_Save_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Enter_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Enter_User_Command
        {
            get;
            set;
        }

        public AttendanceVM()
        {
            InitVM();
            _accountService = new AccountService();
            //Get_Information();
            if (Session.attendanceView)
            {
                Get_InformationLocal();
            }
           
        }

        private void InitVM()
        {
            Tapped_Save_Command = new Command(Tapped_For_BusinessLocal);
            Tapped_For_Enter_Command = new Command(Tapped_For_Enter);
            Tapped_For_Enter_User_Command = new Command(Tapped_For_Enter_Users);
            CleanData();
        }

        private void CleanData()
        {
           
            _lts = new List<AttendanceEnt>();
        }

        #region API
        private async void Tapped_For_Enter(object sender)
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy=true;
                    if (Session.attendanceView)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                    }
                    else
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.Attendance());
                    }
                    
                    IsBusy=false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void Tapped_For_Enter_Users(object sender)
        {

            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                    IsBusy = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
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
                        else if (jsonResult.status == "500")
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
                            foreach (var item in _lts)
                            {
                                var attendanceStatus = await _accountService.SaveAttendanceData(item);
                                var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(attendanceStatus);
                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    if (jsonResult.status == "400")
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                                    }
                                    else if (jsonResult.status == "500")
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                                    }
                                    else if (jsonResult.status == "200")
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Success", jsonResult.Result.ToString(), "Aceptar");
                                    }
                                });
                            }
                            

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

        #endregion

        #region Local
        private async void Get_InformationLocal()
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    int counter = 0;
                    var ltsAtt = new List<AttendanceEnt>();
                    var attenaces = await App.DataBase.getAttendacebyIdUserAsync(Session._IdUser.ToString());
                    //var student = await App.DataBase.getAttendacebyIdUserAsync(Session.);
                    if (attenaces.Count > 0)
                    {
                        foreach (var item in attenaces)
                        {
                            var attendace = new AttendanceEnt();
                            var id_student = Convert.ToInt32(item.id_student);
                            var student = await App.DataBase.getStudentbyIdAsync(id_student);

                            attendace.counter = counter++;
                            attendace.id_course = item.id_course;
                            attendace.id_student = item.id_student;
                            attendace.student_name = student.name + " " + student.last_name;
                            attendace.date_time = item.date_time;

                            ltsAtt.Add(attendace);
                        }

                        if (ltsAtt.Count>0)
                        {
                            ltsAttendance = ltsAtt;
                        }
                    }
                    //{
                    //    var _grade = new List<SchoolGrade>();
                    //    foreach (var item in schoolGradeInfo)
                    //    {
                    //        var grade = new SchoolGrade();
                    //        grade.id = item.id;
                    //        grade.id_user = item.id_user;
                    //        grade.course_name = item.course_name;
                    //        grade.groups = item.groups;
                    //        grade.grade = item.grade;


                    //        _grade.Add(grade);
                    //    }

                    //    if (_grade.Count > 0)
                    //    {
                    //        ltsGrade = _grade;
                    //    }
                    //}
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsBusy = false;
                throw;
            }
        }

        private async void Tapped_For_BusinessLocal(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (_lts.Count > 0)
                    {
                        foreach (var item in _lts)
                        {
                            var id_student = Convert.ToInt32(item.id_student);
                            var id_course = Convert.ToInt32(item.id_course);
                            var exists= await App.DataBase.getStudentbyIdAsync(id_student, id_course);
                            if (!exists)
                            {
                                await Application.Current.MainPage.DisplayAlert("Failed", "This student " + item.student_name + " is not registred in this class", "Ok");
                                return;
                            }
                            var attendance = new AttendanceEntSQLite
                            {
                                id_course = item.id_course,
                                date_time = DateTime.Now,
                                id_student = item.id_student,
                                id_user = item.id_user

                            };

                            var result = await App.DataBase.CreateAttendaceAsync(attendance);
                            if (result > 0)
                            {
                                await Application.Current.MainPage.DisplayAlert("Success", "Data Saved for " + item.student_name, "Ok");
                                CleanData();
                            }
                            
                        }

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
        #endregion
    }
}
