
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class ProcessSettingsConcrete : IProcessSettings
    {
        public int SaveSmtpSettings(SmtpEmailSettings smtpEmailSettings)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    context.SmtpEmailSettings.Add(smtpEmailSettings);
                    return context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SmtpEmailSettingsView> GetAllTicketsbyUserId(string search, int startIndex, int count, string sorting)
        {
            using (var context = new DatabaseContext())
            {
                IQueryable<SmtpEmailSettingsView> query;

                if (!string.IsNullOrEmpty(search))
                {
                    query = (from stmp in context.SmtpEmailSettings
                             where stmp.Name == search.Trim()
                             select new SmtpEmailSettingsView()
                             {
                                 Timeout = stmp.Timeout,
                                 Port = stmp.Port,
                                 TlSProtocol = stmp.TlSProtocol,
                                 SslProtocol = stmp.SslProtocol,
                                 Username = stmp.Username,
                                 SmtpProviderId = stmp.SmtpProviderId,
                                 Host = stmp.Host,
                                 CreatedDate = stmp.CreatedDate,
                                 Name = stmp.Name,
                                 IsDefault = stmp.IsDefault,
                                 Password = stmp.Password
                             });
                }
                else
                {
                    query = (from stmp in context.SmtpEmailSettings

                             select new SmtpEmailSettingsView()
                             {
                                 Timeout = stmp.Timeout,
                                 Port = stmp.Port,
                                 TlSProtocol = stmp.TlSProtocol,
                                 SslProtocol = stmp.SslProtocol,
                                 Username = stmp.Username,
                                 SmtpProviderId = stmp.SmtpProviderId,
                                 Host = stmp.Host,
                                 CreatedDate = stmp.CreatedDate,
                                 Name = stmp.Name,
                                 IsDefault = stmp.IsDefault,
                                 Password = stmp.Password
                             });
                }


                if (string.IsNullOrEmpty(sorting) || sorting.Equals("Name ASC"))
                {
                    query = query.OrderBy(p => p.Name);
                }
                else if (sorting.Equals("Name DESC"))
                {
                    query = query.OrderByDescending(p => p.Name);
                }

                return count > 0
                    ? query.Skip(startIndex).Take(count).ToList()  //Paging
                    : query.ToList(); //No paging

            }


        }

        public int GetSmtpCount(string search)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@search", search);
                    return con.Query<int>("Usp_SMTPEmailSettings_GetCount", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public SmtpEmailSettingsViewModel EditSmtpSettings(int? smtpProviderId)
        {
           
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                    {
                        var param = new DynamicParameters();
                        param.Add("@SmtpProviderId", smtpProviderId);
                        return con.Query<SmtpEmailSettingsViewModel>("Usp_GetSMTPEmailSettingsById", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            
        }

        public int UpdateSmtpSettings(SmtpEmailSettings smtpEmailSettings)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var update = context.SmtpEmailSettings.Find(smtpEmailSettings.SmtpProviderId);

                    if (update != null)
                    {
                        update.Host = smtpEmailSettings.Host;
                        update.Port = smtpEmailSettings.Port;
                        update.Timeout = smtpEmailSettings.Timeout;
                        update.SslProtocol = smtpEmailSettings.SslProtocol;
                        update.TlSProtocol = smtpEmailSettings.TlSProtocol;
                        update.Username = smtpEmailSettings.Username;
                        update.Password = smtpEmailSettings.Password;
                        update.Status = smtpEmailSettings.Status;
                        context.Entry(update).State = System.Data.Entity.EntityState.Modified;
                    }

                    return context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int SettingDefaultConnection(int? smtpProviderId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@SmtpProviderId", smtpProviderId);
                    var result = con.Execute("Usp_SMTPEmailSettings_SetDefault", param, null, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return result;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SmtpEmailSettings GetDefaultEmailSettings()
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var getdefaultsetting = (from smtp in context.SmtpEmailSettings
                                             where smtp.IsDefault == true
                                             select smtp).FirstOrDefault();

                    return getdefaultsetting;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GeneralSettingsViewModel GetGeneralSetting()
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var getsetting = (from general in context.GeneralSettings
                                      select new GeneralSettingsViewModel()
                                      {
                                          Name = general.Name,
                                          Email = general.Email,
                                          GeneralSettingsId = general.GeneralSettingsId,
                                          SupportEmailId = general.SupportEmailId,
                                          WebsiteTitle = general.WebsiteTitle,
                                          WebsiteUrl = general.WebsiteUrl,
                                          EnableSmsFeature = general.EnableSmsFeature,
                                          EnableSignatureFeature = general.EnableSignatureFeature,
                                          EnableEmailFeature = general.EnableEmailFeature
                                      }).FirstOrDefault();

                    return getsetting;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void InsertorUpdateGeneralSetting(GeneralSettings generalSettings)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    if (generalSettings.GeneralSettingsId == null)
                    {
                        context.GeneralSettings.Add(generalSettings);
                        context.SaveChanges();
                    }
                    else
                    {
                        var update = context.GeneralSettings.Find(generalSettings.GeneralSettingsId);

                        if (update != null)
                        {
                            update.Email = generalSettings.Email;
                            update.Name = generalSettings.Name;
                            update.SupportEmailId = generalSettings.SupportEmailId;
                            update.WebsiteTitle = generalSettings.WebsiteTitle;
                            update.WebsiteUrl = generalSettings.WebsiteUrl;
                            update.EnableEmailFeature = generalSettings.EnableEmailFeature;
                            update.EnableSmsFeature = generalSettings.EnableSmsFeature;
                            update.EnableSignatureFeature = generalSettings.EnableSignatureFeature;
                            context.Entry(update).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
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
