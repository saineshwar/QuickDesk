using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class UserMasterConcrete : IUserMaster
    {
        private readonly DatabaseContext _context;

        public UserMasterConcrete(DatabaseContext context)
        {
            _context = context;
        }


        public Usermaster GetUserById(long? userId)
        {
            try
            {
                return _context.Usermasters.Find(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? AddUser(Usermaster usermaster, string password, string salt, short? roleId)
        {
            try
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        long userId = -1;

                        if (usermaster != null)
                        {
                            usermaster.Status = true;
                            usermaster.CreateDate = DateTime.Now;
                            usermaster.IsFirstLogin = true;

                            _context.Usermasters.Add(usermaster);
                            _context.SaveChanges();
                            userId = usermaster.UserId;

                            PasswordMaster passwordMaster = new PasswordMaster()
                            {
                                UserId = userId,
                                CreateDate = DateTime.Now,
                                Password = password,
                                PasswordId = 0
                            };

                            _context.PasswordMaster.Add(passwordMaster);
                            _context.SaveChanges();

                            UserTokens userTokens = new UserTokens()
                            {
                                UserId = userId,
                                HashId = 0,
                                PasswordSalt = salt,
                                CreatedDate = DateTime.Now
                            };

                            var savedAssignedRoles = new SavedAssignedRoles()
                            {
                                RoleId = roleId,
                                UserId = userId,
                                AssignedRoleId = 0,
                                Status = true,
                                CreateDate = DateTime.Now
                            };
                            _context.SavedAssignedRoles.Add(savedAssignedRoles);
                            _context.SaveChanges();

                            _context.UserTokens.Add(userTokens);
                            _context.SaveChanges();
                            dbContextTransaction.Commit();
                            return userId;
                        }

                        return userId;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? UpdateUser(Usermaster usermaster)
        {
            try
            {

                long? result = -1;

                if (usermaster != null)
                {
                    usermaster.UpdateDate = DateTime.Now;
                    _context.Entry(usermaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = usermaster.UserId;
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteUser(int? userId)
        {
            try
            {
                Usermaster usermaster = _context.Usermasters.Find(userId);
                if (usermaster != null) _context.Usermasters.Remove(usermaster);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckUsernameExists(string username)
        {
            try
            {
                var result = (from menu in _context.Usermasters
                              where menu.UserName == username
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usermaster GetUserByUsername(string username)
        {
            try
            {
                var result = (from usermaster in _context.Usermasters
                              where usermaster.UserName == username
                              select usermaster).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserResponseViewModel> GetAutoCompleteUserName(string username, int roleId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Username", username);
                    param.Add("@RoleId", roleId);
                    return con.Query<UserResponseViewModel>("Usp_GetActiveUsers", param, null, false, 0,
                        CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetActiveUsersbyCategoryId(string username, string categoryId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Username", username);
                    param.Add("@CategoryId", categoryId);
                    return con.Query<UserResponseViewModel>("Usp_GetActiveUsersbyCategoryId", param, null, false, 0,
                        CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetAllUsersExceptSuperAdmin(string username)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Username", username);
                    return con.Query<UserResponseViewModel>("Usp_GetActiveUsersExceptSuperAdmin", param, null, false, 0,
                        CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetAllAgentsMembers(string username, long userId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Username", username);
                    param.Add("@UserId", userId);
                    return con.Query<UserResponseViewModel>("Usp_GetActiveUsersforAgent_Not_Agent_itself", param, null, false, 0,
                        CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserResponseViewModel> GetAllAgentsandAdminMembers(string username, long userId)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Username", username);
                    param.Add("@UserId", userId);
                    return con.Query<UserResponseViewModel>("Usp_GetActiveUsersforAgent_Not_Agent_itself", param, null, false, 0,
                        CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditUserViewModel EditUserbyUserId(long? userId)
        {
            try
            {
                using (SqlConnection con =
                       new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<EditUserViewModel>("Usp_GetUsersbyUserId", param, null, false, 0,
                        CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EditAgentViewModel EditAgentbyUserId(long? userId)
        {
            try
            {
                using (SqlConnection con =
                       new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    return con.Query<EditAgentViewModel>("Usp_GetAgentbyUserId", param, null, false, 0,
                        CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckUserIdExists(long userId)
        {
            try
            {
                var result = (from usermaster in _context.Usermasters
                              where usermaster.UserId == userId
                              select usermaster).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateUserDetails(EditUserViewModel editUserViewModel)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                var param = new DynamicParameters();
                param.Add("@FirstName", editUserViewModel.FirstName);
                param.Add("@UserName", editUserViewModel.UserName);
                param.Add("@LastName", editUserViewModel.LastName);
                param.Add("@EmailId", editUserViewModel.EmailId);
                param.Add("@MobileNo", editUserViewModel.MobileNo);
                param.Add("@Gender", editUserViewModel.Gender);
                param.Add("@UserId", editUserViewModel.UserId);
                param.Add("@Status", editUserViewModel.Status);

                var result = con.Execute("Usp_UpdateUsersbyUserId", param, transaction, 0, CommandType.StoredProcedure);

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

        public List<UsermasterViewModel> GetUserList(string username, int? roleid, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {

                    var roleIdofAgent = (from roles in db.RoleMasters
                                         where roles.RoleId == 4
                                         select roles.RoleId).FirstOrDefault();

                    var data = from usermaster in db.Usermasters

                               join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                               join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                               where roleMaster.RoleId != roleIdofAgent
                               select new UsermasterViewModel()
                               {
                                   UserId = usermaster.UserId,
                                   UserName = usermaster.UserName,
                                   FirstName = usermaster.FirstName,
                                   LastName = usermaster.LastName,
                                   EmailId = usermaster.EmailId,
                                   Gender = usermaster.Gender,
                                   Status = usermaster.Status,
                                   MobileNo = usermaster.MobileNo,
                                   RoleName = roleMaster.RoleName,
                                   RoleId = roleMaster.RoleId
                               };

                    IEnumerable<UsermasterViewModel> query = data.ToList();


                    if (!string.IsNullOrEmpty(username) && roleid != null)
                    {
                        query = query.Where(p => p.RoleId == roleid && p.UserName == username);
                    }
                    else if (!string.IsNullOrEmpty(username))
                    {
                        query = query.Where(p => p.UserName.Contains(username));
                    }
                    else if (roleid != null)
                    {
                        query = query.Where(p => p.RoleId == roleid);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("UserId ASC"))
                    {
                        query = query.OrderBy(p => p.UserId);
                    }
                    else if (sorting.Equals("UserId DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserId);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }

                    else if (sorting.Equals("FirstName ASC"))
                    {
                        query = query.OrderBy(p => p.FirstName);
                    }
                    else if (sorting.Equals("FirstName DESC"))
                    {
                        query = query.OrderByDescending(p => p.FirstName);
                    }

                    else if (sorting.Equals("LastName ASC"))
                    {
                        query = query.OrderBy(p => p.LastName);
                    }
                    else if (sorting.Equals("LastName DESC"))
                    {
                        query = query.OrderByDescending(p => p.LastName);
                    }

                    else if (sorting.Equals("EmailId ASC"))
                    {
                        query = query.OrderBy(p => p.EmailId);
                    }

                    else if (sorting.Equals("EmailId DESC"))
                    {
                        query = query.OrderByDescending(p => p.EmailId);
                    }

                    else if (sorting.Equals("MobileNo ASC"))
                    {
                        query = query.OrderBy(p => p.MobileNo);
                    }

                    else if (sorting.Equals("MobileNo DESC"))
                    {
                        query = query.OrderByDescending(p => p.MobileNo);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.UserId); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetUserCount(string username, int? roleid)
        {
            try
            {

                using (var db = new DatabaseContext())
                {
                    var roleIdofAgent = (from roles in db.RoleMasters
                                         where roles.RoleId == 4
                                         select roles.RoleId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(username) && roleid != null)
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId != roleIdofAgent && usermaster.UserName == username && roleMaster.RoleId == roleid
                                    select usermaster).Count();
                        return data;
                    }
                    else if (string.IsNullOrEmpty(username) && roleid != null)
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId != roleIdofAgent && roleMaster.RoleId == roleid
                                    select usermaster).Count();
                        return data;
                    }
                    else if (!string.IsNullOrEmpty(username))
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId != roleIdofAgent && usermaster.UserName == username
                                    select usermaster).Count();
                        return data;
                    }
                    else
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId != roleIdofAgent
                                    select usermaster).Count();
                        return data;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UsermasterViewModel> GetAgentList(string username, int? roleid, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {

                    var roleIdofAgent = (from roles in db.RoleMasters
                                         where roles.RoleId == 4
                                         select roles.RoleId).FirstOrDefault();

                    var data = from usermaster in db.Usermasters
                               join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                               join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                               where roleMaster.RoleId == roleIdofAgent
                               select new UsermasterViewModel()
                               {
                                   UserId = usermaster.UserId,
                                   UserName = usermaster.UserName,
                                   FirstName = usermaster.FirstName,
                                   LastName = usermaster.LastName,
                                   EmailId = usermaster.EmailId,
                                   Gender = usermaster.Gender,
                                   Status = usermaster.Status,
                                   MobileNo = usermaster.MobileNo,
                                   RoleName = roleMaster.RoleName,
                                   RoleId = roleMaster.RoleId
                               };

                    IEnumerable<UsermasterViewModel> query = data.ToList();


                    if (!string.IsNullOrEmpty(username) && roleid != null)
                    {
                        query = query.Where(p => p.RoleId == roleid && p.UserName == username);
                    }
                    else if (!string.IsNullOrEmpty(username))
                    {
                        query = query.Where(p => p.UserName.Contains(username));
                    }
                    else if (roleid != null)
                    {
                        query = query.Where(p => p.RoleId == roleid);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("UserId ASC"))
                    {
                        query = query.OrderBy(p => p.UserId);
                    }
                    else if (sorting.Equals("UserId DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserId);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }

                    else if (sorting.Equals("FirstName ASC"))
                    {
                        query = query.OrderBy(p => p.FirstName);
                    }
                    else if (sorting.Equals("FirstName DESC"))
                    {
                        query = query.OrderByDescending(p => p.FirstName);
                    }

                    else if (sorting.Equals("LastName ASC"))
                    {
                        query = query.OrderBy(p => p.LastName);
                    }
                    else if (sorting.Equals("LastName DESC"))
                    {
                        query = query.OrderByDescending(p => p.LastName);
                    }

                    else if (sorting.Equals("EmailId ASC"))
                    {
                        query = query.OrderBy(p => p.EmailId);
                    }

                    else if (sorting.Equals("EmailId DESC"))
                    {
                        query = query.OrderByDescending(p => p.EmailId);
                    }

                    else if (sorting.Equals("MobileNo ASC"))
                    {
                        query = query.OrderBy(p => p.MobileNo);
                    }

                    else if (sorting.Equals("MobileNo DESC"))
                    {
                        query = query.OrderByDescending(p => p.MobileNo);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.UserId); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAgentCount(string username, int? roleid)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var roleIdofAgent = (from roles in db.RoleMasters
                                         where roles.RoleId == 4
                                         select roles.RoleId).FirstOrDefault();


                    if (!string.IsNullOrEmpty(username) && roleid != null)
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId == roleIdofAgent && usermaster.UserName == username &&
                                          roleMaster.RoleId == roleid
                                    select usermaster).Count();

                        return data;
                    }
                    else if (string.IsNullOrEmpty(username) && roleid != null)
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId == roleIdofAgent &&
                                          roleMaster.RoleId == roleid
                                    select usermaster).Count();
                        return data;
                    }
                    else if (!string.IsNullOrEmpty(username))
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId == roleIdofAgent && usermaster.UserName == username
                                    select usermaster).Count();

                        return data;
                    }
                    else
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where roleMaster.RoleId == roleIdofAgent
                                    select usermaster).Count();

                        return data;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long? AddAgent(Usermaster usermaster, string password, SavedAssignedRoles savedAssignedRoles, int? categoryId, string salt)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {

                    var param = new DynamicParameters();
                    param.Add("@UserName", usermaster.UserName);
                    param.Add("@FirstName", usermaster.FirstName);
                    param.Add("@LastName", usermaster.LastName);
                    param.Add("@EmailId", usermaster.EmailId);
                    param.Add("@MobileNo", usermaster.MobileNo);
                    param.Add("@Gender", usermaster.Gender);
                    param.Add("@CreatedBy", usermaster.CreatedBy);
                    param.Add("@Status", usermaster.Status);
                    param.Add("@UserId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                    var resultUsermaster = con.Execute("Usp_CreateUser", param, transaction, 0, CommandType.StoredProcedure);
                    long userId = param.Get<Int64>("@UserId");

                    var parampassword = new DynamicParameters();
                    parampassword.Add("@Password", password);
                    parampassword.Add("@UserId", userId);
                    var resultpassword = con.Execute("Usp_InsertPassword", parampassword, transaction, 0, CommandType.StoredProcedure);

                    var paramsavedAssignedRoles = new DynamicParameters();
                    paramsavedAssignedRoles.Add("@RoleId", savedAssignedRoles.RoleId);
                    paramsavedAssignedRoles.Add("@Status", savedAssignedRoles.Status);
                    paramsavedAssignedRoles.Add("@UserId", userId);
                    var resultsavedAssignedRoles = con.Execute("Usp_InsertSavedAssignedRoles", paramsavedAssignedRoles, transaction, 0,
                        CommandType.StoredProcedure);

                    var paramAgentCategoryAssigned = new DynamicParameters();
                    paramAgentCategoryAssigned.Add("@CategoryId", categoryId);
                    paramAgentCategoryAssigned.Add("@UserId", userId);
                    var resultAgentCategoryAssigned = con.Execute("Usp_InsertAgentCategoryAssigned", paramAgentCategoryAssigned, transaction, 0,
                        CommandType.StoredProcedure);

                    var paramusertoken = new DynamicParameters();
                    paramusertoken.Add("@PasswordSalt", salt);
                    paramusertoken.Add("@UserId", userId);
                    var resultusertoken = con.Execute("Usp_InsertUserTokens", paramusertoken, transaction, 0, CommandType.StoredProcedure);

                    if (resultUsermaster > 0 && resultpassword > 0 && resultsavedAssignedRoles > 0 && resultAgentCategoryAssigned > 0 && resultusertoken > 0)
                    {
                        transaction.Commit();
                        return 1;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }

        }

        public int UpdateAgentDetails(EditAgentViewModel editAgentViewModel)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@FirstName", editAgentViewModel.FirstName);
                    param.Add("@UserName", editAgentViewModel.UserName);
                    param.Add("@LastName", editAgentViewModel.LastName);
                    param.Add("@EmailId", editAgentViewModel.EmailId);
                    param.Add("@MobileNo", editAgentViewModel.MobileNo);
                    param.Add("@Gender", editAgentViewModel.Gender);
                    param.Add("@UserId", editAgentViewModel.UserId);
                    param.Add("@Status", editAgentViewModel.Status);

                    var result = con.Execute("Usp_UpdateAgentbyUserId", param, transaction, 0, CommandType.StoredProcedure);

                    var paramdelete = new DynamicParameters();
                    paramdelete.Add("@UserId", editAgentViewModel.UserId);
                    var resultdelete = con.Execute("Usp_DeleteAssignedCategoryUserId", paramdelete, transaction, 0, CommandType.StoredProcedure);


                    var paramAgentCategoryAssigned = new DynamicParameters();
                    paramAgentCategoryAssigned.Add("@CategoryId", editAgentViewModel.CategoryId);
                    paramAgentCategoryAssigned.Add("@UserId", editAgentViewModel.UserId);
                    var resultAgentCategoryAssigned = con.Execute("Usp_InsertAgentCategoryAssigned", paramAgentCategoryAssigned, transaction, 0,
                        CommandType.StoredProcedure);


                    if (result > 0 && resultdelete > 0 && resultAgentCategoryAssigned > 0)
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public long GetUserIdbyEmailId(string emailid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@EmailId", emailid);
                    return con.Query<long>("Usp_GetUserIdbyEmailId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofAdmin()
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent

                var adminlist = (from usermaster in _context.Usermasters
                                 join savedroles in _context.SavedAssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && savedroles.RoleId == 3
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofAgents(short categoryId)
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent

                var adminlist = (from agentCategory in _context.AgentCategoryAssigned
                                 join usermaster in _context.Usermasters on agentCategory.UserId equals usermaster.UserId
                                 where agentCategory.CategoryId == categoryId
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofAgentsAdmin()
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent
                //4   AgentAdmin

                var adminlist = (from usermaster in _context.Usermasters
                                 join savedroles in _context.SavedAssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && savedroles.RoleId == 5
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetListofHod()
        {
            try
            {

                //RoleId RoleName
                //1   SuperAdmin
                //2   User
                //3   Admin
                //4   Agent
                //5   AgentAdmin
                //6   HOD

                var adminlist = (from usermaster in _context.Usermasters
                                 join savedroles in _context.SavedAssignedRoles on usermaster.UserId equals savedroles.UserId
                                 where usermaster.Status == true && savedroles.RoleId == 6
                                 select new SelectListItem()
                                 {
                                     Text = usermaster.FirstName + " " + usermaster.LastName,
                                     Value = usermaster.UserId.ToString()
                                 }).ToList();

                adminlist.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-----Select-----"
                });

                return adminlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EscalatedUserViewModel GetTicketEscalatedToUserNames(long? ticketId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@TicketId", ticketId);
                return con.Query<EscalatedUserViewModel>("Usp_GetTicketEscalatedToUserNames", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public bool CheckEmailIdExists(string emailid)
        {
            try
            {
                var result = (from menu in _context.Usermasters
                              where menu.EmailId == emailid
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMobileNoExists(string mobileno)
        {
            try
            {
                var result = (from menu in _context.Usermasters
                              where menu.MobileNo == mobileno
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UsermasterViewModel> GetOnlyUserList(string username, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {

                    var roleIdofUser = (from roles in db.RoleMasters
                                        where roles.RoleId == 2
                                        select roles.RoleId).FirstOrDefault();

                    var query = from usermaster in db.Usermasters

                                join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                where assignedroles.RoleId == roleIdofUser
                                select new UsermasterViewModel()
                                {
                                    UserId = usermaster.UserId,
                                    UserName = usermaster.UserName,
                                    FirstName = usermaster.FirstName,
                                    LastName = usermaster.LastName,
                                    EmailId = usermaster.EmailId,
                                    Gender = usermaster.Gender,
                                    Status = usermaster.Status,
                                    MobileNo = usermaster.MobileNo,
                                    RoleName = roleMaster.RoleName,
                                    RoleId = roleMaster.RoleId
                                };


                    if (!string.IsNullOrEmpty(username))
                    {
                        query = query.Where(p => p.UserName == username);
                    }
                    else if (!string.IsNullOrEmpty(username))
                    {
                        query = query.Where(p => p.UserName.Contains(username));
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("UserId ASC"))
                    {
                        query = query.OrderBy(p => p.UserId);
                    }
                    else if (sorting.Equals("UserId DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserId);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }
                    else if (sorting.Equals("FirstName ASC"))
                    {
                        query = query.OrderBy(p => p.FirstName);
                    }
                    else if (sorting.Equals("FirstName DESC"))
                    {
                        query = query.OrderByDescending(p => p.FirstName);
                    }
                    else if (sorting.Equals("LastName ASC"))
                    {
                        query = query.OrderBy(p => p.LastName);
                    }
                    else if (sorting.Equals("LastName DESC"))
                    {
                        query = query.OrderByDescending(p => p.LastName);
                    }


                    query = query.OrderByDescending(p => p.UserId); //Default!


                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetOnlyUserCount(string username)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var roleUserId = (from roles in db.RoleMasters
                                      where roles.RoleId == 2
                                      select roles.RoleId).FirstOrDefault();
                    if (!string.IsNullOrEmpty(username))
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where assignedroles.RoleId == roleUserId && usermaster.UserName == username
                                    select usermaster).Count();
                        return data;
                    }
                    else
                    {
                        var data = (from usermaster in db.Usermasters
                                    join assignedroles in db.SavedAssignedRoles on usermaster.UserId equals assignedroles.UserId
                                    join roleMaster in db.RoleMasters on assignedroles.RoleId equals roleMaster.RoleId
                                    where assignedroles.RoleId == roleUserId
                                    select usermaster).Count();
                        return data;
                    }



                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long UpdateChangePasswordFlag(long userId, string newpassword)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    long result = 0;
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var usermaster = (from usermasters in context.Usermasters
                                              where usermasters.UserId == userId
                                              select usermasters).FirstOrDefault();

                            if (usermaster != null)
                            {
                                usermaster.UpdateDate = DateTime.Now;
                                usermaster.IsFirstLogin = false;
                                context.Entry(usermaster).State = EntityState.Modified;
                                context.SaveChanges();
                            }

                            var password = (from temppassword in context.PasswordMaster
                                            where temppassword.UserId == userId
                                            select temppassword).FirstOrDefault();

                            if (password != null)
                            {
                                password.PasswordChangedDate = DateTime.Now;
                                password.Password = newpassword;
                                context.Entry(password).State = EntityState.Modified;
                                context.SaveChanges();
                            }

                            transaction.Commit();
                            result = 1;
                            return result;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserTokens GetUserSaltbyUserid(long userId)
        {
            var usertoken = (from tempuser in _context.UserTokens
                             where tempuser.UserId == userId
                             select tempuser).FirstOrDefault();

            return usertoken;
        }

        public long UpdateIsFirstLogin(long? userId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    var result = con.Execute("Usp_UpdateIsFirstLoginbyUserId", param, transaction, 0, CommandType.StoredProcedure);
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool CheckIsCategogryAssignedtoHod(long userId)
        {
            try
            {
                var result = (from category in _context.CategoryConfigration
                              where category.HodUserId == userId && category.Status == true
                              select category).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIsCategogryAssignedtoAgentAdmin(long userId)
        {
            try
            {
                var result = (from category in _context.CategoryConfigration
                              where category.AgentAdminUserId == userId && category.Status == true
                              select category).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}
