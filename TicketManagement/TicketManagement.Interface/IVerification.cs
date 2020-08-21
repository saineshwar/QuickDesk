using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models;

namespace TicketManagement.Interface
{
    public interface IVerification
    {
        void SendRegistrationVerificationToken(long? userid, string verficationToken);
        RegisterVerification GetRegistrationGeneratedToken(string userid);
        ResetPasswordVerification GetResetGeneratedToken(string userid);
        bool UpdateRegisterVerification(long userid);
        bool CheckIsAlreadyVerifiedRegistration(long userid);
        void SendResetVerificationToken(long userid, string verficationToken);
        bool UpdateResetVerification(long userid);
        int GetSentResetPasswordVerificationCount(long? userid);
        bool CheckIsEmailVerifiedRegistration(long userid);
    }
}
