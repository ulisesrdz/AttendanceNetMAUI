﻿using page = Attendance.Pages;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Helpers;
using Attendance.Resources.Localization;

namespace Attendance.VM
{
    class MainMenuVM : ViewModelBase
    {
        #region Properties
        #endregion

        #region Commands
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

        public Command Tapped_For_BackupDatabase_Command
        {
            get;
            set;
        }

        public Command Tapped_For_UserGuide_Command
        {
            get;
            set;
        }
        #endregion


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
            Tapped_For_BackupDatabase_Command = new Command(Tapped_For_BackupDatabase);
            Tapped_For_UserGuide_Command = new Command(Tapped_For_UserGuide);
        }

        private async void Tapped_For_UserGuide()
        {
            string url = "https://www.google.com";
            await Launcher.OpenAsync(url);
            //if (await Launcher.CanOpenAsync(url))
            //{
            //    await Launcher.OpenAsync(url);
            //}
        }

        private async void Tapped_For_BackupDatabase()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Attendance.db3");
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = AppResource.PrintDoc_ShareFile,
                File = new ShareFile(dbPath)
            });
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
            await Application.Current.MainPage.Navigation.PushAsync(new page.SchoolGroup());
            //await Application.Current.MainPage.Navigation.PushModalAsync(new page.SchoolGroup());
            //await Application.Current.MainPage.Navigation.PushModalAsync(new page.AttendanceView());
        }
        private async void Tapped_For_BusinessURL(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushAsync(new page.BusinessURL());
        }
        private async void Tapped_For_SchoolGroup(object sender)
        {
            Session.schoolGrade = true;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushAsync(new page.SchoolGroup());
        }

        private async void Tapped_For_Register(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushAsync(new page.StudentRegister());
        }
        private async void Tapped_For_CourseRegisterCourseList(object sender)
        {
            Session.schoolGrade = false;
            Session.attendanceView = false;
            Session.attendance = false;
            Session.studentsListView = false;
            await Application.Current.MainPage.Navigation.PushAsync(new page.CourseRegister());
        }
        private async void Tapped_For_PrintQRCode(object sender)
        {
            try
            {
                Session.schoolGrade = false;
                Session.attendanceView = false;
                Session.attendance = false;
                Session.attendance = false;
                await Application.Current.MainPage.Navigation.PushAsync(new page.PrintQRCodePage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                throw;
            }
            
        }
        
    }
}
