using page = Attendance.Pages;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.VM
{
    class MainMenuVM : ViewModelBase
    {
        public Command Tapped_For_Attendance_Command
        {
            get;
            set;
        }

        public MainMenuVM() 
        {
            InitVM();
        }

        private void InitVM()
        {
            Tapped_For_Attendance_Command = new Command(Tapped_For_Attendance);
        }

        private async void Tapped_For_Attendance(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new page.Attendance());
        }
    }
}
