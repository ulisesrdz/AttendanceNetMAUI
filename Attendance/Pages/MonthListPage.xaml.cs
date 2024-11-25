using Attendance.Entities;
using Attendance.Helpers;
using Attendance.VM;
using System.Collections.ObjectModel;

namespace Attendance.Pages;

public partial class MonthListPage : ContentPage
{
	public MonthListPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Session.monthlist = true;
        Session.attendanceView = false;
        BindingContext = new VM.AttendanceVM();
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCourse = e.SelectedItem as MonthItem;
            var viewModel = BindingContext as AttendanceVM;
            viewModel.SelectedMonth = selectedCourse;
        }

    }

    
}