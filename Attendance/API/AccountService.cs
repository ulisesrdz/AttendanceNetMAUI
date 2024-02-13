using Attendance.Entities;
using Attendance.Entities.Request;
using Attendance.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZXing.Aztec.Internal;
using static System.Net.WebRequestMethods;
using _business = Attendance.Helpers;

namespace Attendance.API
{
    public class AccountService
    {
        //string ipAddress = _business.BusinessURL.ReadText();
        string ipAddress =  "192.168.1.22";

        public AccountService() 
        {
            GetIP();
        }
        private string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return ipAddress;
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

        public async Task<string> SaveAttendanceData(List<AttendanceEnt> _lts)
        {
            string result = "";           

            //string url = $"{ipAddress}WebAPIAsistencia/users?login=true&suffix=users";
            string url = $"http://{ipAddress}/AttendanceWebApiA/attendace?login=true&suffix=users";
           
            var jsonNewAttendanceData = JsonSerializer.Serialize<List<AttendanceEnt>>(_lts);

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

    }
}
