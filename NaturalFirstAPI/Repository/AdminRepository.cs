using MySql.Data.MySqlClient;
using NaturalFirstAPI.Model;
using NaturalFirstAPI.Models;
using NaturalFirstAPI.ViewModels;
using System.Collections.Generic;
using System.Data;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace NaturalFirstAPI.Repository
{
    public class AdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("myConnectionString");
        }
        //Get Pending Recharge List for Admin Only
        public List<RechargeListVM> GetPendingRechargeList(string Email)
        {
            List<RechargeListVM> lst = new List<RechargeListVM>();
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetPendingRechargeList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RechargeListVM recharge = new RechargeListVM
                            {
                                IdHistory = Convert.ToInt32(reader["IdHistory"]),
                                Email = reader["Email"].ToString(),
                                Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null,
                                Amount = (Decimal)reader["Amount"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                Status = reader["Status"].ToString(),
                                TrnCode = reader["TrnCode"].ToString(),
                                UserId = (int)reader["UserId"]
                            };
                            lst.Add(recharge);
                        }
                    }
                }
            }
            return lst;
        }

        public RechargeListVM GetRechargeDetail(RechargeHistoryVM rc)
        {
            RechargeListVM recharge = new RechargeListVM();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetPendingRechargeDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = rc.Email });
                    command.Parameters.Add(new MySqlParameter("@rcId", MySqlDbType.VarChar) { Value = rc.IdHistory });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            recharge.IdHistory = Convert.ToInt32(reader["IdHistory"]);
                            recharge.Email = reader["Email"].ToString();
                            recharge.Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null;
                            recharge.Amount = (Decimal)reader["Amount"];
                            recharge.CreatedDate = (DateTime)reader["CreatedDate"];
                            recharge.Status = reader["Status"].ToString();
                            recharge.TrnCode = reader["TrnCode"].ToString();
                            recharge.UserId = (int)reader["UserId"];
                        }
                    }
                }
            }
            return recharge;
        }

        //Update Recharge Status By Admin Only
        public Common UpdateRechargeStatus(RechargeHistoryVM recharge)
        {
            Common common = new Common();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_UpdateRechargeStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = recharge.Email });
                    command.Parameters.Add(new MySqlParameter("@rcId", MySqlDbType.VarChar) { Value = recharge.IdHistory });
                    command.Parameters.Add(new MySqlParameter("@rcStatus", MySqlDbType.VarChar) { Value = recharge.Status });
                    command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    // Retrieve the output parameter values
                    common.StatusId = (int)command.Parameters["@StatusId"].Value;
                    common.Status = command.Parameters["@Status"].Value.ToString();
                }
            }
            return common;
        }

        public List<AdminWithdrawVM> GetPendingWithdrawList(string Email)
        {
            List<AdminWithdrawVM> lst = new List<AdminWithdrawVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetPendingWithdrawList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AdminWithdrawVM recharge = new AdminWithdrawVM
                            {
                                IdWithdraw = Convert.ToInt32(reader["IdWithdraw"]),
                                Email = reader["Email"].ToString(),
                                Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null,
                                Amount = (Decimal)reader["Amount"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                Status = reader["Status"].ToString(),
                                UserId = (int)reader["UserId"]
                            };
                            lst.Add(recharge);
                        }
                    }
                }
            }
            return lst;
        }

        public WithdrawDetail GetWithdrawDetailById(WithdrawVM withdraw)
        {
            WithdrawDetail data = new WithdrawDetail();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetPendingWithdrawDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = withdraw.Email });
                    command.Parameters.Add(new MySqlParameter("@wdId", MySqlDbType.VarChar) { Value = withdraw.IdWithdraw });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data.IdWithdraw= Convert.ToInt32(reader["IdWithdraw"]);
                            data.Email = reader["Email"].ToString();
                            data.Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null;
                            data.Amount = (Decimal)reader["Amount"];
                            data.CreatedDate = (DateTime)reader["CreatedDate"];
                            data.Bank = reader["Bank"].ToString();
                            data.BankAccount = reader["BankAccount"].ToString();
                            data.RealName = reader["RealName"].ToString();
                            data.IFSCCode = reader["IFSCCode"].ToString();
                            data.UserId = (int)reader["UserId"];
                        }
                    }
                }
            }
            return data;
        }

        public Common UpdateWithdrawStatus(WithdrawVM vm)
        {
            Common common = new Common();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_UpdateWithdrawStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = vm.Email });
                    command.Parameters.Add(new MySqlParameter("@wdId", MySqlDbType.VarChar) { Value = vm.IdWithdraw });
                    command.Parameters.Add(new MySqlParameter("@wdStatus", MySqlDbType.VarChar) { Value = vm.Status });
                    command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    // Retrieve the output parameter values
                    common.StatusId = (int)command.Parameters["@StatusId"].Value;
                    common.Status = command.Parameters["@Status"].Value.ToString();
                }
            }
            return common;
        }

        public List<RechargeListVM> GetRechargeList(string Email)
        {
            List<RechargeListVM> lst = new List<RechargeListVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetRechargeHistoryList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RechargeListVM recharge = new RechargeListVM
                            {
                                IdHistory = Convert.ToInt32(reader["IdHistory"]),
                                Email = reader["Email"].ToString(),
                                Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null,
                                Amount = (Decimal)reader["Amount"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                Status = reader["Status"].ToString(),
                                TrnCode = reader["TrnCode"].ToString(),
                                UserId = (int)reader["UserId"]
                            };
                            lst.Add(recharge);
                        }
                    }
                }
            }
            return lst;
        }

        public List<AdminWithdrawVM> GetWithdrawList(string Email)
        {
            List<AdminWithdrawVM> lst = new List<AdminWithdrawVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetWithdrawList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = Email });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AdminWithdrawVM recharge = new AdminWithdrawVM
                            {
                                IdWithdraw = Convert.ToInt32(reader["IdWithdraw"]),
                                Email = reader["Email"].ToString(),
                                Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null,
                                Amount = (Decimal)reader["Amount"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                Status = reader["Status"].ToString(),
                                UserId = (int)reader["UserId"]
                            };
                            lst.Add(recharge);
                        }
                    }
                }
            }
            return lst;
        }

        public Common UpdateDailyIncome()
        {
            Common common = new Common();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("spAdmin_UpdateDailyIncome", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        common.StatusId = (int)command.Parameters["@StatusId"].Value;
                        common.Status = command.Parameters["@Status"].Value.ToString();
                    }
                }
                return common;
            }
            catch(Exception ex)
            {
                common.StatusId = 0;
                common.Status = ex.Message;
                return common;
            }
        }

        public List<User> GetActiveUserList()
        {
            List<User> lst = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetActiveList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User usr = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Email = reader["Email"].ToString(),
                                ProfilePic = reader["ProfilePic"] != DBNull.Value ? (byte[])reader["ProfilePic"] : null,
                                NickName = reader["NickName"] != DBNull.Value ? reader["NickName"].ToString() : "",
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            };
                            lst.Add(usr);
                        }
                    }
                }
            }
            return lst;
        }

        public List<User> GetInactiveUserList()
        {
            List<User> lst = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_GetInActiveList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User usr = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Email = reader["Email"].ToString(),
                                ProfilePic = reader["ProfilePic"] != DBNull.Value ? (byte[])reader["ProfilePic"] : null,
                                NickName = reader["NickName"] != DBNull.Value ? reader["NickName"].ToString() : "",
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            };
                            lst.Add(usr);
                        }
                    }
                }
            }
            return lst;
        }

        public Common UpdateUserStatus(User user)
        {
            Common common = new Common();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("spAdmin_UpdateUserStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@user_id", MySqlDbType.VarChar) { Value = user.Id });
                    command.Parameters.Add(new MySqlParameter("@active", MySqlDbType.VarChar) { Value = user.isActive });
                    command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    // Retrieve the output parameter values
                    common.StatusId = (int)command.Parameters["@StatusId"].Value;
                    common.Status = command.Parameters["@Status"].Value.ToString();
                }
            }
            return common;
        }
    }
}
