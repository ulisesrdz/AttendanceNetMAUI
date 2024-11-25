using Attendance.API;
using Attendance.Entities;
using Attendance.Helpers;
using Attendance.Resources.Localization;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using page = Attendance.Pages;

namespace Attendance.VM
{
    class AttendanceVM : ViewModelBase
    {
        #region Properties
        List<AttendanceEnt> _lts;
        AccountService _accountService;
        public List<AttendanceEnt> _LtsAttendace
        {
            get { return _lts; }
            set
            {
                _lts = value;
                OnPropertyChange();
            }
        }
        private int _id_course;
        public int Id_Course
        {
            get { return _id_course; }
            set
            {
                _id_course = value;
                OnPropertyChange();
            }
        }

        private ObservableCollection<SchoolGrade> _ltsGrade { get; set; }
        public ObservableCollection<SchoolGrade> ltsGrade
        {
            get { return _ltsGrade; }
            set
            {
                if (_ltsGrade != value)
                {
                    _ltsGrade = value;
                    OnPropertyChange();
                }

            }
        }

        private ObservableCollection<AttendanceEnt> _ltsAttendance { get; set; }
        public ObservableCollection<AttendanceEnt> ltsAttendance
        {
            get { return _ltsAttendance; }
            set
            {
                if (_ltsAttendance != value)
                {
                    _ltsAttendance = value;
                    OnPropertyChange();
                }

            }
        }

        private SchoolGrade _onItemSelected;

        public SchoolGrade ItemSelected
        {
            get { return _onItemSelected; }
            set
            {
                if (_onItemSelected != value)
                {
                    _onItemSelected = value;
                    OnPropertyChange("ItemSelected");
                }
            }
        }

        private MonthItem _selectedMonth;
        public MonthItem SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));

                }
            }
        }

        private ObservableCollection<MonthItem> _months { get; set; }
        public ObservableCollection<MonthItem> Months
        {
            get { return _months; }
            set
            {
                if (_months != value)
                {
                    _months = value;
                    OnPropertyChange();
                }

            }
        }
        #endregion

        #region Commands
        public Command Tapped_Save_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Enter_Command
        {
            get;
            set;
        }
        public Command Tapped_For_Enter_User_Command
        {
            get;
            set;
        }

        public Command Tapped_For_Print_Command
        {
            get;
            set;
        }
        #endregion        

        public AttendanceVM()
        {
            InitVM();
            _accountService = new AccountService();
            //Get_Information();
            if (Session.attendanceView)
            {
                Get_InformationLocal();
            }
            if (Session.monthlist)
            {
                Get_MonthList();
                //Session.monthlist = false;
            }
           
        }

        private void InitVM()
        {
            Tapped_Save_Command = new Command(Tapped_For_BusinessLocal);
            Tapped_For_Enter_Command = new Command(Tapped_For_Enter);
            Tapped_For_Enter_User_Command = new Command(Tapped_For_Enter_Users);
            Tapped_For_Print_Command = new Command(Tapped_For_Print);
            CleanData();
        }

        private void CleanData()
        {
            _ltsAttendance = new ObservableCollection<AttendanceEnt>();
            _lts = new List<AttendanceEnt>();
            _ltsGrade = new ObservableCollection<SchoolGrade>();
            ltsGrade = new ObservableCollection<SchoolGrade>();
            Months = new ObservableCollection<MonthItem> { new MonthItem() };
        }

        #region API
        private async void Tapped_For_Enter(object sender)
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy=true;
                    if (Session.monthlist)
                    {
                        Session.monthlist = false;
                        Session.attendanceView = true;
                        Session.monthValue = SelectedMonth.Value;
                        await App.Current.MainPage.Navigation.PushAsync(new page.AttendanceView());
                    }
                    else if (Session.attendanceView)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                    }
                    else
                    {
                        //await App.Current.MainPage.Navigation.PushAsync(new page.Attendance());]\
                        await App.Current.MainPage.Navigation.PushAsync(App.Services.GetService<page.Attendance>());
                    }
                    
                    IsBusy=false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                throw;
            }
        }

        private async void Tapped_For_Enter_Users(object sender)
        {

            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                throw;
            }
        }

        private async void Get_Information()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    if (accessType == NetworkAccess.Internet)
                    {
                        var schoolGradeInfo = await _accountService.GetSchoolGradeInfo(Session._IdUser);
                        var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(schoolGradeInfo);
                        if (jsonResult.status == "400")
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                        }
                        else if (jsonResult.status == "500")
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_Error);
                        }
                        else if (jsonResult.status == "200")
                        {
                            //var _obj = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Result);
                            _ltsGrade = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<SchoolGrade>>(jsonResult.Result.ToString());

                            //_ltsGrade
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                throw;
            }
        }

        private async void Tapped_For_Business(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (accessType == NetworkAccess.Internet)
                    {
                        if (_lts.Count > 0)
                        {
                            foreach (var item in _lts)
                            {
                                var attendanceStatus = await _accountService.SaveAttendanceData(item);
                                var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiRequest>(attendanceStatus);
                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    if (jsonResult.status == "400")
                                    {
                                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                    }
                                    else if (jsonResult.status == "500")
                                    {
                                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, jsonResult.Result.ToString(), AppResource.Common_OK);
                                    }
                                    else if (jsonResult.status == "200")
                                    {
                                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, jsonResult.Result.ToString(), AppResource.Common_OK);
                                    }
                                });
                            }
                            

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                        
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);                
                IsBusy = false;
            }
        }

        #endregion

        #region Local

        private void Get_MonthList()
        {
            Months = new ObservableCollection<MonthItem>
            {
                new MonthItem { Name = AppResource.Month_Jan, Value = 1 },
                new MonthItem { Name = AppResource.Month_Feb, Value = 2 },
                new MonthItem { Name = AppResource.Month_Mar, Value = 3 },
                new MonthItem { Name = AppResource.Month_Apr, Value = 4 },
                new MonthItem { Name = AppResource.Month_May, Value = 5 },
                new MonthItem { Name = AppResource.Month_Jun, Value = 6 },
                new MonthItem { Name = AppResource.Month_Jul, Value = 7 },
                new MonthItem { Name = AppResource.Month_Aug, Value = 8 },
                new MonthItem { Name = AppResource.Month_Sep, Value = 9 },
                new MonthItem { Name = AppResource.Month_Oct, Value = 10 },
                new MonthItem { Name = AppResource.Month_Nov, Value = 11 },
                new MonthItem { Name = AppResource.Month_Dec, Value = 12 }
            };
        }
        private async void Get_InformationLocal()
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    int counter = 0;
                    
                    var attenaces = await App.DataBase.getAttendacebyIdUserAsync(Session._IdUser.ToString(), Session.Id_Course.ToString(), Session.monthValue);
                    
                    if (attenaces.Count > 0)                       
                    {
                        _ltsAttendance.Clear();

                        foreach (var item in attenaces)
                        {
                            var attendace = new AttendanceEnt();
                            var id_student = Convert.ToInt32(item.id_student);
                            var student = await App.DataBase.getStudentbyIdAsync(id_student);

                            attendace.counter = counter++;
                            attendace.id_course = item.id_course;
                            attendace.id_student = item.id_student;
                            attendace.student_name = student.name + " " + student.last_name;
                            attendace.date_time = item.date_time;

                            _ltsAttendance.Add(attendace);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
                throw;
            }
        }

        private async void Tapped_For_BusinessLocal(object sender)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (_lts.Count > 0)
                    {
                        foreach (var item in _lts)
                        {
                            var id_student = Convert.ToInt32(item.id_student);
                            var id_course = Convert.ToInt32(item.id_course);
                            var NotExists= await App.DataBase.getStudentbyIdAsync(id_student, id_course);
                            if (!NotExists)
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.CommonWarning, string.Format(AppResource.Attendance_NotRegistered, item.student_name), AppResource.Common_OK);
                                IsBusy = false; 
                                return;
                            }

                            var exists = await App.DataBase.getAttendanceRegistered(item.id_student, item.id_course);
                            if (exists)
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.CommonWarning, string.Format(AppResource.Attendance_AlreadyRegistered, item.student_name), AppResource.Common_OK);
                                IsBusy = false;
                                return;
                            }

                            var attendance = new AttendanceEntSQLite
                            {
                                id_course = item.id_course,
                                date_time = DateTime.Now,
                                id_student = item.id_student,
                                id_user = item.id_user

                            };



                            var result = await App.DataBase.CreateAttendaceAsync(attendance);
                            if (result > 0)
                            {
                                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, string.Format(AppResource.Attendance_Save, item.student_name), AppResource.Common_OK);
                                CleanData();
                            }
                            
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.Common_InfoNotFound, AppResource.Common_OK);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, string.Format(AppResource.Common_InternalError, ex.Message), AppResource.Common_OK);
                IsBusy = false;
            }
        }

        private async void Tapped_For_Print()
        {

            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    IWorkbook workbook = new XSSFWorkbook();                   
                    ISheet sheet = workbook.CreateSheet("Sheet1");
                    
                    IRow row;
                    ICell cell;

                    var id_user = Convert.ToInt32(Session._IdUser);
                    var id_course = Convert.ToInt32(Session.Id_Course);                    
                    var _lts = await App.DataBase.getUserbyIdUserCourseAsync(id_user, id_course);

                    // Header Style
                    ICellStyle headerStyle = workbook.CreateCellStyle();
                    headerStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
                    headerStyle.FillPattern = FillPattern.SolidForeground;
                    headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    headerStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    // Header Title
                    row = sheet.CreateRow(0);
                    row.CreateCell(3).SetCellValue(AppResource.PrintDoc_Title);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 3, 25));
                    row.GetCell(3).CellStyle = headerStyle;

                    //Border Color
                    XSSFColor borderColor = new XSSFColor(new byte[] { 0, 0, 0 });
                    //Number Column
                    ICellStyle itemStyle = workbook.CreateCellStyle();
                    itemStyle.FillForegroundColor = IndexedColors.LightCornflowerBlue.Index;
                    itemStyle.FillPattern = FillPattern.SolidForeground;
                    itemStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    itemStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    itemStyle.Rotation = 90;
                    itemStyle.BorderTop = BorderStyle.Thin;
                    itemStyle.BorderLeft = BorderStyle.Thin;
                    itemStyle.BorderBottom = BorderStyle.Thin;
                    itemStyle.BorderRight = BorderStyle.Thin;
                    itemStyle.WrapText = true;

                    ((XSSFCellStyle)itemStyle).SetTopBorderColor(borderColor);
                    ((XSSFCellStyle)itemStyle).SetBottomBorderColor(borderColor);
                    ((XSSFCellStyle)itemStyle).SetLeftBorderColor(borderColor);
                    ((XSSFCellStyle)itemStyle).SetRightBorderColor(borderColor);

                    //Name column
                    ICellStyle nameStyle = workbook.CreateCellStyle();
                    nameStyle.FillForegroundColor = IndexedColors.BlueGrey.Index;
                    nameStyle.FillPattern = FillPattern.SolidForeground;
                    nameStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    nameStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    nameStyle.BorderTop = BorderStyle.Thin;
                    nameStyle.BorderLeft = BorderStyle.Thin;
                    nameStyle.BorderBottom = BorderStyle.Thin;
                    nameStyle.BorderRight = BorderStyle.Thin;
                    nameStyle.WrapText = true;
                    
                    //Details
                    ICellStyle detailStyle = workbook.CreateCellStyle();
                    //detailStyle.FillForegroundColor = IndexedColors.BlueGrey.Index;
                    detailStyle.FillPattern = FillPattern.NoFill;
                    detailStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    detailStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    detailStyle.BorderTop = BorderStyle.Thin;
                    detailStyle.BorderLeft = BorderStyle.Thin;
                    detailStyle.BorderBottom = BorderStyle.Thin;
                    detailStyle.BorderRight = BorderStyle.Thin;
                    detailStyle.WrapText = true;
                    
                    //Details percentage
                    ICellStyle percentageStyle = workbook.CreateCellStyle();
                    IDataFormat dataFormat = workbook.CreateDataFormat();
                    //detailStyle.FillForegroundColor = IndexedColors.BlueGrey.Index;
                    percentageStyle.FillPattern = FillPattern.NoFill;
                    percentageStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    percentageStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    percentageStyle.DataFormat = dataFormat.GetFormat("0.00%");
                    percentageStyle.BorderTop = BorderStyle.Thin;
                    percentageStyle.BorderLeft = BorderStyle.Thin;
                    percentageStyle.BorderBottom = BorderStyle.Thin;
                    percentageStyle.BorderRight = BorderStyle.Thin;
                    percentageStyle.WrapText = true;

                    ((XSSFCellStyle)nameStyle).SetTopBorderColor(borderColor);
                    ((XSSFCellStyle)nameStyle).SetBottomBorderColor(borderColor);
                    ((XSSFCellStyle)nameStyle).SetLeftBorderColor(borderColor);
                    ((XSSFCellStyle)nameStyle).SetRightBorderColor(borderColor);
                   
                   
                    row = sheet.CreateRow(2);
                    ICell itemMergedCell = row.CreateCell(0);
                    ICell nameMergedCell = row.CreateCell(1);
                    ICell monthMergedCell = row.CreateCell(2);
                   
                    row.CreateCell(0).SetCellValue("No.");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 5, 0, 0));
                    row.CreateCell(1).SetCellValue(AppResource.PrintDoc_Name);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 5, 1, 1));
                    row.CreateCell(2).SetCellValue($"{DateTime.Now.ToString("MMMM",new CultureInfo("es-ES")).ToUpper()}");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 2, 25));
                    
                    nameMergedCell.CellStyle = nameStyle;
                    itemMergedCell.CellStyle = itemStyle;
                    monthMergedCell.CellStyle = nameStyle;
                   
                    sheet.SetColumnWidth(0, 4 * 256);
                    sheet.SetColumnWidth(1, 30 * 256);

                    row = sheet.CreateRow(3);
                    ICell itemMerged = row.CreateCell(2);
                    row.CreateCell(2).SetCellValue(AppResource.PrintDoc_Course);
                    itemMerged.CellStyle = nameStyle;
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 2, 25));
                    
                    IRow row4 = sheet.CreateRow(4);                    
                    IRow row5 = sheet.CreateRow(5);
                    
                    var _listMoth = getDayofWeek(DateTime.Now.Year, DateTime.Now.Month);
                    var index = 2;

                    foreach (var (day,no) in _listMoth)
                    {
                        ICell DayCell = row4.CreateCell(index);
                        row4.CreateCell(index).SetCellValue(day);
                        DayCell.CellStyle = detailStyle;

                        ICell DayNoCell = row5.CreateCell(index);
                        row5.CreateCell(index).SetCellValue(no);
                        DayNoCell.CellStyle = detailStyle;                       

                        sheet.SetColumnWidth(index, 4 * 256);
                        sheet.SetColumnWidth(index, 4 * 256);
                        
                        index++;
                    }

                    index = 1;
                   
                    foreach (var item in _lts)
                    {
                        row = sheet.CreateRow(index + 5);                       
                        cell = row.CreateCell(0);
                        row.CreateCell(0).SetCellValue($"{index}");
                        cell.CellStyle = detailStyle;
                        
                        cell = row.CreateCell(1);
                        row.CreateCell(1).SetCellValue($"{item.name} {item.last_name}");
                        cell.CellStyle = detailStyle;
                        
                        var att = await App.DataBase.getAttendacebyStudentAsync(item.id_user.ToString(), item.id_course.ToString(), item.id.ToString());
                        
                        for (int i = 0; i < _listMoth.Count; i++)
                        {
                            var dayNumber = _listMoth[i].Item2;

                            cell = row.CreateCell(2 + i);                           

                            bool hasAttendance = att.Any(a => a.date_time.Day == dayNumber);

                            cell.SetCellValue(hasAttendance ? "✓" : "X");
                            cell.CellStyle = detailStyle;
                        }

                        int lastDayColumnIndex = 2 + _listMoth.Count - 1;
                        cell = row.CreateCell(lastDayColumnIndex + 1);
                        cell.CellFormula = $"COUNTIF({GetColumnLetter(2)}{index + 6}:{GetColumnLetter(lastDayColumnIndex)}{index + 6}, \"✓\")";
                        cell.CellStyle = detailStyle;

                        // Columna para porcentaje de asistencia
                        cell = row.CreateCell(lastDayColumnIndex + 2);
                        cell.CellFormula = $"{GetColumnLetter(lastDayColumnIndex + 1)}{index + 6}/{_listMoth.Count}";                        
                        cell.CellStyle = percentageStyle;

                        index++;
                    }

                    // Guardar el archivo Excel
                    string filePath = Path.Combine(FileSystem.AppDataDirectory, "attendance.xlsx");
#if ANDROID
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(fileStream);

                            bool msg = await Application.Current.MainPage.DisplayAlert("Aviso", "Se genero archivo correctamente, Desea compartirlo?", "Si", "No");
                            
                            
                            if (msg)
                            {
                                await Share.RequestAsync(new ShareFileRequest
                                {
                                    Title = "Compartir Archivo",
                                    File = new ShareFile(filePath)
                                });
                            }
                            else
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                            }
                        }
#else
                    if (IsFileLocked(filePath))
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, AppResource.PrintDoc_DocOpen, AppResource.Common_OK);
                        IsBusy = false;
                        return;
                    }
                    else
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(fileStream);

                            bool msg = await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, AppResource.PrintDoc_Saved, AppResource.CommonYes, AppResource.CommonNo);


                            if (msg)
                            {
                                await Share.RequestAsync(new ShareFileRequest
                                {
                                    Title = AppResource.PrintDoc_ShareFile,
                                    File = new ShareFile(filePath)
                                });
                            }
                            else
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                            }
                        }
                    }
#endif


                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResource.Common_Error, ex.Message, AppResource.Common_OK);
                IsBusy = false;
                throw;
            }
        }
        #endregion

        #region Privete Methods
        private static List<(string,int)> getDayofWeek(int year, int month)
        {
            List<(string,int)> _daysOfMonthList = new List<(string,int)>();

            int totalDaysInMonth = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= totalDaysInMonth; i++)
            {
                DateTime _dateTime = new DateTime(year, month, i);

                if (_dateTime.DayOfWeek != DayOfWeek.Saturday && _dateTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    string day = _dateTime.ToString("ddd", new CultureInfo("es-ES")).Substring(0, 1).ToUpper();

                    _daysOfMonthList.Add((day,i));
                }                    
            }

            return _daysOfMonthList;
        }

        private static bool IsFileLocked(string filePath)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

        string GetColumnLetter(int columnNumber)
        {
            int dividend = columnNumber + 1;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        #endregion
    }
}
