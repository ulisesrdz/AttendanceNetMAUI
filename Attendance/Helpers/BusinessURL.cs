using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Helpers
{
    public class BusinessURL
    {
        static string DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        const string fileName = "BusinessURL";
        public static void SaveText(string data)
        {
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.WriteAllText(filePath, data);
        }

        public static string ReadText()
        {
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            if (!File.Exists(filePath))
                return null;
            return File.ReadAllText(filePath);
        }
    }
}
