using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class VerifyConcrete : IVerify
    {
        public int InsertEmailVerification(long? userId, string emailId, string verificationCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@EmailId", emailId);
                    param.Add("@VerificationCode", verificationCode);
                    var result = con.Execute("Usp_InsertEmailVerification", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckVerificationCodeExists(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<bool>("Usp_CheckVerificationCodeExists", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatedVerificationCode(string verificationCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@VerificationCode", verificationCode);
                    param.Add("@IsVerified", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    var result = con.Execute("Usp_UpdatedVerificationCode", param, transaction, 0, CommandType.StoredProcedure);
                    bool isVerified = param.Get<bool>("@IsVerified");

                    if (result > 0)
                    {
                        transaction.Commit();
                        return isVerified;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidateEmailIdExists(string emailid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@EmailId", emailid);
                    return con.Query<bool>("Usp_ValidateEmailIdExists", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertForgotPasswordVerification(string emailId, string verificationCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@EmailId", emailId);
                    param.Add("@VerificationCode", verificationCode);
                    param.Add("@EmailId", emailId);
                    param.Add("@EmailId", emailId);
                    param.Add("@EmailId", emailId);
                    var result = con.Execute("Usp_InsertForgotPasswordVerification", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetForgotPasswordToken(string emailid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@EmailId", emailid);
                    return con.Query<string>("Usp_GetFogotPasswordVerificationTokenbyEmailId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateFogotPasswordVerification(string verificationCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@VerificationCode", verificationCode);
                    var result = con.Execute("Usp_UpdateFogotPasswordVerification", param, transaction, 0, CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckEmailIdIsAlreadyVerified(string emailid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@EmailId", emailid);
                    return con.Query<bool>("Usp_CheckEmailIdIsAlreadyVerified", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
