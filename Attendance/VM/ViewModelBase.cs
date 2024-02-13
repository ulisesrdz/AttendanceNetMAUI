using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Helpers;

namespace Attendance.VM
{
    class ViewModelBase : ObservableObject
    {
        //protected INavigation navigation;

        //public ViewModelBase(INavigation navigation)
        //{
        //    this.navigation = navigation;
        //}

        public ViewModelBase()
        {

        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChange();
            }
        }
    }
}
