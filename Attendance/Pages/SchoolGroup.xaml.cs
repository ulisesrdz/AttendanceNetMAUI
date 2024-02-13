namespace Attendance.Pages;

public partial class SchoolGroup : ContentPage
{
	public SchoolGroup()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
	{
		base.OnAppearing();
		if (ltsGrade == null)
		{
            Get_Information();
        }
    }

	private void Get_Information()
	{
		VM.SchoolGradeVM obj = new VM.SchoolGradeVM();
		obj.getInfo();

    }

    protected override void OnDisappearing()
    {
		base.OnDisappearing();
		ltsGrade = null;
    }
}