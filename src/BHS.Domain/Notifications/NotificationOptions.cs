using System.ComponentModel.DataAnnotations;

namespace BHS.Domain.Notifications;

public class NotificationOptions
{
    [Required]
    public string FromAddress { get; set; } = string.Empty;

    [Required]
    public string FromName { get; set; } = string.Empty;
}
