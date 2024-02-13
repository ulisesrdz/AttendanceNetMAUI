using Attendance.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Pages;

namespace Attendance.VM
{
    class CourseVM:ViewModelBase
    {
        AccountService _accountService;
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
            await App.Current.MainPage.Navigation.PushModalAsync(new MainPage());
            //await Shell.Current.GoToAsync("//MainMenu");
        }
        
    }
}
