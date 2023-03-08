using System.ComponentModel.DataAnnotations;

namespace BHS.Domain.ContactUs;

public class ContactUsOptions
{
    [MinLength(1)]
    public IList<string> ToAddresses { get; set; } = new List<string>();
}
