using Lab_App.Models;

namespace Lab_App.services
{
    public class LicenseService
    {
        private readonly myDBcontext db;
        public LicenseService(myDBcontext db)
        {
            this.db = db;
        }

        public bool ValidateLicense(string licenseKey)
        {
            var license = db.Licenses.FirstOrDefault(l => l.LicenseKey == licenseKey && l.IsActive);
            if (license == null || license.ExpirationDate < DateTime.UtcNow)
            {
                return false; 
            }
            return true;
        }
        public DateTime  getexpirationdate(string licenseKey)
        {
            var license = db.Licenses.FirstOrDefault(l => l.LicenseKey == licenseKey && l.IsActive);
            var ExpDate = license.ExpirationDate;
            if (license == null || license.ExpirationDate < DateTime.UtcNow)
            {
                return ExpDate;
            }
            return DateTime.UtcNow;
        }
    }
}
