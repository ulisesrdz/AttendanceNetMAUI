using Attendance.Entities;
using Attendance.Helpers;
using Attendance.VM;

namespace Attendance.Pages;

public partial class AttendanceView : ContentPage
{
	public AttendanceView()
	{
		InitializeComponent();
        BindingContext = new VM.AttendanceVM();
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCourse = e.SelectedItem as SchoolGrade;

            var viewModel = BindingContext as AttendanceVM;
            viewModel.ItemSelected = selectedCourse;
           
        }
    }
}