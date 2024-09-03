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

namespace Attendance.VM
{
    public class ItemViewModel
    {
        public string Name { get; set; }
        // Otros propiedades según tus necesidades
    }

    class SchoolGradeVM : ViewModelBase
    {
        AccountService _accountService;
        //SchoolGrade ltsGrade;

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

        private void InitVM()
        {
            Tapped_For_Enter_Command = new Command(Tapped_For_Enter);
            Tapped_For_Add_Command = new Command(Tapped_For_Add);
            Tapped_For_DeleteCourse_Command = new Command(Tapped_For_DeleteCourse);
            Tapped_For_DeleteCourses_Command = new Command(Tapped_For_DeleteCourses);
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
                                await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Continuar");
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {

                                string jsonObj = jsonResult.Result.ToString();

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                            }
                        }


                        // Connection to internet is available
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Es necesario tener una conexion a internet para continuar", "Aceptar");
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
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
                    if (accessType == NetworkAccess.Internet)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                        //await Application.Current.MainPage.Navigation.PushAsync(new Attendance());
                        //var aaa = ItemSelected;
                        //var dataInfoStatus = await _accountService.GetSchoolGradeInfo(Session._IdUser);
                    }
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
                                await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Continuar");
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {

                                //ltsStudents.Remove(SelectedPerson);

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                            }
                        }


                        // Connection to internet is available
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Es necesario tener una conexion a internet para continuar", "Aceptar");
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
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
                                await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Continuar");
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {

                                //ltsStudents.Remove(SelectedPerson);

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                            }
                        }


                        // Connection to internet is available
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Es necesario tener una conexion a internet para continuar", "Aceptar");
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsBusy = false;
            }


        }

        private async void Tapped_For_Subject(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CourseList());            
        }
        #endregion
        #region Local
        private async void Get_InformationLocal()
        {
            try
            {
                if (!IsBusy)
                {

                    var schoolGradeInfo = await App.DataBase.getSchoolGradebyIdUserAsync(Session._IdUser);

                    if (schoolGradeInfo.Count > 0)
                    {
                        var _grade = new List<SchoolGrade>();
                        foreach (var item in _grade)
                        {
                            var grade = new SchoolGrade();
                            grade.id = item.id;
                            grade.id_user = item.id_user;
                            grade.course_name = item.course_name;
                            grade.groups = item.groups;
                            grade.grade = item.grade;


                            _grade.Add(grade);
                        }

                        if (_grade.Count > 0)
                        {
                            ltsGrade = _grade;
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

        private async void Tapped_For_AddLocal(object sender)
        {            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    
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


                        //var result = await _accountService.AddCourse(course);

                        //var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(result);

                        //if (jsonResult.status == "400")
                        //{
                        //    await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Continuar");
                        //    CleanData();
                        //}
                        //else if (jsonResult.status == "200")
                        //{

                        //    string jsonObj = jsonResult.Result.ToString();

                        //    CleanData();
                        //    Session.status = 1;
                        //    await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                        //    //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                        //    //await Shell.Current.GoToAsync("//MainMenu");
                        //}
                        //else
                        //{
                        //    await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                        //}
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Es necesario tener una conexion a internet para continuar", "Aceptar");
                CleanData();
                IsBusy = false;

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
