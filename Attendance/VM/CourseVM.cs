using Attendance.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Tapped_For_Back_Command = new Command(Tapped_For_Back);
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
            await Shell.Current.GoToAsync("//CourseList");
        }

        private async void Tapped_For_Back(object sender)
        {
            await Shell.Current.GoToAsync("//MainMenu");
        }
        
    }
}
