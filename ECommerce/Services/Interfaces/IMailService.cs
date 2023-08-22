namespace ECommerce.Services.Interfaces
{
    public interface IMailService
    {
        void SendConfirmationMessage(string destinationAddress,string url,string receiverName);
        void SendResetPasswordMessage(string destinationAddress,string url);
    }
}
