using Attendance.Entities;
using Attendance.Helpers;
using Attendance.VM;

namespace Attendance.Pages;

public partial class StudentsList : ContentPage
{
	public StudentsList()
	{
		InitializeComponent();
		
        BindingContext = Session.attendanceView ? new VM.UsersVM(Session._IdUser,Session.Id_Course) : new VM.UsersVM(Session._IdUser);

        if ((!Session.attendanceView && !Session.schoolGrade && !Session.attendance) || Session.studentsListView)
        {
            btnEnter.IsVisible = false;
            if (Session.studentsListView)
            {
                Session.attendanceView = false;
                Session.studentsListView = false;
            }            
        }
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var viewModel = BindingContext as UsersVM;
            var selectedPerson = e.SelectedItem as Students;

            viewModel.SelectedPerson = selectedPerson;

            // Reiniciar la selección para permitir seleccionar el mismo elemento nuevamente
            //((ListView)sender).SelectedItem = null;
        }
    }
    
}