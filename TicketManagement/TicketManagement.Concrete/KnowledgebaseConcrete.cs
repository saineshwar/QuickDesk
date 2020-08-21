using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class KnowledgebaseConcrete : IKnowledgebase
    {
        public int AddKnowledgebase(
            Knowledgebase knowledgebase,
            List<KnowledgebaseAttachments> listknowledgebaseAttachments,
            KnowledgebaseDetails knowledgebaseDetails,
            List<KnowledgebaseAttachmentsDetails> knowledgebaseAttachmentsDetails
            )
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {

                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseTypeId", knowledgebase.KnowledgebaseTypeId);
                    param.Add("@Subject", knowledgebase.Subject);
                    param.Add("@Status", knowledgebase.Status);
                    param.Add("@UserId", knowledgebase.UserId);
                    param.Add("@CategoryId", knowledgebase.CategoryId);
                    param.Add("@KnowledgebaseId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                    var resultknowledgebase = con.Execute("Usp_InsertKnowledgebase", param, transaction, 0, CommandType.StoredProcedure);
                    long knowledgebaseId = param.Get<Int64>("@KnowledgebaseId");

                    var paramknowledgebaseDetails = new DynamicParameters();
                    paramknowledgebaseDetails.Add("@Contents", knowledgebaseDetails.Contents);
                    paramknowledgebaseDetails.Add("@Keywords", knowledgebaseDetails.Keywords);
                    paramknowledgebaseDetails.Add("@KnowledgebaseId", knowledgebaseId);
                    var resultknowledgebaseDetails = con.Execute("Usp_InsertKnowledgebaseDetails", paramknowledgebaseDetails, transaction, 0, CommandType.StoredProcedure);

                    for (var index = 0; index < listknowledgebaseAttachments.Count; index++)
                    {
                        var listkb = listknowledgebaseAttachments[index];
                        var paramknowledgebaseAttachment = new DynamicParameters();
                        paramknowledgebaseAttachment.Add("@AttachmentsName", listkb.AttachmentsName);
                        paramknowledgebaseAttachment.Add("@AttachmentsType", listkb.AttachmentsType);
                        paramknowledgebaseAttachment.Add("@KnowledgebaseId", knowledgebaseId);
                        paramknowledgebaseAttachment.Add("@KnowledgebaseAttachmentsId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                        con.Execute("Usp_InsertKnowledgebaseAttachments", paramknowledgebaseAttachment, transaction, 0, CommandType.StoredProcedure);
                        long knowledgebaseAttachmentsId = paramknowledgebaseAttachment.Get<Int64>("@KnowledgebaseAttachmentsId");

                        var listofattachmentbytes = knowledgebaseAttachmentsDetails[index];
                        var paramKnowledgebaseAttachmentsDetails = new DynamicParameters();
                        paramKnowledgebaseAttachmentsDetails.Add("@AttachmentBytes", listofattachmentbytes.AttachmentBytes);
                        paramKnowledgebaseAttachmentsDetails.Add("@KnowledgebaseId", knowledgebaseId);
                        paramKnowledgebaseAttachmentsDetails.Add("@KnowledgebaseAttachmentsId", knowledgebaseAttachmentsId);

                        con.Execute("Usp_InsertKnowledgebaseAttachmentsDetails", paramKnowledgebaseAttachmentsDetails, transaction, 0, CommandType.StoredProcedure);

                    }

                    if (resultknowledgebase > 0 && resultknowledgebaseDetails > 0)
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

        public int UpdateKnowledgebase(
       Knowledgebase knowledgebase,
       KnowledgebaseDetails knowledgebaseDetails,
       List<KnowledgebaseAttachments> listknowledgebaseAttachments,
       List<KnowledgebaseAttachmentsDetails> knowledgebaseAttachmentsDetails
       )
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {

                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseTypeId", knowledgebase.KnowledgebaseTypeId);
                    param.Add("@Subject", knowledgebase.Subject);
                    param.Add("@Status", knowledgebase.Status);
                    param.Add("@UserId", knowledgebase.UserId);
                    param.Add("@CategoryId", knowledgebase.CategoryId);
                    param.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                    var resultknowledgebase = con.Execute("Usp_UpdateKnowledgebase", param, transaction, 0, CommandType.StoredProcedure);

                    var paramknowledgebaseDetails = new DynamicParameters();
                    paramknowledgebaseDetails.Add("@Contents", knowledgebaseDetails.Contents);
                    paramknowledgebaseDetails.Add("@Keywords", knowledgebaseDetails.Keywords);
                    paramknowledgebaseDetails.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                    var resultknowledgebaseDetails = con.Execute("Usp_UpdateKnowledgebaseDetails", paramknowledgebaseDetails, transaction, 0, CommandType.StoredProcedure);

                    if (listknowledgebaseAttachments != null)
                    {
                        for (var index = 0; index < listknowledgebaseAttachments.Count; index++)
                        {
                            var listkb = listknowledgebaseAttachments[index];
                            var paramknowledgebaseAttachment = new DynamicParameters();
                            paramknowledgebaseAttachment.Add("@AttachmentsName", listkb.AttachmentsName);
                            paramknowledgebaseAttachment.Add("@AttachmentsType", listkb.AttachmentsType);
                            paramknowledgebaseAttachment.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                            paramknowledgebaseAttachment.Add("@KnowledgebaseAttachmentsId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                            con.Execute("Usp_InsertKnowledgebaseAttachments", paramknowledgebaseAttachment, transaction, 0, CommandType.StoredProcedure);
                            long knowledgebaseAttachmentsId = paramknowledgebaseAttachment.Get<Int64>("@KnowledgebaseAttachmentsId");

                            var listofattachmentbytes = knowledgebaseAttachmentsDetails[index];
                            var paramKnowledgebaseAttachmentsDetails = new DynamicParameters();
                            paramKnowledgebaseAttachmentsDetails.Add("@AttachmentBytes", listofattachmentbytes.AttachmentBytes);
                            paramKnowledgebaseAttachmentsDetails.Add("@KnowledgebaseId", knowledgebase.KnowledgebaseId);
                            paramKnowledgebaseAttachmentsDetails.Add("@KnowledgebaseAttachmentsId", knowledgebaseAttachmentsId);

                            var resultKnowledgebaseAttachmentsDetails = con.Execute("Usp_InsertKnowledgebaseAttachmentsDetails", paramKnowledgebaseAttachmentsDetails, transaction, 0, CommandType.StoredProcedure);
                        }
                    }


                    if (resultknowledgebase > 0 && resultknowledgebaseDetails > 0)
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

        public List<SelectListItem> KnowledgebaseTypeList()
        {
            try
            {
                List<SelectListItem> knowledgebaseTypeList;
                string key = "KnowledgebaseType_Cache";
                if (!CacheHelper.CheckExists(key))
                {
                    using (SqlConnection con =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                    {
                        con.Open();
                        knowledgebaseTypeList = con.Query<SelectListItem>("Usp_GetKnowledgebaseType", null, null, false, 0,
                            CommandType.StoredProcedure).ToList();

                        knowledgebaseTypeList.Insert(0, new SelectListItem()
                        {
                            Value = "",
                            Text = "-----Select-----"
                        });
                    }
                    CacheHelper.AddToCacheWithNoExpiration(key, knowledgebaseTypeList);
                }
                else
                {
                    knowledgebaseTypeList =  (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return knowledgebaseTypeList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetKnowledgebaseCount()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var data = (from knowledgebase in db.Knowledgebase
                                select knowledgebase).Count();
                    return data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<KnowledgebaseModel> GetKnowledgebaseList(string subject, int? categoryId, int startIndex, int count, string sorting)
        {
            try
            {
                using (var db = new DatabaseContext())
                {

                    var query = (from knowledgebase in db.Knowledgebase
                                 join category in db.Category on knowledgebase.CategoryId equals category.CategoryId
                                 join knowledgebaseType in db.KnowledgebaseType on knowledgebase.KnowledgebaseTypeId equals knowledgebaseType.KnowledgebaseTypeId
                                 select new KnowledgebaseModel()
                                 {
                                     Subject = knowledgebase.Subject,
                                     KnowledgebaseId = knowledgebase.KnowledgebaseId,
                                     KnowledgebaseTypeId = knowledgebase.KnowledgebaseTypeId,
                                     Status = knowledgebase.Status,
                                     CreateDate = knowledgebase.CreateDate,
                                     CategoryName = category.CategoryName,
                                     KnowledgebaseTypeName = knowledgebaseType.KnowledgebaseTypeName,
                                     CategoryId = knowledgebase.CategoryId
                                 }).AsQueryable();

                    if (!string.IsNullOrEmpty(subject) && categoryId != null)
                    {
                        query = query.Where(p => p.Subject.Contains(subject) && p.CategoryId == categoryId);
                    }
                    else if (!string.IsNullOrEmpty(subject))
                    {
                        query = query.Where(p => p.Subject.Contains(subject));
                    }
                    else if (categoryId != null)
                    {
                        query = query.Where(p => p.CategoryId == categoryId);
                    }

                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("KnowledgebaseId ASC"))
                    {
                        query = query.OrderBy(p => p.KnowledgebaseId);
                    }
                    else if (sorting.Equals("KnowledgebaseId DESC"))
                    {
                        query = query.OrderByDescending(p => p.KnowledgebaseId);
                    }
                    else if (sorting.Equals("Subject ASC"))
                    {
                        query = query.OrderBy(p => p.Subject);
                    }
                    else if (sorting.Equals("Subject DESC"))
                    {
                        query = query.OrderByDescending(p => p.Subject);
                    }
                    else if (sorting.Equals("Status ASC"))
                    {
                        query = query.OrderBy(p => p.Status);
                    }
                    else if (sorting.Equals("Status DESC"))
                    {
                        query = query.OrderByDescending(p => p.Status);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.KnowledgebaseId); //Default!
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
        public EditKnowledgebaseViewModel GetKnowledgebasebyKnowledgebaseId(long knowledgebaseId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseId", knowledgebaseId);
                    return con.Query<EditKnowledgebaseViewModel>("Usp_GetKnowledgebasebyKnowledgebaseId", param, null, false, 0, CommandType.StoredProcedure)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KnowledgebaseAttachments> GetKnowledgebaseAttachments(long? knowledgebaseId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseId", knowledgebaseId);
                    return con.Query<KnowledgebaseAttachments>("Usp_GetKnowledgebaseAttachmentsByKnowledgebaseId", param, null, false, 0, CommandType.StoredProcedure)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetKnowledgebaseAttachmentsCount(long? knowledgebaseId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseId", knowledgebaseId);
                    return con.Query<int>("Usp_GetKnowledgebaseAttachmentsCountByKnowledgebaseId", param, null, false, 0, CommandType.StoredProcedure)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public KnowledgebaseAttachmentsDownload GetAttachments(long? knowledgebaseAttachmentsId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseAttachmentsId", knowledgebaseAttachmentsId);
                    return con.Query<KnowledgebaseAttachmentsDownload>("Usp_GetKnowledgebaseAttachmentsbyAttachmentsId", param, null, false, 0, CommandType.StoredProcedure)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int DeleteAttachments(long? knowledgebaseAttachmentsId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseAttachmentsId", knowledgebaseAttachmentsId);
                    return con.Execute("Usp_DeleteKnowledgebaseAttachmentsbyAttachmentsId", param, null, 0, CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public KnowledgebaseArticleViewModel GetKnowledgebaseDetailsForArticle(long knowledgebaseId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@KnowledgebaseId", knowledgebaseId);
                    return con.Query<KnowledgebaseArticleViewModel>("Usp_GetKnowledgebaseDetailsForArticle", param, null, false, 0, CommandType.StoredProcedure)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KnowledgeSearch> SearchKnowledgebase(string search)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@search", search);
                    return con.Query<KnowledgeSearch>("Usp_SearchKnowledgebase", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AllKnowledgebase> AllKnowledgebase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    return con.Query<AllKnowledgebase>("Usp_GetAllKnowledgebase", null, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<KnowledgeSearch> SearchKnowledgebasebyCategoryId(string categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", categoryId);
                    return con.Query<KnowledgeSearch>("Usp_SearchKnowledgebasebyCategoryId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
