using System.Data.Entity;
using System.Diagnostics;
using TicketManagement.Models;

namespace TicketManagement.Concrete
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseConnection")
        {
            Database.SetInitializer<DatabaseContext>(null);
            Database.Log = (query) => Debug.Write(query);

        }
        
        public DbSet<MenuCategory> MenuCategory { get; set; }
        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<SubMenuMaster> SubMenuMasters { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<Usermaster> Usermasters { get; set; }
        
        public DbSet<PasswordMaster> PasswordMaster { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<SavedAssignedRoles> SavedAssignedRoles { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketDetails> TicketDetails { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<AttachmentDetails> AttachmentDetails { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TicketReply> TicketReply { get; set; }
        public DbSet<TicketReplyDetails> TicketReplyDetails { get; set; }   
        public DbSet<ReplyAttachment> ReplyAttachment { get; set; }
        public DbSet<ReplyAttachmentDetails> ReplyAttachmentDetails { get; set; }
        public DbSet<AgentCheckInStatusSummary> AgentCheckInStatusSummary { get; set; }
        public DbSet<AgentCheckInStatusDetails> AgentCheckInStatusDetails { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<ProfileImage> ProfileImage { get; set; }
        public DbSet<ProfileImageStatus> ProfileImageStatus { get; set; }
        public DbSet<Signatures> Signatures { get; set; }
        public DbSet<SmtpEmailSettings> SmtpEmailSettings { get; set; }
        public DbSet<TicketDeleteLockStatus> TicketDeleteLockStatus { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<AgentCategoryAssigned> AgentCategoryAssigned { get; set; }
        public DbSet<Knowledgebase> Knowledgebase { get; set; }
        public DbSet<KnowledgebaseAttachments> KnowledgebaseAttachments { get; set; }
        public DbSet<KnowledgebaseDetails> KnowledgebaseDetails { get; set; }
        public DbSet<KnowledgebaseType> KnowledgebaseType { get; set; }
        public DbSet<HolidayList> HolidayList { get; set; }
        public DbSet<BusinessHoursType> BusinessHoursType { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<BusinessHoursDetails> BusinessHoursDetails { get; set; }
        public DbSet<SlaPolicies> SlaPolicies { get; set; }
        public DbSet<CategoryConfigration> CategoryConfigration { get; set; }
        public DbSet<DefaultTicketSettings> DefaultTicketSettings { get; set; }
        public DbSet<TicketEscalationHistory> TicketEscalationHistory { get; set; }
        public DbSet<RegisterVerification> RegisterVerification { get; set; }
        public DbSet<ResetPasswordVerification> ResetPasswordVerification { get; set; }
        public DbSet<OverdueTypes> OverdueTypes { get; set; }
    }
}