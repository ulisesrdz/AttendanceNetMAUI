﻿using page = Attendance.Pages;
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
        }

        private async void Tapped_For_Attendance(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new page.Attendance());
        }

        private async void Tapped_For_BusinessURL(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new page.BusinessURL());
        }
        private async void Tapped_For_SchoolGroup(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new page.SchoolGroup());
        }

        private async void Tapped_For_Register(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new page.StudentRegister());
        }
        private async void Tapped_For_CourseRegisterCourseList(object sender)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new page.CourseRegister());
        }
    }
}
