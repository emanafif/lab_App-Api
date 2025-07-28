namespace Lab_App.DTO
{
    public class loginResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string CurrentUser { get; set; }
        public string LicenseKey { get; set; }
        public int LabId { get; set; }
        public List<string> RolesforthisUser { get; set; } // قائمة من الأدوار
    }
}
