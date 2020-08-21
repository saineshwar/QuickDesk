using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IProfile
    {
        UsermasterEditView GetprofileById(long userId);
        int UpdateUserMasterDetails(long userId, UsermasterEditView usermasterEditView);
        void UpdateProfileImage(ProfileImage profileImage);
        bool IsProfileImageExists(long userId);
        string GetProfileImageBase64String(long userId);
        int DeleteProfileImage(long userId);
        int UpdateSignature(Signatures signatures);
        bool CheckSignatureAlreadyExists(long userId);
        int DeleteSignature(long userId);
        string GetSignature(long userId);
        bool ValidatePassword(string existingPassword, long userId);
        UserProfileView GetUserprofileById(long userId);
    }
}