using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NaturalFirstAPI.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NaturalFirstAPI.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using NaturalFirstAPI.ViewModels;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Globalization;
using NaturalFirstAPI.Models;
using Org.BouncyCastle.Ocsp;

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
                        if (reader.Read())
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
                        command.CommandType = CommandType.StoredProcedure;
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
                                ProfilePic = reader["ProfilePic"] != DBNull.Value ? (byte[])reader["ProfilePic"] : null
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
        //Wallet Balance
        public decimal GetWalletBalance(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_GetWalletBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@Balance", MySqlDbType.Decimal);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        Decimal balance = (Decimal)statusIdParameter.Value;
                        return balance;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return 0;
                }
            }
        }
        //Add Wallet amount to user account
        public Common RechargeWallet(RechargeVM recharge)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_AddWalletBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = recharge.Email });
                        command.Parameters.Add(new MySqlParameter("@userAmount", MySqlDbType.Decimal) { Value = recharge.Amount });
                        command.Parameters.Add(new MySqlParameter("@userPayCode", MySqlDbType.VarChar) { Value = recharge.PayCode });
                        command.Parameters.Add(new MySqlParameter("@userOption", MySqlDbType.VarChar) { Value = recharge.PayOption });

                        // Output parameters
                        // Add output parameters to the command
                        command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = (int)command.Parameters["@StatusId"].Value;
                        string status = command.Parameters["@Status"].Value.ToString();

                        Common common = new Common
                        {
                            StatusId = statusId,
                            Status = status
                        };
                        return common;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return new Common { Status = ex.Message, StatusId = 0 };
                }
            }
        }
        //Get list of recharge made by a particular user
        public List<RechargeHistoryVM> GetRecharges(RechargeVM rec)
        {
            List<RechargeHistoryVM> recharges = new List<RechargeHistoryVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetRechargeHistoryUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = rec.Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RechargeHistoryVM _recharge = new RechargeHistoryVM();
                            _recharge.IdHistory = Convert.ToInt32(reader["IdHistory"]);
                            _recharge.Amount = (Decimal)reader["Amount"];
                            _recharge.Status = Convert.ToInt32(reader["Status"]);
                            _recharge.TrnCode = reader["TrnCode"].ToString();
                            _recharge.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                            recharges.Add(_recharge);
                        }
                    }
                }
            }
            return recharges;
        }
        //true if bank details exist else false
        public bool IfBankExists(User user)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_IfBankExists", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@bank", MySqlDbType.Int32);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        int statusId = (int)statusIdParameter.Value;
                        if (statusId == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return false;
        }
        //Get Bank Details of a user
        public BankDetail GetBankDetail(User user)
        {
            BankDetail _bank = new BankDetail();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_BankDetailsForUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _bank.AccountNo = reader["AccountNo"].ToString();
                            _bank.BankName = reader["BankName"].ToString();
                            _bank.RealName = reader["RealName"].ToString();
                            _bank.IFSCCode = reader["IFSCCode"].ToString();
                        }
                    }
                }
            }
            return _bank;
        }

        //Get Balance Details
        public Decimal GetBalanceDetails(User user)
        {
            Decimal result = 0;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_CalculateActualBalanceForWithdraw", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@result", MySqlDbType.Decimal);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        result = (Decimal)statusIdParameter.Value;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return result;
        }

        public Common InsertWithdrawRequest(WithdrawVM withdraw)
        {
            Common common = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_AddWithdrawRequest", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = withdraw.Email });
                        command.Parameters.Add(new MySqlParameter("@userAmount", MySqlDbType.VarChar) { Value = withdraw.Amount });
                        command.Parameters.Add(new MySqlParameter("@userPassword", MySqlDbType.VarChar) { Value = withdraw.TrnPassword });

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

        public List<Withdraw> GetWithdrawalHistoryUser(User user)
        {
            List<Withdraw> data = new List<Withdraw>();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetWithdrawalHistoryForUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Withdraw _data = new Withdraw();
                            _data.IdWithdraw = Convert.ToInt32(reader["IdWithdraw"]);
                            _data.Amount = (Decimal)reader["Amount"];
                            _data.Status = Convert.ToInt32(reader["Status"]);
                            _data.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                            data.Add(_data);
                        }
                    }
                }
            }
            return data;
        }

        public PDWallet GetPDWallet(User user)
        {
            PDWallet data = new PDWallet();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetUserPurchaseWallet", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            data.Balance = reader["Balance"] != DBNull.Value?(Decimal)reader["Balance"]:0;
                            data.Recharge = reader["Recharge"] != DBNull.Value ? (Decimal)reader["Recharge"] : 0;
                            //data.Id = (int)reader["Id"] == null?0: (int)reader["Id"];
                            //data.UserId = (int)reader["UserId"] == null ? 0 : (int)reader["UserId"];
                            
                        }
                    }
                }
            }
            return data;
        }

        public decimal GetBalanceForWithdraw(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_GetBalanceForUserWithdrawPage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = user.Email });

                        // Output parameters
                        MySqlParameter statusIdParameter = new MySqlParameter("@Balance", MySqlDbType.Decimal);
                        statusIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(statusIdParameter);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        Decimal balance = (Decimal)statusIdParameter.Value;
                        return balance;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return 0;
                }
            }
        }

        public List<MyTeamVM> GetMyTeamList(MyTeamPostVM myTeam)
        {
            List<MyTeamVM> data = new List<MyTeamVM>();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string proc = "";
                if(myTeam.Percent == 10)
                {
                    proc = "sp_Get10PercUserListForReferrer";
                }else if(myTeam.Percent == 5)
                {
                    proc = "sp_Get5PercUserListForReferrer";
                }else if (myTeam.Percent == 3)
                {
                    proc = "sp_Get3PercUserListForReferrer";
                }
                using (MySqlCommand command = new MySqlCommand(proc, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userId", MySqlDbType.Int32) { Value = myTeam.UserId });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MyTeamVM team = new MyTeamVM();
                            team.UserId = (int)reader["UserId"];
                            team.Email = reader["Email"].ToString();
                            team.CreatedDate = (DateTime)reader["CreatedDate"];
                            //data.Id = (int)reader["Id"] == null?0: (int)reader["Id"];
                            //data.UserId = (int)reader["UserId"] == null ? 0 : (int)reader["UserId"];
                            data.Add(team);
                        }
                    }
                }
            }
            return data;
        }
        //{
        //sp_GetUserMyTeam
        //}
        public UserProfile GetUserProfile(User user)
        {
            UserProfile _loginUser = new UserProfile();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetUserProfileInformation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userId", MySqlDbType.VarChar) { Value = user.Id });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _loginUser.Id = Convert.ToInt32(reader["Id"]);
                            _loginUser.Email = reader["Email"].ToString();
                            _loginUser.Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null;
                            _loginUser.Recharge = reader["Recharge"] != DBNull.Value ? Convert.ToDecimal(reader["Recharge"]) : 0;
                            _loginUser.Balance = Convert.ToDecimal(reader["Balance"]);
                            _loginUser.TotalEarning = Convert.ToDecimal(reader["TotalEarning"]);
                            _loginUser.TeamIncome = Convert.ToDecimal(reader["TeamIncome"]);
                            _loginUser.IncomeToday = Convert.ToDecimal(reader["IncomeToday"]);
                        }
                    }
                }
            }
            return _loginUser;
        }
    }
}
