using System;
using TicketManagement.Concrete;
using TicketManagement.Interface;
using Unity;

namespace TicketManagement
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            
            container.RegisterType<IMenuCategory, MenuCategoryConcrete>();
            container.RegisterType<IMenu, MenuConcrete>();
            container.RegisterType<ISubMenu, SubMenuConcrete>();
            container.RegisterType<IRole, RoleConcrete>();
            container.RegisterType<IUserMaster, UserMasterConcrete>();
            container.RegisterType<IPassword, PasswordConcrete>();
            container.RegisterType<ISavedAssignedRoles, SavedAssignedRolesConcrete>();

            container.RegisterType<ICategory, CategoryConcrete>();
            container.RegisterType<IPriority, PriorityConcrete>();
            container.RegisterType<ITickets, TicketsConcrete>();
            container.RegisterType<IDisplayTickets, DisplayTicketsConcrete>();
            container.RegisterType<IStatus, StatusConcrete>();
            container.RegisterType<ITicketsReply, TicketsReplyConcrete>();
            container.RegisterType<IAttachments, AttachmentsConcrete>();
            container.RegisterType<IAgentCheckInStatus, AgentCheckInStatusConcrete>();
            container.RegisterType<ITicketHistory, TicketHistoryConcrete>();
            container.RegisterType<IProfile, ProfileConcrete>();
            container.RegisterType<IDashboardTicketCount, DashboardTicketCountConcrete>();
            container.RegisterType<IProcessSettings, ProcessSettingsConcrete>();
            container.RegisterType<ISendingEmail, SendingEmailConcrete>();
            container.RegisterType<IKnowledgebase, KnowledgebaseConcrete>();
            container.RegisterType<IChart, ChartConcrete>();
            container.RegisterType<IVerify, VerifyConcrete>();
            container.RegisterType<IHolidayList, HolidayListConcrete>();
            container.RegisterType<IBusinessHours, BusinessHoursConcrete>();
            container.RegisterType<ISlaPolicies, SlaPoliciesConcrete>();
            container.RegisterType<IDefaultTicketSettings, DefaultTicketSettingsConcrete>();
            container.RegisterType<ITicketEscalationHistory, TicketEscalationHistoryConcrete>();
            container.RegisterType<IAllTicketGrid, AllTicketGridConcrete>();
            container.RegisterType<IVerification, VerificationConcrete>();
            container.RegisterType<IExportReport, ExportReportConcrete>();
            container.RegisterType<IOverdueTypes, OverdueTypesConcrete>();
            container.RegisterType<ITicketNotification, TicketNotificationConcrete>();
        }
    }
}