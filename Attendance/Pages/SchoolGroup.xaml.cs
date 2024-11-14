using Attendance.Entities;
using Attendance.Helpers;
using Attendance.VM;

namespace Attendance.Pages;

public partial class SchoolGroup : ContentPage
{
	public SchoolGroup()
	{
		InitializeComponent();
        //BindingContext = VM.SchoolGradeVM.GetInstance();
        
    }

    protected override void OnAppearing()
	{
		base.OnAppearing();
        BindingContext = new VM.SchoolGradeVM();
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCourse = e.SelectedItem as SchoolGrade;
            var viewModel = BindingContext as SchoolGradeVM;
            viewModel.ItemSelected = selectedCourse;            
        }
       
    }

    protected override void OnDisappearing()
    {
		base.OnDisappearing();
		ltsGrade = null;
    }
}