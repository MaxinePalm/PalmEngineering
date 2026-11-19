namespace PalmEngineering.Models
{
    /// <summary>
    /// Legal and contact details shown in the footer, contact page, privacy policy and terms.
    /// Override any value in appsettings.json under "Business".
    /// </summary>
    public class BusinessInfo
    {
        public string LegalName { get; set; } = "MICROMATICA 908 (PTY) LTD";
        public string TradingName { get; set; } = "Palm Engineering";
        public string RegistrationNumber { get; set; } = "2003/011854/07";
        public string? VatNumber { get; set; }
        public string? Directors { get; set; }
        public string? InformationOfficer { get; set; }
        public string InformationOfficerName => string.IsNullOrWhiteSpace(InformationOfficer) ? "The Information Officer" : InformationOfficer;

        public string Email { get; set; } = "palm@palmeng.co.za";
        public string PreferredPhone { get; set; } = "082 921 7644";
        public string AlternativePhone { get; set; } = "083 656 6604";
        public string WorkPhone { get; set; } = "041 453 9681";

        public string StreetAddress { get; set; } = "242 Commercial Rd";
        public string Suburb { get; set; } = "Sidwell";
        public string City { get; set; } = "Gqeberha";
        public string Province { get; set; } = "Eastern Cape";
        public string PostalCode { get; set; } = "6070";

        public string FullAddress => $"{StreetAddress}, {Suburb}, {City}, {Province}, {PostalCode}";

        public static string Tel(string phone) => new string(phone.Where(char.IsDigit).ToArray());
    }
}
