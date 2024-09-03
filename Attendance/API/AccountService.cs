using Attendance.Entities;
using Attendance.Entities.Request;
using Attendance.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using Windows.System;
using ZXing.Aztec.Internal;
using static System.Net.WebRequestMethods;
using _business = Attendance.Helpers;

namespace Attendance.API
{
    public class AccountService
    {
        //string ipAddress = _business.BusinessURL.ReadText();
        string ipAddress =  "192.168.1.35";
        //string ipAddress = "127.0.0.1";
        //string ipAddress = string.Empty;
        public AccountService() 
        {
            //ipAddress = GetIP();
            //ipAddress = GetEthernetIpAddress();
            //ShowAllIpAddresses();
        }
        private string GetIP()
        {
            string ip = string.Empty;
            // Obtener el nombre del host
            string hostName = Dns.GetHostName();
            Console.WriteLine("Nombre del host: " + hostName);

            // Obtener la lista de direcciones IP asociadas con el nombre del host
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // Mostrar cada dirección IPv4
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine("Dirección IPv4: " + address.ToString());
                    ip= address.ToString();
                }
            }
            return ip;
        }

        static void ShowAllIpAddresses()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine($"Interfaz: {networkInterface.Name}");
                Console.WriteLine($"  Tipo: {networkInterface.NetworkInterfaceType}");
                Console.WriteLine($"  Estado: {networkInterface.OperationalStatus}");

                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                foreach (UnicastIPAddressInformation ipAddress in ipProperties.UnicastAddresses)
                {
                    Console.WriteLine($"  Dirección IP: {ipAddress.Address}");
                }
                Console.WriteLine();
            }
        }

        public async Task<string> LoginUser(string username, string password)
        {
            string result = "";
            LoginRequest newUser = new LoginRequest(username, password);

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";
            string url = $"http://{ipAddress}/AttendanceWebApiA/users?login=true&suffix=users";

            var postData = new StringContent("{'key':'value'}");

            var user = new UserRequest
            {
                 email_user = username,
                password_user= password
            };

            var jsonNewUser = JsonSerializer.Serialize<UserRequest>(user);

            StringContent requestContent = new StringContent(jsonNewUser, Encoding.UTF8, "application/json");           

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    
                    var resp = await client.PostAsync(url, requestContent);
                    result = await resp.Content.ReadAsStringAsync();

                    if (resp.IsSuccessStatusCode)
                    {
                        //var message = new ApiRequest();
                        //message.status = "200";
                        //message.Result = "Success";
                        //result = message.ToJson();
                        return result;
                    }                                

                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        

        #region Course
        public async Task<string> GetSchoolGradeInfo(int idUser=0)
        {
            string result = "";

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";
            string url = $"http://{ipAddress}/AttendanceWebApiA/Courses?whereParam=id_user={idUser}";           

          
            using (HttpClient client = new HttpClient())
            {
                try
                {                    
                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);
                        var resp = await client.GetAsync(url);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        public async Task<string> DeleteCourse(int id_user, int idCourse= 0)
        {
            string result = "";

            string url = $"http://{ipAddress}/AttendanceWebApiA/Courses?whereParam=id_user={id_user}";

            if (idCourse > 0)
            {
                url += $"&id={idCourse}";
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);
                        var resp = await client.DeleteAsync(url);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            var message = new ApiRequest();
                            message.status = "200";
                            message.Result = "Success";
                            result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        public async Task<string> AddCourse(SchoolGrade schoolGrade)
        {
            string result = "";

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";
            string url = $"http://{ipAddress}/AttendanceWebApiA/Courses";

            var jsonNewAttendanceData = JsonSerializer.Serialize(schoolGrade);

            StringContent requestContent = new StringContent(jsonNewAttendanceData, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        var resp = await client.PostAsync(url, requestContent);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            //var message = new ApiRequest();
                            //message.status = "200";
                            //message.Result = "Success";
                            //result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;
        }
        #endregion
        
        #region Students
        public async Task<string> SaveStudents(Students students)
        {
            string result = "";
            
            string url = $"http://{ipAddress}/AttendanceWebApiA/students";

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        var jsonNewUser = JsonSerializer.Serialize(students);

                        StringContent requestContent = new StringContent(jsonNewUser, Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);
                        
                        
                        var resp = await client.PostAsync(url, requestContent);
                        
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            //var message = new ApiRequest();
                            //message.status = "200";
                            //message.Result = "Success";
                            //result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }
                        

                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        public async Task<string> DeleteStudent(int idStudent)
        {
            string result = "";

            string url = $"http://{ipAddress}/AttendanceWebApiA/Students?id={idStudent}";

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);
                        var resp = await client.DeleteAsync(url);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            var message = new ApiRequest();
                            message.status = "200";
                            message.Result = "Success";
                            result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        public async Task<string> DeleteStudents(int idUser)
        {
            string result = "";

            string url = $"http://{ipAddress}/AttendanceWebApiA/Students?whereParam=id_user={idUser}";

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);
                        var resp = await client.DeleteAsync(url);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            var message = new ApiRequest();
                            message.status = "200";
                            message.Result = "Success";
                            result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }        

        public async Task<string> UpdateStudents(Students students, int idStudent)
        {
            string result = "";

            string url = $"http://{ipAddress}/AttendanceWebApiA/students?id={idStudent}";

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        var jsonNewUser = JsonSerializer.Serialize(students);

                        StringContent requestContent = new StringContent(jsonNewUser, Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);

                        var resp = await client.PutAsync(url, requestContent);

                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            var message = new ApiRequest();
                            message.status = "200";
                            message.Result = "Success";
                            result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        public async Task<string> GetStudents(int id = 0)
        {
            string result = "";

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";
            string url = $"http://{ipAddress}/AttendanceWebApiA/Students";

            if (id > 0)
            {
                url += $"?whereParam=id_user={id}";
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);
                        var resp = await client.GetAsync(url);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }

        public async Task<string> CreateStudents(Students students)
        {
            string result = "";

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";
            string url = $"http://{ipAddress}/AttendanceWebApiA/Students?id_user={Session._IdUser}";


            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        var jsonNewAttendanceData = JsonSerializer.Serialize(students);

                        StringContent requestContent = new StringContent(jsonNewAttendanceData, Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);

                        var resp = await client.PutAsync(url, requestContent);

                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            //var message = new ApiRequest();
                            //message.status = "200";
                            //message.Result = "Success";
                            //result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }
        #endregion

        #region attendance
        public async Task<string> SaveAttendance(int id_student, int id_course)
        {
            string result = "";

            string url = $"http://{ipAddress}/AttendanceWebApiA/students";

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        var user = new AttendanceRequest()
                        {
                            id_student = id_student,
                            id_user = Session._IdUser,
                            id_course = id_course,
                            attendaceDate = DateTime.Now
                        };

                        var jsonNewUser = JsonSerializer.Serialize(user);

                        StringContent requestContent = new StringContent(jsonNewUser, Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session._token);


                        var resp = await client.PostAsync(url, requestContent);

                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            //var message = new ApiRequest();
                            //message.status = "200";
                            //message.Result = "Success";
                            //result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;
        }

        public async Task<string> SaveAttendanceData(AttendanceEnt _lts)
        {
            string result = "";

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";

            string url = $"http://{ipAddress}/AttendanceWebApiA/attendance";

            var jsonNewAttendanceData = JsonSerializer.Serialize<AttendanceEnt>(_lts);

            StringContent requestContent = new StringContent(jsonNewAttendanceData, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (TokenJWT.ValidateToken(Session._token))
                    {
                        var resp = await client.PostAsync(url, requestContent);
                        result = await resp.Content.ReadAsStringAsync();

                        if (resp.IsSuccessStatusCode)
                        {
                            //var message = new ApiRequest();
                            //message.status = "200";
                            //message.Result = "Success";
                            //result = message.ToJson();
                            return result;
                        }
                    }
                    else
                    {
                        var message = new ApiRequest();
                        message.status = "500";
                        message.Result = "Error de validación del token";
                        result = message.ToJson();
                    }


                }
                catch (Exception ex)
                {
                    var message = new ApiRequest();
                    message.status = "400";
                    message.Result = ex.Message;
                    result = message.ToJson();
                }

            }


            return result;

        }
        #endregion

    }
}
