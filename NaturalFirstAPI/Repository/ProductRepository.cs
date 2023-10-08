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
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("myConnectionString");
        }

        public List<Product> GetAllProducts()
        {
            List<Product> prd = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetProductList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add(new MySqlParameter("@productId", MySqlDbType.Int32) { Value = prd});

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product _prd = new Product();
                            _prd.IdProducts = Convert.ToInt32(reader["IdProducts"]);
                            _prd.ProductName = reader["ProductName"].ToString();
                            _prd.Cycle = Convert.ToInt32(reader["Cycle"]);
                            _prd.ProductImage = reader["ProductImage"] != DBNull.Value ? (byte[])reader["ProductImage"] : null;
                            _prd.IncomePerDay = (Decimal)reader["IncomePerDay"];
                            _prd.InvestAmt = (Decimal)reader["InvestAmt"];
                            _prd.TotalAmt = (Decimal)reader["TotalAmt"];
                            prd.Add(_prd);
                        }
                    }
                }
            }
            return prd;
        }

        public Product GetProduct(int IdProducts)
        {
            Product _prd = new Product();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetProductById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@productId", MySqlDbType.Int32) { Value = IdProducts });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _prd.IdProducts = Convert.ToInt32(reader["IdProducts"]);
                            _prd.ProductName = reader["ProductName"].ToString();
                            _prd.Cycle = Convert.ToInt32(reader["Cycle"]);
                            _prd.ProductImage = reader["ProductImage"] != DBNull.Value ? (byte[])reader["ProductImage"] : null;
                            _prd.IncomePerDay = (Decimal)reader["IncomePerDay"];
                            _prd.InvestAmt = (Decimal)reader["InvestAmt"];
                            _prd.TotalAmt = (Decimal)reader["TotalAmt"];
                            _prd.Description = reader["Description"].ToString();
                        }
                    }
                }
            }
            return _prd;
        }

        public Common PurchaseProduct(PurchaseVM product)
        {
            Common common = new Common();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_InsertPurchaseProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = product.Email });
                        command.Parameters.Add(new MySqlParameter("@userProductId", MySqlDbType.Int32) { Value = product.IdProducts });

                        // Add output parameters to the command
                        command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = Convert.ToInt32(command.Parameters["@StatusId"].Value);
                        string status = command.Parameters["@Status"].Value.ToString();

                        common.StatusId = statusId;
                        common.Status = status;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }

        public Common AddNewProduct(ProductVM prd)
        {
            Common common = new Common();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Execute SQL queries here
                    // For example, you can use a MySqlCommand to execute queries:
                    using (MySqlCommand command = new MySqlCommand("sp_AddNewProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new MySqlParameter("@userEmail", MySqlDbType.VarChar) { Value = prd.email });
                        command.Parameters.Add(new MySqlParameter("@prdCycle", MySqlDbType.Int32) { Value = prd.Cycle });
                        command.Parameters.Add(new MySqlParameter("@prdDescription", MySqlDbType.VarChar) { Value = prd.Description });
                        command.Parameters.Add(new MySqlParameter("@prdProductName", MySqlDbType.VarChar) { Value = prd.ProductName });
                        command.Parameters.Add(new MySqlParameter("@prdIncome", MySqlDbType.Decimal) { Value = prd.IncomePerDay });
                        command.Parameters.Add(new MySqlParameter("@prdInvestAmt", MySqlDbType.Decimal) { Value = prd.InvestAmt });
                        command.Parameters.Add(new MySqlParameter("@prdTotal", MySqlDbType.Decimal) { Value = prd.TotalAmt });
                        command.Parameters.Add(new MySqlParameter("@prdImage", MySqlDbType.LongBlob) { Value = prd.ProductImage });

                        // Add output parameters to the command
                        command.Parameters.Add("@StatusId", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Status", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        int statusId = Convert.ToInt32(command.Parameters["@StatusId"].Value);
                        string status = command.Parameters["@Status"].Value.ToString();

                        common.StatusId = statusId;
                        common.Status = status;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return common;
        }

        public List<Product> GetTeamProduct(User user)
        {
            List<Product> prd = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("sp_GetTeamProductById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@user_id", MySqlDbType.Int32) { Value = user.Id });

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product _prd = new Product();
                            _prd.IdProducts = Convert.ToInt32(reader["IdProducts"]);
                            _prd.ProductName = reader["ProductName"].ToString();
                            _prd.Cycle = Convert.ToInt32(reader["Cycle"]);
                            _prd.ProductImage = reader["ProductImage"] != DBNull.Value ? (byte[])reader["ProductImage"] : null;
                            _prd.IncomePerDay = (Decimal)reader["IncomePerDay"];
                            _prd.InvestAmt = (Decimal)reader["InvestAmt"];
                            _prd.TotalAmt = (Decimal)reader["TotalAmt"];
                            prd.Add(_prd);
                        }
                    }
                }
            }
            return prd;
        }
    }
}
