namespace TicketManagement.ViewModels
{
    public class TicketNotificationViewModel
    {
        public long? TicketId { get; set; }
        public long? NotificationId { get; set; }
        public string TrackingId { get; set; }
        public string TicketAssignedDate { get; set; }
        public string NotificationDate { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
    }

    public class RequestNotificationViewModel
    {
        public long? NotificationId { get; set; }
    }
}