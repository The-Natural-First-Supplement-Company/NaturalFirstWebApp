using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NaturalFirstAPI.Model;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NaturalFirstWebApp.ViewModels;

namespace NaturalFirstAPI.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("myConnectionString");
        }
        //Login for User and Admin
        public User GetUserLogin(User user)
        {
            User _loginUser = null;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_UserLoginByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            _loginUser = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Email = reader["Email"].ToString(),
                                ReferralCode = reader["ReferralCode"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            return _loginUser;
        }
        //Register New User
        public Common AddNewUser(User user)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_NewUserRegister", connection))
                    {
                        command.CommandType= CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });
                        command.Parameters.Add(new MySqlParameter("@userPassword", MySqlDbType.VarChar) { Value = user.Password });
                        command.Parameters.Add(new MySqlParameter("@referredBy", MySqlDbType.VarChar) { Value = user.ReferralCode });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@StatusId", MySqlDbType.Int32);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        MySqlParameter statusParameter = new MySqlParameter("@Status", MySqlDbType.VarChar, 50);
                        statusParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = (int)statusIdParameter.Value;
                        string status = statusParameter.Value.ToString();

                        common = new Common
                        {
                            StatusId = statusId,
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }
        //Reset Login Password
        public Common ResetPassword(ResetPassword reset)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_ResetPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = reset.Email });
                        command.Parameters.Add(new MySqlParameter("@userPassword", MySqlDbType.VarChar) { Value = reset.Password });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@StatusId", MySqlDbType.Int32);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        MySqlParameter statusParameter = new MySqlParameter("@Status", MySqlDbType.VarChar, 50);
                        statusParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = (int)statusIdParameter.Value;
                        string status = statusParameter.Value.ToString();

                        common = new Common
                        {
                            StatusId = statusId,
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }
        //Add Bank Account
        public Common InsertBankAccount(BankDetail bank)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_InsertBankAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userBank", MySqlDbType.VarChar) { Value = bank.BankName });
                        command.Parameters.Add(new MySqlParameter("@userName", MySqlDbType.VarChar) { Value = bank.RealName });
                        command.Parameters.Add(new MySqlParameter("@userAccount", MySqlDbType.VarChar) { Value = bank.AccountNo });
                        command.Parameters.Add(new MySqlParameter("@userIFSC", MySqlDbType.VarChar) { Value = bank.IFSCCode });
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = bank.email });
                        command.Parameters.Add(new MySqlParameter("@trnPassword", MySqlDbType.VarChar) { Value = bank.TrnPassword });

                        // Add output parameters to the command
                        command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = (int)command.Parameters["@StatusId"].Value;
                        string status = command.Parameters["@Status"].Value.ToString();

                        common = new Common
                        {
                            StatusId = statusId,
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }
        //User Details for Profile Information
        public User GetUserDetails(User user)
        {
            User _loginUser = null;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetUserInfo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _loginUser = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Email = reader["Email"].ToString(),
                                NickName = reader["NickName"].ToString(),
                                ProfilePic = reader["ProfilePic"] != DBNull.Value ? (byte[])reader["ProfilePic"]: null
                            };
                        }
                    }
                }
            }

            return _loginUser;
        }

        //Update Information of User Profile
        public Common UpdateUserInfo(User user)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_UpdateProfileInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });
                        command.Parameters.Add(new MySqlParameter("@userPic", MySqlDbType.LongBlob) { Value = user.ProfilePic });
                        command.Parameters.Add(new MySqlParameter("@userNick", MySqlDbType.VarChar) { Value = user.NickName });

                        // Add output parameters to the command
                        command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = (int)command.Parameters["@StatusId"].Value;
                        string status = command.Parameters["@Status"].Value.ToString();

                        common = new Common
                        {
                            StatusId = statusId,
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }
        //Reset Payment Password
        public Common ResetPaymentPassword(ResetPassword reset)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_ResetPaymentPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = reset.Email });
                        command.Parameters.Add(new MySqlParameter("@userPassword", MySqlDbType.VarChar) { Value = reset.Password });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@StatusId", MySqlDbType.Int32);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        MySqlParameter statusParameter = new MySqlParameter("@Status", MySqlDbType.VarChar, 50);
                        statusParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = (int)statusIdParameter.Value;
                        string status = statusParameter.Value.ToString();

                        common = new Common
                        {
                            StatusId = statusId,
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }
    }
}
