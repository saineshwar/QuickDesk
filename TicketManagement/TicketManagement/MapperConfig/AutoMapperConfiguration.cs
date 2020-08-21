using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.MapperConfig
{
    public class AutoMapperConfiguration
    {
        public static void Config()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SubMenuMasterCreate, SubMenuMaster>()
                    .ForMember(dest => dest.SubMenuName, opt => opt.MapFrom(src => src.SubMenuName))
                    .ForMember(dest => dest.ActionMethod, opt => opt.MapFrom(src => src.ActionMethod))
                    .ForMember(dest => dest.ControllerName, opt => opt.MapFrom(src => src.ControllerName))
                    .ForMember(dest => dest.MenuId, opt => opt.MapFrom(src => src.MenuId))
                    .ForMember(dest => dest.SubMenuId, opt => opt.MapFrom(src => src.SubMenuId))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());

                cfg.CreateMap<SubMenuMaster, SubMenuMasterViewModel>();
                cfg.CreateMap<UsermasterView, Usermaster>();
                cfg.CreateMap<AssignRoleViewModel, SavedMenuRoles>();
                cfg.CreateMap<AssignRoleViewModelSubMenu, SavedSubMenuRoles>();
                cfg.CreateMap<SavedAssignedRoles, AssignViewUserRoleModel>();

                cfg.CreateMap<CreateUserViewModel, Usermaster>()
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.EmailId, opt => opt.MapFrom(src => src.EmailId))
                    .ForMember(dest => dest.Status, opt => opt.Ignore())
                    .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());

                cfg.CreateMap<TicketsViewModel, Tickets>()
                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                    .ForMember(dest => dest.TrackingId, opt => opt.MapFrom(src => src.TrackingId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());


                cfg.CreateMap<TicketsUserViewModel, Tickets>()
                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                    .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contact))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                    .ForMember(dest => dest.TrackingId, opt => opt.MapFrom(src => src.TrackingId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());

                cfg.CreateMap<SmtpEmailSettingsViewModel,SmtpEmailSettings>()
                    .ForMember(dest => dest.Host, opt => opt.MapFrom(src => src.Host))
                    .ForMember(dest => dest.Port, opt => opt.MapFrom(src => src.Port))
                    .ForMember(dest => dest.Timeout, opt => opt.MapFrom(src => src.Timeout))
                    .ForMember(dest => dest.SslProtocol, opt => opt.MapFrom(src => src.SslProtocol))
                    .ForMember(dest => dest.TlSProtocol, opt => opt.MapFrom(src => src.TlSProtocol))
                    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Status, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore())
                    .ForMember(dest => dest.IsDefault, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());



                cfg.CreateMap<GeneralSettingsViewModel, GeneralSettings>()
                    .ForMember(dest => dest.GeneralSettingsId, opt => opt.MapFrom(src => src.GeneralSettingsId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.SupportEmailId, opt => opt.MapFrom(src => src.SupportEmailId))
                    .ForMember(dest => dest.WebsiteTitle, opt => opt.MapFrom(src => src.WebsiteTitle))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl));


                cfg.CreateMap<CreateAgentViewModel, Usermaster>()
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.EmailId, opt => opt.MapFrom(src => src.EmailId))
                    .ForMember(dest => dest.Status, opt => opt.Ignore())
                    .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());

            });
        }
    }
}