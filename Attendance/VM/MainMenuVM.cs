using page = Attendance.Pages;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Helpers;

namespace Attendance.VM
{
    class MainMenuVM : ViewModelBase
    {
        public Command Tapped_For_Attendance_Command
        {
            get;
            set;
        }
        public Command Tapped_For_AttendanceView_Command
        {
            get;
            set;
        }
        public Command Tapped_For_BusinessURL_Command
        {
            get;
            set;
        }

        public Command Tapped_For_SchoolGroup_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Register_Command
        {
            get;
            set;
        }
        public Command Tapped_For_CourseRegisterCourseList_Command
        {
            get;
            set;
        }

        public Command Tapped_For_PrintQRCode_Command
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
            Tapped_For_BusinessURL_Command = new Command(Tapped_For_BusinessURL);
            Tapped_For_SchoolGroup_Command = new Command(Tapped_For_SchoolGroup);
            Tapped_For_Register_Command = new Command(Tapped_For_Register);
            Tapped_For_CourseRegisterCourseList_Command = new Command(Tapped_For_CourseRegisterCourseList);
            Tapped_For_PrintQRCode_Command = new Command(Tapped_For_PrintQRCode);
            Tapped_For_AttendanceView_Command = new Command(Tapped_For_AttendanceList);
        }

        private async void Tapped_For_Attendance(object sender)
        {
            try
            {
                Session.schoolGrade = false;
                Session.attendanceView = false;
                Session.attendance = true;
                Session.studentsListView = false;
                await Application.Current.MainPage.Navigation.PushAsync(new page.SchoolGroup());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
                throw;
            }
            
            //await Application.Current.MainPage.Navigation.PushAsync(new page.Attendance());
        }
        private async void Tapped_For_AttendanceList(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = true;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new page.SchoolGroup());
            //await Application.Current.MainPage.Navigation.PushModalAsync(new page.SchoolGroup());
            //await Application.Current.MainPage.Navigation.PushModalAsync(new page.AttendanceView());
        }
        private async void Tapped_For_BusinessURL(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new page.BusinessURL());
        }
        private async void Tapped_For_SchoolGroup(object sender)
        {
            Session.schoolGrade = true;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new page.SchoolGroup());
        }

        private async void Tapped_For_Register(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new page.StudentRegister());
        }
        private async void Tapped_For_CourseRegisterCourseList(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new page.CourseRegister());
        }
        private async void Tapped_For_PrintQRCode(object sender)
        {
            try
            {
                Session.schoolGrade = false;
                Session.attendanceView = false;
                Session.attendance = false;
                Session.attendance = false;
                await Application.Current.MainPage.Navigation.PushModalAsync(new page.PrintQRCodePage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                throw;
            }
            
        }
        
    }
}
