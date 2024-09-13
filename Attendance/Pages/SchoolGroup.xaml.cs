using Attendance.Entities;
using Attendance.Helpers;
using Attendance.VM;

namespace Attendance.Pages;

public partial class SchoolGroup : ContentPage
{
	public SchoolGroup()
	{
		InitializeComponent();

        //BindingContext = Session.schoolGrade ? new VM.SchoolGradeVM() : new VM.AttendanceVM();
        BindingContext = new VM.SchoolGradeVM();
        //Get_Information();
    }

    protected override void OnAppearing()
	{
		base.OnAppearing();
		//if (ltsGrade == null)
		//{
  //          Get_Information();
  //      }
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCourse = e.SelectedItem as SchoolGrade;
            var viewModel = BindingContext as SchoolGradeVM;
            viewModel.ItemSelected = selectedCourse;
            //if (Session.schoolGrade)
            //{
            //    var viewModel = BindingContext as SchoolGradeVM;
            //    viewModel.ItemSelected = selectedCourse;
            //}
            //else
            //{
            //    var viewModel = BindingContext as  AttendanceVM;
            //    viewModel.ItemSelected = selectedCourse;
            //}
            
            // Reiniciar la selección para permitir seleccionar el mismo elemento nuevamente
            //((ListView)sender).SelectedItem = null;
        }
    }

    protected override void OnDisappearing()
    {
		base.OnDisappearing();
		ltsGrade = null;
    }
}