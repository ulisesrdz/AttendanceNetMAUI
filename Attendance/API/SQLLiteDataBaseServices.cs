﻿using Attendance.Entities;
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
        private static bool _tablesInitialized = false;
        public SQLLiteDataBaseServices(string dbPath)
        {            
            _database = new SQLiteAsyncConnection(dbPath);

            // verify if the db is already initialized
            if (!_tablesInitialized)
            {
                Task.Run(async () => await InitializeDatabase()).Wait();
                _tablesInitialized = true;
            }
        }

        private async Task InitializeDatabase()
        {
            if (!await TableExistsAsync("UserSQLite"))
                await _database.CreateTableAsync<UserSQLite>();
            if (!await TableExistsAsync("StudentsSQLite"))
                await _database.CreateTableAsync<StudentsSQLite>();
            if (!await TableExistsAsync("SchoolGradeSQLite"))
                await _database.CreateTableAsync<SchoolGradeSQLite>();
            if (!await TableExistsAsync("AttendanceEntSQLite"))
                await _database.CreateTableAsync<AttendanceEntSQLite>();
            if (!await TableExistsAsync("StudentInCourseSQLite"))
                await _database.CreateTableAsync<StudentInCourseSQLite>();
        }

        private async Task<bool> TableExistsAsync(string tableName)
        {
            var result = await _database.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}';");

            return result > 0;
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
            //return _database.Table<StudentsSQLite>().Where(u => u.id_user == id_user && u.id_course == id_course).ToListAsync();
            var query = @"SELECT s.*
                        FROM StudentsSQLite s
                        INNER JOIN StudentInCourseSQLite sic ON s.id_user = sic.id_student
                        WHERE s.id_user = ? AND sic.id_course = ?";

            return _database.QueryAsync<StudentsSQLite>(query, id_user, id_course);
        }
        public Task<int> AddCourseStudentAsync(StudentInCourseSQLite _students)
        {
            return _database.InsertAsync(_students);            
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
            //return await _database.Table<StudentsSQLite>().Where(u => u.id == id  && u.id_course == id_course).FirstOrDefaultAsync() != null ;
            var query = @"SELECT 1 
                        FROM StudentsSQLite s
                        INNER JOIN StudentInCourseSQLite sic ON s.id = sic.id_student
                        WHERE s.id = ? AND sic.id_course = ? 
                        LIMIT 1";

            var result = await _database.QueryAsync<StudentsSQLite>(query, id, id_course);
            return result.Any();
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
        public Task<List<AttendanceEntSQLite>> getAttendacebyIdUserAsync(string idUser, string id_course, int month)
        {
            int selectedYear = DateTime.Now.Year;
            int selectedMonth = month;
            DateTime startDate = new DateTime(selectedYear, selectedMonth, 1); // 1 de noviembre de 2024
            DateTime endDate = startDate.AddMonths(1);

            return _database.Table<AttendanceEntSQLite>().Where(u => u.id_user == idUser 
                                                                && u.id_course == id_course
                                                               && u.date_time >= startDate
             && u.date_time < endDate).ToListAsync();
        }

        public Task<List<AttendanceEntSQLite>> getAttendacebyStudentAsync(string idUser, string id_course, string idStudent)
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            return _database.Table<AttendanceEntSQLite>().Where(u => u.id_user == idUser 
                                                                    && u.id_course == id_course 
                                                                    && u.id_student == idStudent
                                                                    && u.date_time >= startDate 
                                                                    && u.date_time <= endDate).ToListAsync();
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

        public async Task<bool> getAttendanceRegistered(string id_student, string id_course)
        {
            //return await _database.Table<AttendanceEntSQLite>().Where(u => u.id_student == id_student && DateTime.Now.Date>=u.date_time.Date && u.id_course== id_course).FirstOrDefaultAsync() != null;
            var attendance = await _database.Table<AttendanceEntSQLite>()
                                    .Where(u => u.id_student == id_student && u.id_course == id_course)
                                    .ToListAsync();

            return attendance.Any(u => u.date_time.Date == DateTime.Now.Date);
        }
        #endregion
    }
}
