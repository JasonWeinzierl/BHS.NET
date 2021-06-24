using System.Collections.Generic;

namespace BHS.Model.Providers
{
    public class ContactUsOptions
    {
        public string? FromAddress { get; set; }
        public string? FromName { get; set; }
        public IReadOnlyCollection<string> ToAddresses { get; set; } = new List<string>();
    }
}
