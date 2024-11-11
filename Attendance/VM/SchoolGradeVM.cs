//using Android.SE.Omapi;
using Attendance.API;
using Attendance.Entities;
using Attendance.Helpers;
using Attendance.Pages;
using page = Attendance.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Resources.Localization;
using System.Collections.ObjectModel;

namespace Attendance.VM
{
    class SchoolGradeVM : ViewModelBase
    {
        #region Properties
        AccountService _accountService;        

        private string _name;
        private string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChange();
                }

            }
        }

        private string _courseName;
        public string CourseName
        {
            get { return _courseName; }
            set
            {
                if (_courseName != value)
                {
                    _courseName = value;
                    OnPropertyChange();
                }

            }
        }

        private string _grade;
        public string Grade
        {
            get { return _grade; }
            set
            {
                if (_grade != value)
                {
                    _grade = value;
                    OnPropertyChange();
                }

            }
        }

        private string _group;
        public string Group
        {
            get { return _group; }
            set
            {
                if (_group != value)
                {
                    _group = value;
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
        
        private ObservableCollection<SchoolGrade> _ltsGrade { get; set; }
        public ObservableCollection<SchoolGrade> ltsGrade
        {
            get { return _ltsGrade; }
            set
            {
                if (_ltsGrade != value)
                {
                    _ltsGrade = value;
                    OnPropertyChange();
                }
                else
                {

                }

            }
        }

        private SchoolGrade _infoSelected { get; set; }
        public SchoolGrade InfoSeletected
        {
            get { return _infoSelected; }
            set
            {
                if (_infoSelected != value)
                {
                    _infoSelected = value;
                    OnPropertyChange();
                }

            }
        }

        #endregion

        #region Command

        public Command Tapped_For_Add_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Enter_Command
        {
            get;
            set;
        }

        public Command Tapped_For_Subject_Command
        {
            get;
            set;
        }
        public Command Tapped_For_DeleteCourse_Command
        {
            get;
            set;
        }
        public Command Tapped_For_DeleteCourses_Command
        {
            get;
            set;
        }

        #endregion

        
        public SchoolGradeVM()
        {            
            InitVM();
            CleanData();
            _accountService = new AccountService();
            if (Session.studentsListView || Session.attendanceView || Session.attendance || Session.schoolGrade)
            {
                Get_InformationLocal();
            }
            
        }
        
        public void getInfo()
        {
            InitVM();
            _accountService = new AccountService();
            //Get_InformationLocal();
        }

        void CleanData()
        {
            _name = String.Empty;
            _group = String.Empty;
            _grade = String.Empty;
            Name = String.Empty;
            Group = String.Empty;
            Grade = String.Empty;
            CourseName = String.Empty;
            _courseName = String.Empty;
            ltsGrade = new ObservableCollection<SchoolGrade>();
            _ltsGrade = new ObservableCollection<SchoolGrade>();
        }        

        private void InitVM()
        {
            Tapped_For_Enter_Command = new Command(Tapped_For_EnterLocal);
            Tapped_For_Add_Command = new Command(Tapped_For_AddLocal);
            Tapped_For_DeleteCourse_Command = new Command(Tapped_For_DeleteCourseLocal);
            Tapped_For_DeleteCourses_Command = new Command(Tapped_For_DeleteCoursesLocal);
            Tapped_For_Subject_Command = new Command(Tapped_For_Subject);
            CleanData();
        }

        #region API
        private async void Get_InformationAPI()
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
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                        }
                        else if (jsonResult.status == "500")
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                        }
                        else if (jsonResult.status == "200")
                        {
                            //var _obj = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Result);
                            ltsGrade = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<SchoolGrade>>(jsonResult.Result.ToString());

                            //_ltsGrade
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
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
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (!string.IsNullOrWhiteSpace(_courseName))
                        {
                            var course = new SchoolGrade
                            {
                                course_name = _courseName,
                                id_user = Session._IdUser,
                                grade = _grade,
                                groups = _group
                            }; 
                            var result = await _accountService.AddCourse(course);

                            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(result);

                            if (jsonResult.status == "400")
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {

                                string jsonObj = jsonResult.Result.ToString();

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());                               
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                            }
                        }

                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_OK);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }
        }

        private async void Tapped_For_Enter(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        Session.Id_Course = ItemSelected.id;
                        Session._IdUser = ItemSelected.id_user;

                        if (Session.attendanceView)
                        {                           
                            await App.Current.MainPage.Navigation.PushAsync(new page.AttendanceView());
                        }
                        else if (Session.attendance)                        
                        {
                            //await App.Current.MainPage.Navigation.PushAsync(new page.Attendance());
                            await App.Current.MainPage.Navigation.PushAsync(App.Services.GetService<page.Attendance>());
                        }
                        else if (ItemSelected != null)
                        {
                            if(Session.schoolGrade)
                            {
                                var opt = await Application.Current.MainPage.DisplayAlert(AppResource.Common_Question, AppResource.SchooGradel_ListView, AppResource.CommonYes, AppResource.CommonNo);
                                if (opt)
                                {                                    
                                    Session.attendanceView = true;
                                    Session.studentsListView = true;
                                    await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                                }
                                else
                                {
                                    Session.attendanceView = false;                                    
                                    await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());                                   
                                }
                            }                           
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.SchoolGrade_CourseNotSelected, AppResource.Common_OK);
                        }                        
                        
                    }
                    else
                    {
                        if (ItemSelected != null)
                        {
                            Session.Id_Course = ItemSelected.id;
                            Session._IdUser = ItemSelected.id_user;
                            await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.SchoolGrade_CourseNotSelected, AppResource.Common_OK);
                        }
                    }
                    IsBusy = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void Tapped_For_DeleteCourse(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (ItemSelected != null)
                        {
                            var result = await _accountService.DeleteCourse(ItemSelected.id_user,ItemSelected.id);

                            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(result);

                            if (jsonResult.status == "400")
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {
                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());                                
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                            }
                        }

                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_OK);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }


        }

        private async void Tapped_For_DeleteCourses(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (Session._IdUser > 0)
                        {
                            var result = await _accountService.DeleteCourse(ItemSelected.id_user);

                            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(result);

                            if (jsonResult.status == "400")
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {
                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());                                
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                            }
                        }
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_OK);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }


        }

        private async void Tapped_For_Subject(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CourseList());            
        }
        #endregion

        #region Local

        private async void Tapped_For_EnterLocal(object sender)
        {           
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    if(ItemSelected == null)
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.SchoolGrade_CourseNotSelected, AppResource.Common_OK);
                        IsBusy = false;
                        return;
                    }

                    Session.Id_Course = ItemSelected.id;
                    Session._IdUser = ItemSelected.id_user;

                    if (Session.attendanceView)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.AttendanceView());
                    }
                    else if (Session.attendance)
                    {
                        //await App.Current.MainPage.Navigation.PushAsync(new page.Attendance());
                        await App.Current.MainPage.Navigation.PushAsync(App.Services.GetService<page.Attendance>());
                    }
                    else if (ItemSelected != null)
                    {
                       

                        if (Session.schoolGrade)
                        {
                            var opt = await Application.Current.MainPage.DisplayAlert(AppResource.Common_Question, AppResource.SchooGradel_ListView, AppResource.CommonYes, AppResource.CommonNo);
                            if (opt)
                            {
                                Session.attendanceView = true;
                                Session.studentsListView = true;
                                await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                            }
                            else
                            {
                                Session.attendanceView = false;
                                await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.SchoolGrade_CourseNotSelected, AppResource.Common_OK);
                    }
                    IsBusy = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async void Get_InformationLocal()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var schoolGradeInfo = await App.DataBase.getSchoolGradebyIdUserAsync(Session._IdUser);

                    if (schoolGradeInfo.Count > 0)
                    {
                        _ltsGrade.Clear();
                        foreach (var item in schoolGradeInfo)
                        {
                            var grade = new SchoolGrade();
                            grade.id = item.id;
                            grade.id_user = item.id_user;
                            grade.course_name = item.course_name;
                            grade.groups = item.groups;
                            grade.grade = item.grade;


                            _ltsGrade.Add(grade);
                        }

                        
                    }
                    else
                    {
                        _ltsGrade = new ObservableCollection<SchoolGrade>();
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    IsBusy = false;
                    //CleanData();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                await Application.Current.MainPage.Navigation.PopAsync();
                throw;
            }
        }

        private async void Tapped_For_AddLocal(object sender)
        {            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var exist = await App.DataBase.existSchoolGradeAsync(Session._IdUser, _courseName);
                    if (exist.Count > 0)
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.SchoolGrade_RecordExist, AppResource.Common_OK);

                        IsBusy = false;
                        CleanData();
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(_courseName))
                    {
                        var course = new SchoolGradeSQLite
                        {
                            course_name = _courseName,
                            id_user = Session._IdUser,
                            grade = _grade,
                            groups = _group
                        };

                        var schoolGradeInfo = await App.DataBase.CreateGradeAsync(course);
                        if (schoolGradeInfo>0)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.Common_RecordSaved, AppResource.Common_OK);
                            CleanData();
                        }
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_OK);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }
        }

        private async void Tapped_For_DeleteCourseLocal(object sender)
        {            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (ItemSelected != null)
                    {
                        var item = new SchoolGradeSQLite
                        {
                            id = ItemSelected.id,
                            id_user = ItemSelected.id_user,
                            course_name = ItemSelected.course_name,
                            grade = ItemSelected.grade,
                            groups = ItemSelected.groups
                        };
                        var result = await App.DataBase.DeleteSchoolGradeAsync(item);

                        if (result == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_SaveFailed, AppResource.Common_OK);
                            CleanData();
                        }
                        else if (result > 0)
                        {
                            ltsGrade.Remove(ItemSelected);
                            CleanData();
                            Session.status = 1;
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.Common_RecordSaved, AppResource.Common_OK);
                            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                        }                       
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_OK);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }


        }

        private async void Tapped_For_DeleteCoursesLocal(object sender)
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    int result = 0;
                    if (Session._IdUser > 0)
                    {
                        foreach (var item in _ltsGrade)
                        {
                            var course = new SchoolGradeSQLite
                            {
                                course_name = item.course_name,
                                id_user = Session._IdUser,
                                grade = item.grade,
                                groups = item.groups
                            };
                            result = await App.DataBase.DeleteSchoolGradeAsync(course);                           
                        }

                        if (result == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_SaveFailed, AppResource.Common_OK);
                            CleanData();
                        }
                        else if (result > 0)
                        {
                            ltsGrade.Clear();
                            CleanData();
                            Session.status = 1;
                            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                        }

                    }                   
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_OK);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }


        }
        #endregion

    }
}
