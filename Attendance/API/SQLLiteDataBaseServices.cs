using Attendance.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.API
{
    public class SQLLiteDataBaseServices
    {
        private readonly SQLiteAsyncConnection _database;

        public SQLLiteDataBaseServices(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UserSQLite>().Wait();
            _database.CreateTableAsync<StudentsSQLite>().Wait();
            _database.CreateTableAsync<SchoolGradeSQLite>().Wait();
            _database.CreateTableAsync<AttendanceEntSQLite>().Wait();
        }

        #region Users Queries
        public Task<List<UserSQLite>> getUsersAsync()
        {
            return _database.Table<UserSQLite>().ToListAsync();
        }

        public Task<int> CreateUserAsync(UserSQLite _user)
        {
            if (_user.id != 0)
            {
                return _database.UpdateAsync(_user);
            }
            else
            {
                return _database.InsertAsync(_user);
            }
        }

        public Task<int> DeleteUserAsync(UserSQLite _user)
        {
            return _database.DeleteAsync(_user);
        }

        public Task<UserSQLite> getUserbyIdAsync(int id)
        {
            return _database.Table<UserSQLite>().Where(u => u.id == id).FirstOrDefaultAsync();
        }

        public Task<UserSQLite> getUserbyUserAsync(string email)
        {
            return _database.Table<UserSQLite>().Where(u => u.email_user == email).FirstOrDefaultAsync();
        }

        public Task<UserSQLite> loginAsync(string email, string password)
        {
            return _database.Table<UserSQLite>().Where(u => u.email_user == email && u.password == password).FirstOrDefaultAsync();
        }
        #endregion

        #region Student Queries

        public Task<List<StudentsSQLite>> getStudentAsync()
        {
            return _database.Table<StudentsSQLite>().ToListAsync();
        }
        public Task<List<StudentsSQLite>> getUserbyIdUserAsync(int id_user)
        {
            return _database.Table<StudentsSQLite>().Where(u => u.id_user == id_user).ToListAsync();
        }

        public Task<List<StudentsSQLite>> getUserbyIdUserCourseAsync(int id_user, int id_course)
        {
            return _database.Table<StudentsSQLite>().Where(u => u.id_user == id_user && u.id_course == id_course).ToListAsync();
        }

        public Task<int> CreateStudentAsync(StudentsSQLite _students)
        {
            if (_students.id != 0)
            {
                return _database.UpdateAsync(_students);
            }
            else
            {
                return _database.InsertAsync(_students);
            }
        }

        public Task<int> DeleteStudentAsync(StudentsSQLite students)
        {
            return _database.DeleteAsync(students);
        }

        public Task<StudentsSQLite> getStudentbyIdAsync(int id)
        {
            return _database.Table<StudentsSQLite>().Where(u => u.id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> getStudentbyIdAsync(int id, int id_course)
        {
            return await _database.Table<StudentsSQLite>().Where(u => u.id == id  && u.id_course == id_course).FirstOrDefaultAsync() != null ;
        }
        #endregion

        #region SchoolGrade

        public Task<List<SchoolGradeSQLite>> getSchoolGradeAsync()
        {
            return _database.Table<SchoolGradeSQLite>().ToListAsync();
        }
        public Task<SchoolGradeSQLite> getSchoolGradebyIdAsync(int id)
        {
            return _database.Table<SchoolGradeSQLite>().Where(u => u.id == id).FirstOrDefaultAsync();
        }
        public Task<List<SchoolGradeSQLite>> getSchoolGradebyIdUserAsync(int idUser)
        {
            return _database.Table<SchoolGradeSQLite>().Where(u => u.id_user == idUser).ToListAsync();
        }
        public Task<List<SchoolGradeSQLite>> existSchoolGradeAsync(int idUser, string name)
        {
            return _database.Table<SchoolGradeSQLite>().Where(u => u.id_user == idUser && u.course_name==name).ToListAsync();
        }

        public Task<int> CreateGradeAsync(SchoolGradeSQLite _grade)
        {
            if (_grade.id != 0)
            {
                return _database.UpdateAsync(_grade);
            }
            else
            {
                return _database.InsertAsync(_grade);
            }
        }

        public Task<int> DeleteSchoolGradeAsync(SchoolGradeSQLite _grade)
        {
            return _database.DeleteAsync(_grade);
        }

        #endregion

        #region Attendance

        public Task<List<AttendanceEntSQLite>> getAttendaceAsync()
        {
            return _database.Table<AttendanceEntSQLite>().ToListAsync();
        }
        public Task<AttendanceEntSQLite> getAttendacebyIdAsync(int id)
        {
            return _database.Table<AttendanceEntSQLite>().Where(u => u.id == id).FirstOrDefaultAsync();
        }
        public Task<List<AttendanceEntSQLite>> getAttendacebyIdUserAsync(string idUser)
        {
            return _database.Table<AttendanceEntSQLite>().Where(u => u.id_user == idUser).ToListAsync();
        }

        public Task<int> CreateAttendaceAsync(AttendanceEntSQLite _attendance)
        {
            if (_attendance.id != 0)
            {
                return _database.UpdateAsync(_attendance);
            }
            else
            {
                return _database.InsertAsync(_attendance);
            }
        }

        public Task<int> DeleteAttendaceAsync(AttendanceEntSQLite _attendance)
        {
            return _database.DeleteAsync(_attendance);
        }

        #endregion
    }
}
