using Attendance.Entities;
using Attendance.VM;
using Attendance.Helpers;

namespace Attendance.Pages;

public partial class CourseList : ContentPage
{
	public CourseList()
	{
		InitializeComponent();
        Session.studentsListView = true;
        BindingContext = new VM.SchoolGradeVM();
        //Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        //BindingContext = new VM.CourseVM();
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var viewModel = BindingContext as SchoolGradeVM;
            var selectedCourse = e.SelectedItem as SchoolGrade;

            viewModel.ItemSelected = selectedCourse;

            // Reiniciar la selección para permitir seleccionar el mismo elemento nuevamente
            //((ListView)sender).SelectedItem = null;
        }
    }
}