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
    class LoginVM : ViewModelBase
    {
        AccountService _accountService;
        string password;
        string username;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChange();
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChange();
            }
        }

        public Command Tapped_For_Login_Command
        {
            get;
            set;
        }

        public Command Tapped_For_SignUp_Command
        {
            get;
            set;
        }
        public LoginVM()
        {
            InitVM();
            _accountService = new AccountService();
        }

        void CleanData()
        {
            Username = String.Empty;
            Password = String.Empty;
        }

        private void InitVM()
        {
            Tapped_For_Login_Command = new Command(Tapped_For_Login);
            Tapped_For_SignUp_Command = new Command(Tapped_For_SignUp);
            CleanData();
        }
        private async void Tapped_For_SignUp(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Pages.Register());
        }
        private async void Tapped_For_Login(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
                        {
                            var loginStatus = await _accountService.LoginUser(Username, Password);
                            var jsonLogin = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(loginStatus);
                            if (jsonLogin.status == "400")
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", jsonLogin.Result.ToString(), "Continuar");
                                CleanData();
                            }
                            else if (jsonLogin.status == "200")
                            {

                                string jsonObj = jsonLogin.Result.ToString();
                                var _user = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Users>>(jsonObj);
                                Session._IdUser = _user[0].id;
                                Session._name = _user[0].user_name;
                                Session._email = _user[0].email_user;
                                Session._token = _user[0].Token_user;
                                Session._secretKey = _user[0].client_secret;

                                CleanData();
                                Session.status = 1;
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                                //Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                //await Shell.Current.GoToAsync("//MainMenu");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", jsonLogin.Result.ToString(), "Aceptar");
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
    }
}
