using Student_Tracker_App.Schemas;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Student_Tracker_App.Services
{
    public class DatabaseServices
    {
        private SQLiteAsyncConnection _database;

        public DatabaseServices(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            CreateTableAsync().Wait();
        }

        private async Task CreateTableAsync()
        {
            // Create Terms, Courses, and Assessments table if they don't exist
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
        }


        #region User Data
        // User Account Verification
        /// <summary>
        /// Checks if a users account exists in the database
        /// </summary>
        public async Task<bool> CheckUserExistsAsync(string username)
        {
            var user = await _database.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            return user != null; // Returns true if the user is found, false if not
        }

        // User Registration Method
        /// <summary>
        /// Registers a new user. Returns false if the username already exists.
        /// </summary>
        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            // Check if the username already exists
            var existingUser = await _database.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            if (existingUser != null) 
            {
                return false; // Username already exists
            }

            // Create user with hashed password

            var newUser = new User
            {
                Username = username,
                PasswordHash = SecurityHelper.HashPassword(password)
            };

            await _database.InsertAsync(newUser);
            return true;
        }

        // User Login Method
        /// <summary>
        /// Validates a user's login attempt against the hashed password.
        /// </summary>
        
        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            // Fetch the user by username
            var user = await _database.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return false; // User not found
            }

            // Hash the input password and compare with stored hash
            string inputHash = SecurityHelper.HashPassword(password);
            return user.PasswordHash == inputHash;
        }

        #endregion

        #region Term Data

        /// <summary>
        /// Fetch all terms from database
        /// </summary>
        /// <returns></returns>
        public Task<List<Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        /// <summary>
        /// Fetch Terms by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Term> GetTermAsync(int id)
        {
            return _database.Table<Term>()
                .Where(t => t.TermId == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Deletes a term from the database
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public Task<int> DeleteTermAsync(Term term)
        {
            return _database.DeleteAsync(term);
        }


        /// <summary>
        /// Saves or updates a term in the database
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public Task<int> SaveTermAsync(Term term)
        {
            try
            {
                if (term.TermId != 0)
                {
                    return _database.UpdateAsync(term);
                }
                else
                {
                    return _database.InsertAsync(term);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving term: {ex.Message}");
            }
        }
        #endregion

        #region Course Data

        /// <summary>
        /// Fetch all courses from database
        /// </summary>
        /// <param name="termId"></param>
        /// <returns></returns>

        public Task<List<Course>> GetCoursesAsync(int termId)
        {
            return _database.Table<Course>()
                .Where(c => c.TermId == termId)
                .ToListAsync();
        }

        /// <summary>
        /// Saves or updates a course in the database
        /// <param name="course"></param>
        /// </summary>
        /// <returns></returns>

        public Task<int> SaveCourseAsync(Course course)
        {

            if (course.CourseId != 0)
            {
                return _database.UpdateAsync(course);
            }
            else
            {
                return _database.InsertAsync(course);
            }
        }

        /// <summary>
        /// Deletes a course from the database
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>

        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);
        }
        #endregion

        #region Assessment Data

        public Task<List<Assessment>> GetAssessmentsAsync(int courseId)
        {
            return _database.Table<Assessment>()
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
        }

        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.AssessmentId != 0)
            {
                return _database.UpdateAsync(assessment);
            }
            else
            {
                return _database.InsertAsync(assessment);
            }
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }
        #endregion
    }
}
