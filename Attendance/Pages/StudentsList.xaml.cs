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
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var viewModel = BindingContext as UsersVM;
            var selectedPerson = e.SelectedItem as Students;

            viewModel.SelectedPerson = selectedPerson;

            // Reiniciar la selecci�n para permitir seleccionar el mismo elemento nuevamente
            //((ListView)sender).SelectedItem = null;
        }
    }
}