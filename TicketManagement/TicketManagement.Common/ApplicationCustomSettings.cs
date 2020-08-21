using System;
using System.Linq;
using TicketManagement.Concrete;
using TicketManagement.ViewModels;

namespace TicketManagement.Common
{
    public class ApplicationCustomSettings
    {
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
                            EnableEmailFeature = general.EnableEmailFeature,
                            EnableSignatureFeature = general.EnableSignatureFeature,
                            EnableSmsFeature = general.EnableSmsFeature,
                        }).FirstOrDefault();

                    return getsetting;
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }
    }
}