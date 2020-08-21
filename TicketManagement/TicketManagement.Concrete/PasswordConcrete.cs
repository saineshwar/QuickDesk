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
using TicketManagement.Models;

namespace TicketManagement.Concrete
{
    public class PasswordConcrete : IPassword
    {
        private readonly DatabaseContext _context;
        public PasswordConcrete(DatabaseContext context)
        {
            _context = context;
        }
       

        public string GetPasswordbyUserId(long userId)
        {
            try
            {
                var password = (from passwordmaster in _context.PasswordMaster
                                where passwordmaster.UserId == userId
                                select passwordmaster.Password).FirstOrDefault();

                return password;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdatePasswordandHistory(long userId, string passwordHash, string passwordSalt, string processType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@PasswordHash", passwordHash);
                    param.Add("@PasswordSalt", passwordSalt);
                    param.Add("@ProcessType", processType);
                    var result = con.Execute("Usp_PasswordMaster_UpdatePassword", param, transaction, 0, CommandType.StoredProcedure);

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
    }
}
