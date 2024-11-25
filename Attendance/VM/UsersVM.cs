using Attendance.API;
using Attendance.Entities.Request;
using Attendance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Helpers;
using Attendance.Pages;
using System.Collections.ObjectModel;
using Attendance.Resources.Localization;

namespace Attendance.VM
{
    class UsersVM : ViewModelBase
    {
        AccountService _accountService;

        #region Private Properties
        string first_name;
        string last_name;
        int id_user;
        int id_course;
        int id_student;
        bool due_student;
        string _email;
        string _phoneNumber;
        #endregion

        #region Public Properties
        public string FirstName
        {
            get { return first_name; }
            set
            {
                first_name = value;
                OnPropertyChange();
            }
        }

        public bool Due_student
        {
            get { return due_student; }
            set
            {
                due_student = value;
                OnPropertyChange();
            }
        }

        public string LastName
        {
            get { return last_name; }
            set
            {
                last_name = value;
                OnPropertyChange();
            }
        }

        public int IDUser
        {
            get { return id_user; }
            set
            {
                id_user = value;
                OnPropertyChange();
            }
        }
        public int IDCourse
        {
            get { return id_course; }
            set
            {
                id_course = value;
                OnPropertyChange();
            }
        }

        public int IdStudent
        {
            get { return id_student; }
            set
            {
                id_student = value;
                OnPropertyChange();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChange();
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChange();
            }
        }

        private Students _selectedPerson { get; set; }
        public Students SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    OnPropertyChange();
                }

            }
        }

        private ObservableCollection<Students> _ltsStudents { get; set; }
        public ObservableCollection<Students> ltsStudents
        {
            get { return _ltsStudents; }
            set
            {
                if (_ltsStudents != value)
                {
                    _ltsStudents = value;
                    OnPropertyChange();
                }

            }
        }
        #endregion

        #region Commands
        public Command Tapped_For_SaveStudent_Command
        {
            get;
            set;
        }

        public Command Tapped_For_DeleteStudent_Command
        {
            get;
            set;
        }

        public Command Tapped_For_DeleteStudents_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Subject_Command
        {
            get;
            set;
        }

        public Command PersonSelectedCommand
        {
            get;
            set;
        }
        public Command Tapped_For_Enter_Command
        {
            get;
            set;
        }
        
        #endregion
        public UsersVM()
        {
            _accountService = new AccountService();
            InitVM();
            //Get_Students_InformationLocal();
            //Tapped_For_StudentList();
        }

        public UsersVM(int id)
        {
            _accountService = new AccountService();
            InitVM();
            //Get_Students_Information();
            Tapped_For_StudentListLocal(id);
        }
        public UsersVM(int id,int id_course)
        {
            _accountService = new AccountService();
            InitVM();
            //Get_Students_Information();
            Tapped_For_StudentListLocal(id,id_course);
        }
        private void InitVM()
        {
            Tapped_For_SaveStudent_Command = new Command(Tapped_For_SaveStudentLocal);
            Tapped_For_DeleteStudent_Command = new Command(Tapped_For_DeleteStudentLocal);
            Tapped_For_DeleteStudents_Command = new Command(Tapped_For_DeleteStudentsLocal); 
            Tapped_For_Subject_Command = new Command(Tapped_For_Subject);
            //PersonSelectedCommand = new Command(Tapped_PersonSelected);
            Tapped_For_Enter_Command = new Command(Tapped_For_EnterLocal);
            CleanData();
        }

        void CleanData()
        {
            FirstName = String.Empty;
            LastName = String.Empty;
            IDUser = 0;
            IDCourse = 0;
            IdStudent = 0;
            _ltsStudents = new ObservableCollection<Students>();
            ltsStudents = new ObservableCollection<Students>();
        }

        #region API
        private async void Tapped_For_EnterAPI(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (Session.schoolGrade)
                        {
                            var student = new Students
                            {
                                name = _selectedPerson.name,
                                last_name = _selectedPerson.last_name,
                                id_course = Session.Id_Course,
                                id_user = Session._IdUser,
                                phoneNumber = _selectedPerson.phoneNumber,
                                email = _selectedPerson.email
                            };

                            var result = await _accountService.UpdateStudents(student, _selectedPerson.id);

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
                                bool msg = await Application.Current.MainPage.DisplayAlert(AppResource.CommonWarning, AppResource.Student_Added, AppResource.CommonYes, AppResource.CommonNo);
                                if (!msg)
                                {
                                    await App.Current.MainPage.Navigation.PopAsync();
                                }
                                else
                                {
                                    IsBusy = false;
                                }
                                
                                
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                CleanData();
                                IsBusy = false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private async void Tapped_For_SaveStudentAPI(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (!string.IsNullOrWhiteSpace(first_name) && !string.IsNullOrWhiteSpace(last_name))
                        {
                            var student = new Students
                            {
                                name = first_name,
                                last_name = last_name,
                                id_course = 0,
                                id_user = Session._IdUser,
                                phoneNumber = _phoneNumber
                            };

                            var result = await _accountService.SaveStudents(student);

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
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
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

        private async void Tapped_For_DeleteStudentAPI(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (SelectedPerson != null)
                        {
                            var result = await _accountService.DeleteStudent(SelectedPerson.id);

                            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(result);
                            
                            if (jsonResult.status == "400")
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {

                                ltsStudents.Remove(SelectedPerson);

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
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

        private async void Tapped_For_DeleteStudentsAPI(object sender)
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
                            var result = await _accountService.DeleteStudents(Session._IdUser);

                            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(result);

                            if (jsonResult.status == "400")
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                CleanData();
                            }
                            else if (jsonResult.status == "200")
                            {

                                ltsStudents.Remove(SelectedPerson);

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                            }
                        }


                        // Connection to internet is available
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Error", "Es necesario tener una conexion a internet para continuar", "Aceptar");
                }
                
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsBusy = false;
            }


        }

        private async void Get_Students_InformationAPI()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    if (accessType == NetworkAccess.Internet)
                    {
                        var schoolGradeInfo = await _accountService.GetStudents();
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(schoolGradeInfo);
                        if (jsonResult.status == "400")
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                        }
                        else if (jsonResult.status == "500")
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                        }
                        else if (jsonResult.status == "200")
                        {
                            //var _obj = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Result);
                            _ltsStudents = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Students>>(jsonResult.Result.ToString());

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

        private async void Tapped_For_Subject(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new StudentsList());
            //await Shell.Current.GoToAsync("//CourseList");
        }

        private async void Tapped_For_StudentListAPI(int id_user)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {

                    if (accessType == NetworkAccess.Internet)
                    {
                        var studentInfo = await _accountService.GetStudents(id_user);
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(studentInfo);
                        if (jsonResult.status == "400")
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                        }
                        else if (jsonResult.status == "500")
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                        }
                        else if (jsonResult.status == "200")
                        {
                            //var _obj = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Result);
                            _ltsStudents = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Students>>(jsonResult.Result.ToString());

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
        #endregion
        
        #region Local

        private async void Tapped_For_StudentListLocal(int id_user)
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var _students = await App.DataBase.getUserbyIdUserAsync(id_user);
                    if (_students.Count > 0)
                    {
                        _ltsStudents.Clear();
                        foreach (var item in _students)
                        {
                            var stundent = new Students();
                            stundent.id = item.id;
                            stundent.id_user = item.id_user;
                            stundent.email = item.email;
                            stundent.phoneNumber = item.phoneNumber;
                            stundent.name = item.name;
                            stundent.last_name = item.last_name;

                            _ltsStudents.Add(stundent);
                        }
                       
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                        CleanData();                        
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                throw;
            }
        }

        private async void Tapped_For_StudentListLocal(int id_user, int id_course)                      
        {

            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var _students = await App.DataBase.getUserbyIdUserCourseAsync(id_user, id_course);
                    if (_students.Count > 0)
                    {
                        _ltsStudents.Clear();
                        foreach (var item in _students)
                        {
                            var stundent = new Students();
                            stundent.id = item.id;
                            stundent.id_user = item.id_user;
                            stundent.email = item.email;
                            stundent.phoneNumber = item.phoneNumber;
                            stundent.name = item.name;
                            stundent.last_name = item.last_name;

                            _ltsStudents.Add(stundent);
                        }                       
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                        CleanData();
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                throw;
            }
        }

        private async void Tapped_For_SaveStudentLocal(object sender)
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    
                    if (!string.IsNullOrWhiteSpace(first_name) && !string.IsNullOrWhiteSpace(last_name))
                    {
                        var student = new StudentsSQLite
                        {
                            name = first_name,
                            last_name = last_name,
                            id_course = 0,
                            id_user = Session._IdUser,
                            phoneNumber = _phoneNumber,
                            email=Email,
                            id = 0
                        };
                        var result = await App.DataBase.CreateStudentAsync(student);                       

                        if (result == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_SaveFailed, AppResource.Common_OK);
                            CleanData();
                        }
                        else if (result > 0)
                        {                            
                            CleanData();
                            Session.status = 1;
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.Common_RecordSaved, AppResource.Common_OK);
                            await App.Current.MainPage.Navigation.PopAsync();                           
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

        private async void Tapped_For_EnterLocal(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (Session.attendanceView)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new AttendanceView());
                    }
                    else if (Session.schoolGrade)
                    {
                        var student = new StudentsSQLite
                        {
                            name = _selectedPerson.name,
                            last_name = _selectedPerson.last_name,
                            id_course = Session.Id_Course,
                            id_user = Session._IdUser,
                            phoneNumber = _selectedPerson.phoneNumber,
                            email = _selectedPerson.email,
                            id = _selectedPerson.id
                        };

                        var exist = await App.DataBase.getStudentbyIdAsync(student.id, student.id_course);
                        if (exist)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.CommonWarning, AppResource.Student_Exists, AppResource.Common_OK);
                            IsBusy = false;
                            return;
                        }
                        var StudentCourse = new StudentInCourseSQLite
                        {
                            id_course = Session.Id_Course.ToString(),
                            id_student = _selectedPerson.id.ToString()
                        };
                        var result = await App.DataBase.AddCourseStudentAsync(StudentCourse);

                        if (result == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_SaveFailed, AppResource.Common_OK);
                            CleanData();
                        }
                        else if (result > 0)
                        {
                            CleanData();
                            Session.status = 1;
                            bool msg = await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.Student_Added, AppResource.CommonYes, AppResource.CommonNo);
                            if (!msg)
                            {
                                await App.Current.MainPage.Navigation.PopAsync();
                            }
                            else
                            {
                                IsBusy = false;
                            }
                        }
                    }                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void Tapped_For_DeleteStudentLocal(object sender)
        {           
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (SelectedPerson != null)
                    {
                        var student = new StudentsSQLite
                        {
                            name = _selectedPerson.name,
                            last_name = _selectedPerson.last_name,
                            id_course = _selectedPerson.id_course,
                            id_user = _selectedPerson.id_user,
                            phoneNumber = _selectedPerson.phoneNumber,
                            email = _selectedPerson.email,
                            id = _selectedPerson.id
                        };

                        var result = await App.DataBase.DeleteStudentAsync(student);

                        if (result == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_SaveFailed, AppResource.Common_OK);
                            CleanData();
                        }
                        else if (result > 0)
                        {
                            ltsStudents.Remove(SelectedPerson);
                            CleanData();
                            Session.status = 1;
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.Common_RecordSaved, AppResource.Common_OK);
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_Processing, AppResource.Common_Error);
                CleanData();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }


        }

        private async void Tapped_For_DeleteStudentsLocal(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (Session._IdUser > 0)
                    {
                        foreach (var item in _ltsStudents)
                        {
                            var student = new StudentsSQLite
                            {
                                name = item.name,
                                last_name = item.last_name,
                                id_course = item.id_course,
                                id_user = item.id_user,
                                phoneNumber = item.phoneNumber,
                                email = item.email,
                                id = item.id
                            };

                            var result = await App.DataBase.DeleteStudentAsync(student);

                            if (result == 0)
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_SaveFailed, AppResource.Common_OK);
                                CleanData();
                            }
                            else if (result > 0)
                            {
                                ltsStudents.Remove(item);
                                CleanData();
                                Session.status = 1;
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.Common_RecordSaved, AppResource.Common_OK);
                                await App.Current.MainPage.Navigation.PopAsync();
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

        private async void Get_Students_InformationLocal()
        {            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var _students = await App.DataBase.getStudentAsync();
                    if (_students.Count > 0)
                    {
                        _ltsStudents.Clear();
                        foreach (var item in _students)
                        {
                            var stundent = new Students();
                            stundent.id = item.id;
                            stundent.id_user = item.id_user;
                            stundent.email = item.email;
                            stundent.phoneNumber = item.phoneNumber;
                            stundent.name = item.name;
                            stundent.last_name = item.last_name;

                            _ltsStudents.Add(stundent);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                        CleanData();
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError,ex.Message), AppResource.Common_OK);
                throw;
            }
        }
        
        #endregion


    }
}
