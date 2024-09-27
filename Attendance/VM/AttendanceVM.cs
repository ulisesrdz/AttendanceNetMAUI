using Attendance.API;
using Attendance.Entities;
using Attendance.Helpers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
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

        private List<SchoolGrade> _ltsGrade { get; set; }
        public List<SchoolGrade> ltsGrade
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

        private List<AttendanceEnt> _ltsAttendance { get; set; }
        public List<AttendanceEnt> ltsAttendance
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

        public AttendanceVM()
        {
            InitVM();
            _accountService = new AccountService();
            //Get_Information();
            if (Session.attendanceView)
            {
                Get_InformationLocal();
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
           
            _lts = new List<AttendanceEnt>();
        }

        #region API
        private async void Tapped_For_Enter(object sender)
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy=true;
                    if (Session.attendanceView)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.StudentsList());
                    }
                    else
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new page.Attendance());
                    }
                    
                    IsBusy=false;
                }
            }
            catch (Exception)
            {

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
            catch (Exception)
            {

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
                            await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                        }
                        else if (jsonResult.status == "500")
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                        }
                        else if (jsonResult.status == "200")
                        {
                            //var _obj = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Result);
                            ltsGrade = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolGrade>>(jsonResult.Result.ToString());

                            //_ltsGrade
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
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
                                        await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                                    }
                                    else if (jsonResult.status == "500")
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Error", jsonResult.Result.ToString(), "Aceptar");
                                    }
                                    else if (jsonResult.status == "200")
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Success", jsonResult.Result.ToString(), "Aceptar");
                                    }
                                });
                            }
                            

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Missing URL", "OK");
                        }
                    }
                        
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsBusy = false;
            }
        }

        #endregion

        #region Local
        private async void Get_InformationLocal()
        {
            
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    int counter = 0;
                    var ltsAtt = new List<AttendanceEnt>();
                    var attenaces = await App.DataBase.getAttendacebyIdUserAsync(Session._IdUser.ToString());
                    //var student = await App.DataBase.getAttendacebyIdUserAsync(Session.);
                    if (attenaces.Count > 0)
                    {
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

                            ltsAtt.Add(attendace);
                        }

                        if (ltsAtt.Count>0)
                        {
                            ltsAttendance = ltsAtt;
                        }
                    }
                    //{
                    //    var _grade = new List<SchoolGrade>();
                    //    foreach (var item in schoolGradeInfo)
                    //    {
                    //        var grade = new SchoolGrade();
                    //        grade.id = item.id;
                    //        grade.id_user = item.id_user;
                    //        grade.course_name = item.course_name;
                    //        grade.groups = item.groups;
                    //        grade.grade = item.grade;


                    //        _grade.Add(grade);
                    //    }

                    //    if (_grade.Count > 0)
                    //    {
                    //        ltsGrade = _grade;
                    //    }
                    //}
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
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
                            var exists= await App.DataBase.getStudentbyIdAsync(id_student, id_course);
                            if (!exists)
                            {
                                await Application.Current.MainPage.DisplayAlert("Failed", "This student " + item.student_name + " is not registred in this class", "Ok");
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
                                await Application.Current.MainPage.DisplayAlert("Success", "Data Saved for " + item.student_name, "Ok");
                                CleanData();
                            }
                            
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Missing URL", "OK");
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
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
                    
                    

                    //ICellStyle dataStyle = workbook.CreateCellStyle();
                    //dataStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
                    //dataStyle.FillPattern = FillPattern.SolidForeground;

                    //ICellStyle mergedStyle = workbook.CreateCellStyle();
                    //mergedStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    //mergedStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

                    //ICellStyle verticalTextStyle = workbook.CreateCellStyle();
                    //verticalTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    //verticalTextStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    //verticalTextStyle.Rotation = 90;

                    ISheet sheet = workbook.CreateSheet("Sheet1");

                   
                    // Header Style
                    ICellStyle headerStyle = workbook.CreateCellStyle();
                    headerStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
                    headerStyle.FillPattern = FillPattern.SolidForeground;
                    headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    headerStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    // Header Title
                    IRow row1 = sheet.CreateRow(0);
                    row1.CreateCell(3).SetCellValue("CONTROL DE ASISTENCIA");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 3, 25)); 
                    row1.GetCell(3).CellStyle = headerStyle;

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

                    ((XSSFCellStyle)nameStyle).SetTopBorderColor(borderColor);
                    ((XSSFCellStyle)nameStyle).SetBottomBorderColor(borderColor);
                    ((XSSFCellStyle)nameStyle).SetLeftBorderColor(borderColor);
                    ((XSSFCellStyle)nameStyle).SetRightBorderColor(borderColor);
                   
                   
                    IRow row2 = sheet.CreateRow(2);
                    ICell itemMergedCell = row2.CreateCell(0);
                    ICell nameMergedCell = row2.CreateCell(1);
                    ICell monthMergedCell = row2.CreateCell(2);
                    //ICell DayCell = row2.CreateCell(3);
                    //ICell DayNoCell = row2.CreateCell(4);
                    row2.CreateCell(0).SetCellValue("No.");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 5, 0, 0));
                    row2.CreateCell(1).SetCellValue("Nombre y Apellidos");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 5, 1, 1));
                    row2.CreateCell(2).SetCellValue($"{DateTime.Now.ToString("MMMM",new CultureInfo("es-ES")).ToUpper()}");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 2, 10));
                    
                    //row2.CreateCell(4).SetCellValue("1");
                    nameMergedCell.CellStyle = nameStyle;
                    itemMergedCell.CellStyle = itemStyle;
                    monthMergedCell.CellStyle = nameStyle;
                    //DayCell.CellStyle = itemStyle;
                    //DayNoCell.CellStyle = itemStyle;
                    sheet.SetColumnWidth(0, 4 * 256);
                    sheet.SetColumnWidth(1, 30 * 256);
                    
                    
                    IRow row3 = sheet.CreateRow(3);
                    ICell DayCell = row2.CreateCell(3);
                    row3.CreateCell(3).SetCellValue("V");
                    DayCell.CellStyle = itemStyle;
                    // Crear celdas normales debajo del encabezado combinado

                    //row2.CreateCell(0).SetCellValue("ID");
                    //row2.CreateCell(1).SetCellValue("Nombre");
                    //row2.CreateCell(2).SetCellValue("Edad");

                    //// Combinar celdas verticalmente (Filas 3 a 4 en la columna A)
                    //IRow row3 = sheet.CreateRow(2);
                    //ICell mergedCell = row3.CreateCell(0);
                    //mergedCell.SetCellValue("Valor Combinado");
                    //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 6, 0, 0)); // (fila_inicio, fila_fin, col_inicio, col_fin)

                    //// Aplicar el estilo a la celda combinada verticalmente
                    //mergedCell.CellStyle = verticalTextStyle;

                    //// Agregar datos a otras celdas no combinadas
                    //row3.CreateCell(1).SetCellValue("Ejemplo");
                    //row3.CreateCell(2).SetCellValue(25);

                    //IRow row4 = sheet.CreateRow(3);
                    //row4.CreateCell(1).SetCellValue("Otro");
                    //row4.CreateCell(2).SetCellValue(30);

                    //// Crear una fila y añadir encabezados
                    //IRow headerRow = sheet.CreateRow(0);
                    //ICell headerCell1 = headerRow.CreateCell(0);
                    //headerCell1.SetCellValue("ID");
                    //headerCell1.CellStyle = headerStyle;
                    //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 2)); // (fila_inicio, fila_fin, col_inicio, col_fin)
                    //row1.GetCell(0).CellStyle = mergedStyle;                                                                     // Crear una fila y añadir encabezados

                    //ICell headerCell2 = headerRow.CreateCell(1);
                    //headerCell2.SetCellValue("Nombre");
                    //headerCell2.CellStyle = headerStyle;
                    ////headerRow2.CreateCell(1).SetCellValue("Nombre");

                    //// Crear una fila con datos
                    //IRow dataRow = sheet.CreateRow(1);
                    //dataRow.CreateCell(0).SetCellValue(1);
                    //dataRow.CreateCell(1).SetCellValue("Ejemplo");

                    // Guardar el archivo Excel
                    string filePath = Path.Combine(FileSystem.AppDataDirectory, "attendance.xlsx");
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fileStream);
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsBusy = false;
                throw;
            }
        }
        #endregion
    }
}
