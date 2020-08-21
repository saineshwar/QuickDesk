namespace TicketManagement.Interface
{
    public interface IVerify
    {
        int InsertEmailVerification(long? userId, string emailId, string verificationCode);
        bool CheckVerificationCodeExists(long userId);
        bool UpdatedVerificationCode(string verificationCode);
        bool ValidateEmailIdExists(string emailid);
        int InsertForgotPasswordVerification(string emailId, string verificationCode);
        string GetForgotPasswordToken(string emailid);
        bool UpdateFogotPasswordVerification(string verificationCode);
        bool CheckEmailIdIsAlreadyVerified(string emailid);
    }
}