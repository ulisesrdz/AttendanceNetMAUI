using Attendance.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Pages;
using Attendance.Helpers;
using Attendance.Resources.Localization;

namespace Attendance.VM
{
    class CourseVM:ViewModelBase
    {
        #region Properties
        AccountService _accountService;
        int id_course;
        public int IdCourse
        {
            get { return id_course; }
            set
            {
                id_course = value;
                OnPropertyChange();
            }
        }
        #endregion
        #region Commands
        public Command Tapped_For_Add_Command
        {
            get;
            set;
        }

        public Command Tapped_For_Subject_Command
        {
            get;
            set;
        }

        public Command Tapped_For_Back_Command
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



        public CourseVM()
        {
            InitVM();
            _accountService = new AccountService();           
        }

        private void InitVM()
        {
            Tapped_For_Add_Command = new Command(Tapped_For_Add);
            Tapped_For_Subject_Command = new Command(Tapped_For_Subject);
            //Tapped_For_Back_Command = new Command(Tapped_For_Back);
            Tapped_For_DeleteCourse_Command = new Command(Tapped_For_DeleteCourse);
            Tapped_For_DeleteCourses_Command = new Command(Tapped_For_DeleteCourses);
            CleanData();
        }

        void CleanData()
        {

        }

        private async void Tapped_For_Add(object sender)
        {

        }

        private async void Tapped_For_Subject(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CourseList());
            //await Shell.Current.GoToAsync("//CourseList");
        }

        private async void Tapped_For_Back(object sender)
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
            //await Shell.Current.GoToAsync("//MainMenu");
        }

        private async void Tapped_For_DeleteCourse(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (id_course != 0)
                    {
                        var result = await _accountService.DeleteStudent(id_course);

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

                            //ltsStudents.Remove(SelectedPerson);

                            CleanData();
                            Session.status = 1;
                            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
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

    }
}
