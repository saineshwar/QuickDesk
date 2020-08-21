using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class VerificationConcrete : IVerification
    {
        private readonly DatabaseContext _databaseContext;
        public VerificationConcrete(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Here we can Send VerificationToken to User In Email or OTP On Mobile
        /// </summary>
        public void SendRegistrationVerificationToken(long? userid, string verficationToken)
        {
            RegisterVerification registerVerification = new RegisterVerification()
            {
                RegisterVerificationId = 0,
                GeneratedDate = DateTime.Now,
                GeneratedToken = verficationToken,
                UserId = userid,
                Status = true,
                VerificationStatus = false
            };

            _databaseContext.RegisterVerification.Add(registerVerification);
            _databaseContext.SaveChanges();
        }

        public RegisterVerification GetRegistrationGeneratedToken(string userid)
        {
            var tempuserid = Convert.ToInt64(userid);

            var registerVerification = (from rv in _databaseContext.RegisterVerification
                                        orderby rv.RegisterVerificationId descending
                                        where rv.UserId == tempuserid
                                        select rv).FirstOrDefault();

            return registerVerification;
        }

        public ResetPasswordVerification GetResetGeneratedToken(string userid)
        {
            var tempuserid = Convert.ToInt64(userid);

            var resetPasswordVerification = (from rv in _databaseContext.ResetPasswordVerification
                                             orderby rv.ResetTokenId descending
                                             where rv.UserId == tempuserid
                                             select rv).FirstOrDefault();

            return resetPasswordVerification;
        }

        public bool UpdateRegisterVerification(long userid)
        {
            var value = 0;
            using (var entities = new DatabaseContext())
            {
                var registerVerification = (from rv in entities.RegisterVerification
                                            where rv.UserId == userid
                                            orderby rv.RegisterVerificationId descending
                                            select rv).FirstOrDefault();

                if (registerVerification != null)
                {
                    registerVerification.VerificationStatus = true;
                    registerVerification.VerificationDate = DateTime.Now;
                    entities.Entry(registerVerification).State = EntityState.Modified;
                    value = entities.SaveChanges();
                }

                return value > 0 ? true : false;
            }
        }

        public bool CheckIsAlreadyVerifiedRegistration(long userid)
        {
            var registerVerification = (from rv in _databaseContext.RegisterVerification
                                        where rv.UserId == userid && rv.VerificationStatus == true
                                        select rv).Any();

            return registerVerification;
        }

        public bool CheckIsEmailVerifiedRegistration(long userid)
        {
            var registerVerification = (from rv in _databaseContext.RegisterVerification
                                        where rv.UserId == userid
                                        select rv.VerificationStatus).FirstOrDefault();

            return registerVerification;
        }

        public void SendResetVerificationToken(long userid, string verficationToken)
        {
            ResetPasswordVerification registerVerification = new ResetPasswordVerification()
            {
                ResetTokenId = 0,
                GeneratedDate = DateTime.Now,
                GeneratedToken = verficationToken,
                UserId = userid,
                Status = true,
                VerificationStatus = false
            };

            _databaseContext.ResetPasswordVerification.Add(registerVerification);
            _databaseContext.SaveChanges();
        }

        public bool UpdateResetVerification(long userid)
        {
            var resetVerification = (from rv in _databaseContext.ResetPasswordVerification
                                     where rv.UserId == userid
                                     orderby rv.ResetTokenId descending
                                     select rv).FirstOrDefault();
            if (resetVerification != null)
            {
                resetVerification.VerificationStatus = true;
                resetVerification.VerificationDate = DateTime.Now;
                _databaseContext.Entry(resetVerification).State = EntityState.Modified;
                _databaseContext.SaveChanges();
            }

            return _databaseContext.SaveChanges() > 0;
        }

        public int GetSentResetPasswordVerificationCount(long? userid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userid);
                    return con.Query<int>("Usp_GetSentResetPasswordVerificationCount", param, null, false, 0,
                        CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
