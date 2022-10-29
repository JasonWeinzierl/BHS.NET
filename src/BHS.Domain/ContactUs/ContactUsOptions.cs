namespace BHS.Domain.ContactUs;

public class ContactUsOptions
{
    public string? FromAddress { get; set; }
    public string? FromName { get; set; }
    public IList<string> ToAddresses { get; set; } = new List<string>();
}
