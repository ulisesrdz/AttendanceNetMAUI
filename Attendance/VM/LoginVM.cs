
using Attendance.API;
using Attendance.Entities;
using Attendance.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.Media.Devices;

namespace Attendance.VM
{
    class LoginVM : ViewModelBase
    {
        #region Properties
        AccountService _accountService;
        string password;
        string username;

        public int id;
        public string identifier;
        public string name_user;
        public string email_user;
        public string pass;
        public string phone_number;        

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

        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChange();
            }
        }
        public string Identifier
        {
            get { return identifier; }
            set
            {
                identifier = value;
                OnPropertyChange();
            }
        }
        public string Name_user
        {
            get { return name_user; }
            set
            {
                name_user = value;
                OnPropertyChange();
            }
        }
        public string Email_user
        {
            get { return email_user; }
            set
            {
                email_user = value;
                OnPropertyChange();
            }
        }
        public string Phone_number
        {
            get { return phone_number; }
            set
            {
                phone_number = value;
                OnPropertyChange();
            }
        }

        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                OnPropertyChange();
            }
        }       
        #endregion

       #region Command
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
        public Command Tapped_For_SignIn_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Register_Command
        {
            get;
            set;
        }

        
        #endregion

        public LoginVM()
        {
            InitVM();
            _accountService = new AccountService();
        }

        void CleanData()
        {
            Username = String.Empty;
            Password = String.Empty;

            Identifier = String.Empty;
            Name_user = String.Empty;
            Email_user = String.Empty;
            Phone_number = String.Empty;
            Pass = String.Empty;
        }

        private void InitVM()
        {
            Tapped_For_Login_Command = new Command(Tapped_For_LoginLocal);
            Tapped_For_SignUp_Command = new Command(Tapped_For_SignUp);
            Tapped_For_Register_Command = new Command(Tapped_For_RegisterLocal);
            Tapped_For_SignIn_Command= new Command(Tapped_For_SignIn);
            CleanData();
        }

        private void HideKeyboard()
        {
            MessagingCenter.Send(this, "HideKeyboard");
        }
        
        #region Local
        private async void Tapped_For_RegisterLocal(object sender)
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (!string.IsNullOrWhiteSpace(name_user) && !string.IsNullOrWhiteSpace(email_user))
                    {
                        var exist = await App.DataBase.getUserbyUserAsync(email_user);
                        if (exist != null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "The email is already created", "Ok");
                        }
                        else
                        {
                            var _user = new UserSQLite
                            {
                                name_user = name_user,
                                email_user = email_user,
                                identifier = name_user + "," + email_user,
                                password = pass,
                                phone_number = phone_number
                            };

                            var result = await App.DataBase.CreateUserAsync(_user);
                            if (result > 0)
                            {
                                await Application.Current.MainPage.DisplayAlert("Guardado", "Usuario guardado en la base de datos", "OK");
                                //OnCargarUsuariosClicked(null, null);
                                CleanData();
                                await App.Current.MainPage.Navigation.PopModalAsync();
                                //await App.Current.MainPage.Navigation.PushModalAsync(new Pages.LoginA());
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrio un error", "Aceptar");
                            }
                        }
                        
                       
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed", "Please add the required information", "OK");
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

        private async void OnCargarUsuariosClicked(object sender, EventArgs e)
        {
            var _users = await App.DataBase.getUsersAsync();
            foreach (var user in _users)
            {
                Console.WriteLine($"Usuario: {user.name_user}, Email: {user.email_user}");
            }
        }

        private async void Tapped_For_LoginLocal(object sender)
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
                            
                            var _users = await App.DataBase.loginAsync(Username, Password);
                            if (_users != null)
                            {
                                Session._IdUser = _users.id;
                                Session._name = _users.name_user;
                                Session._email = _users.email_user;
                                Session._identifier = _users.identifier;
                                Session.phone_number = _users.phone_number;
                                Session.status = 1;

                                CleanData();
                                Application.Current.MainPage = new NavigationPage(new Pages.MainMenu());
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                            }
                            else
                            {
                                HideKeyboard();
                                await Application.Current.MainPage.DisplayAlert("Error", "Username or password invalid", "Ok");
                                //CleanData();
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

        #endregion

        #region API

        private async void Tapped_For_LoginAPI(object sender)
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
        private async void Tapped_For_SignUp(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Pages.Register());
        }

        private async void Tapped_For_SignIn(object sender)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        #endregion




    }
}
