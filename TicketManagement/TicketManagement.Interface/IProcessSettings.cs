using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IProcessSettings
    {
        int SaveSmtpSettings(SmtpEmailSettings ticketsViewModel);
        int GetSmtpCount(string search);
        List<SmtpEmailSettingsView> GetAllTicketsbyUserId(string search, int startIndex, int count, string sorting);
        SmtpEmailSettingsViewModel EditSmtpSettings(int? smtpProviderId);
        int UpdateSmtpSettings(SmtpEmailSettings smtpEmailSettings);
        int SettingDefaultConnection(int? smtpProviderId);
        SmtpEmailSettings GetDefaultEmailSettings();
        GeneralSettingsViewModel GetGeneralSetting();
        void InsertorUpdateGeneralSetting(GeneralSettings generalSettings);
    }
}
