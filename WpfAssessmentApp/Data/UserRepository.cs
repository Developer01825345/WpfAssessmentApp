using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using WpfAssessmentApp.Model;

namespace WpfAssessmentApp.Data
{
    public class UserRepository
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
       /// Get All Users
       /// </summary>
       /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("GetAllUsers", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DOB = reader.GetDateTime(3),
                        Email = reader.GetString(4),
                        Gender = reader.GetString(5),
                        Phone = reader.GetString(6),
                        Language = reader.GetString(7)
                    });

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            return users;
        }

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("AddUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DOB", user.DOB);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Gender", user.Gender);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Language", user.Language);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("UpdateUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DOB", user.DOB);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Gender", user.Gender);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Language", user.Language);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("DeleteUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
