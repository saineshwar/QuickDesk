using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{

    public class ProfileConcrete : IProfile
    {

        public UsermasterEditView GetprofileById(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<UsermasterEditView>("Usp_Usermasters_GetAgentByUserId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserProfileView GetUserprofileById(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<UserProfileView>("Usp_UserProfileByUserId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateUserMasterDetails(long userId, UsermasterEditView usermasterEditView)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@FirstName", usermasterEditView.FirstName);
                    param.Add("@LastName", usermasterEditView.LastName);
                    param.Add("@EmailId", usermasterEditView.EmailId);
                    param.Add("@MobileNo", usermasterEditView.MobileNo);
                    param.Add("@Gender", usermasterEditView.Gender);
                    var result = con.Execute("Usp_Usermasters_UpdateUserMasterDetails", param, transaction, 0, CommandType.StoredProcedure);

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

        public void UpdateProfileImage(ProfileImage profileImage)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.ProfileImage.Add(profileImage);
                            var result = context.SaveChanges();

                            if (result != 0)
                            {
                                ProfileImageStatus profileImageStatus = new ProfileImageStatus
                                {
                                    UserId = profileImage.UserId,
                                    CreatedDate = DateTime.Now,
                                    Isuploaded = true,
                                    ProfileImageId = profileImage.ProfileImageId
                                };
                                context.ProfileImageStatus.Add(profileImageStatus);
                                context.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool IsProfileImageExists(long userId)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var data = (from pi in context.ProfileImageStatus
                                where pi.UserId == userId
                                select pi).Any();

                    return data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetProfileImageBase64String(long userId)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var data = (from pi in context.ProfileImage
                                where pi.UserId == userId
                                select pi.ProfileImageBase64String).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteProfileImage(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    var result = con.Execute("Usp_ProfileImage_DeleteProfileImage", param, transaction, 0, CommandType.StoredProcedure);

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

        public int UpdateSignature(Signatures signatures)
        {
            try
            {
                int result = -1;
                using (var context = new DatabaseContext())
                {
                    context.Signatures.Add(signatures);
                    result = context.SaveChanges();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSignatureAlreadyExists(long userId)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var signaturesExists = (from sign in context.Signatures
                                            where sign.UserId == userId
                                            select sign).Any();
                    return signaturesExists;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteSignature(long userId)
        {
            try
            {
                var result = -1;
                using (var context = new DatabaseContext())
                {
                    var signatures = (from sign in context.Signatures
                                      where sign.UserId == userId
                                      select sign).FirstOrDefault();

                    if (signatures != null) context.Signatures.Remove(signatures);
                    result = context.SaveChanges();

                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetSignature(long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<string>("Usp_Signatures_GetSignature", param, null, false, 0, CommandType.StoredProcedure)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public bool ValidatePassword(string existingPassword, long userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@Password", existingPassword);
                    return con.Query<bool>("Usp_PasswordMaster_ValidatePassword", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}
