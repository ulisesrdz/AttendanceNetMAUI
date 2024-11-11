namespace Attendance.Pages;

public partial class CourseRegister : ContentPage
{
	public CourseRegister()
	{
		InitializeComponent();

        BindingContext = new VM.SchoolGradeVM();
    }
   
}