using AdvanceAPI.Controllers;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants;
using System.Data;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AdvanceAPI.IServices.Inclusive;

namespace AdvanceAPI.Services
{
    public class General : IGeneral
    {
        private readonly ILogger<General> _logger;
        private readonly IDBOperations _dBOperations;
        // private readonly IInclusiveService _iInclusiveService;
        private readonly IHttpContextAccessor _httpAccessor;
        public General(ILogger<General> logger, IDBOperations dBOperations, IHttpContextAccessor httpAccessor)
        {
            _logger = logger;
            _dBOperations = dBOperations;
            _httpAccessor = httpAccessor;
            // _iInclusiveService = iInclusiveService;
        }

        public string EncryptOrDecrypt(string? text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return string.Empty;
                }
                int key = 7;
                StringBuilder stringBuilder = new StringBuilder("");
                for (int i = 0; i < text.Length; i++)
                {
                    int charValue = Convert.ToInt32(text[i]);
                    charValue ^= key;
                    stringBuilder.Append(char.ConvertFromUtf32(charValue));
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During EncryptOrDecrypt. Parameters: Parameters: {Request}", text);
                return string.Empty;
            }
        }

        public async Task<string> CampusNameByCode(string CampusCode)
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@CampusCode", CampusCode));
            DataTable getCampusCame=await _dBOperations.SelectAsync(GeneralSql.GET_CAMPUS_NAME,sqlParameters,DBConnections.Advance);
            if (getCampusCame.Rows.Count > 0)
            {
                return getCampusCame.Rows[0][0]?.ToString() ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetFinancialSession(DateTime dateTime)
        {
            return (dateTime.Month >= 4 ? dateTime.Year.ToString() + "-" + (dateTime.Year + 1).ToString().Substring(2, 2) : (dateTime.Year - 1).ToString() + "-" + (dateTime.Year).ToString().Substring(2, 2));
        }

        public long StringToLong(string? input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return 0;
                }
                return Convert.ToInt64(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During StringToLong. Parameters: Parameters: {Request}", input);
                return 0;
            }
        }

        public bool IsValidCampusCode(string? campusCode)
        {
            var validCampusCodes = new HashSet<string> { "101", "102", "103" };
            return !string.IsNullOrEmpty(campusCode) && validCampusCodes.Contains(campusCode);
        }

        public  string GetReplace(string str)
        {
            return str.Replace("\"", "\\\"").Replace("'", "\\'");
        }
        public string GetIpAddress()
        {
            if (_httpAccessor.HttpContext == null)
            {
                return "";
            }
            var r=Dns.GetHostAddresses(Dns.GetHostName());
    
            for(int i=0;i<r.Length;i++)
            {
                string s = r[i].ToString();
                if(s.Contains("172.16.13.244"))
                {
                    return _httpAccessor.HttpContext.Connection.RemoteIpAddress.ToString() ?? "";
                }
            }

            return _httpAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString()??"";
        }

        public async Task<string> GetEmpName(string empCode)
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@EmpCode", empCode));
            DataTable dt=await _dBOperations.SelectAsync(GeneralSql.GETEMPNAME,sqlParameters,DBConnections.Advance);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0]?.ToString() ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public  bool IsFileExists(string file)
        {
            try
            {
                string fullPath = Path.GetFullPath(file);
                return System.IO.File.Exists(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During IsFileExists. File: {File}", file);
                return false;
            }
        }
        public string EncryptWithKey(string clearText, string key)
        {
            string EncryptionKey = key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public string ConvertToTwoDecimalPlaces(string input)
        {
            if (decimal.TryParse(input, out decimal number))
            {
                return number.ToString("F2");
            }
            return input;
        }

        public double ConvertToDouble(string input)
        {
            if (double.TryParse(input, out double number))
            {
                return number;
            }
            return 0;
        }

        public decimal ConvertToDecimal(string input)
        {
            if (decimal.TryParse(input, out decimal number))
            {
                return number;
            }
            return 0;
        }

        public string AmountInWords(string amount)
        {
            return Spell.SpellAmount.InWrods(Convert.ToDecimal(amount)).Replace("Taka ", "").Replace("Only", "Rupees Only");
        }

        public bool ViewCurrentStock(string? type)
        {
            string[] GetBalanceStock = new string[] { "Purchase Approval Form", "Repeat Purchase Approval Form", "Additional Purchase Approval Form", "Transport - Purchase Approval Form", "Post Facto - Purchase Approval Form", "Post Facto - Repeat Purchase Approval Form", "Post Facto - Additional Purchase Approval Form", "Post Facto - Transport - Purchase Approval Form" };

            return Array.IndexOf(GetBalanceStock, type) != -1;
        }

        public bool ViewPreviousRate(string? type)
        {

            string[] GetPrevRate = new string[] { "Purchase Approval Form", "Repeat Purchase Approval Form", "Repeat Repair & Maintenance Approval Form", "Additional Purchase Approval Form", "Water Testing Approval Form", "Renewal Approval Form", "Annual Maintenance Approval Form", "Transport - Purchase Approval Form", "Post Facto - Purchase Approval Form", "Post Facto - Repeat Purchase Approval Form", "Post Facto - Repeat Repair & Maintenance Approval Form", "Post Facto - Additional Purchase Approval Form", "Post Facto - Water Testing Approval Form", "Post Facto - Renewal Approval Form", "Post Facto - Annual Maintenance Approval Form", "Post Facto - Transport - Purchase Approval Form", "Job Work Approval Form", "Services", "Post Facto - Services" };

            return Array.IndexOf(GetPrevRate, type) != -1;
        }

        public  bool ValidatePdfFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // Check file extension
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                return false;

            // Check MIME type
            if (file.ContentType != "application/pdf")
                return false;

            // Optional: Check PDF file header bytes ("%PDF-")
            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                byte[] headerBytes = reader.ReadBytes(5);
                var header = System.Text.Encoding.ASCII.GetString(headerBytes);
                if (!header.Equals("%PDF-", StringComparison.Ordinal))
                    return false;
            }

            return true;
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "GLA" + DateTime.Now.Year + "UNI" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "VER" + DateTime.Now.Day.ToString().PadLeft(2, '0') + "SITY" + DateTime.Now.Hour.ToString().PadLeft(2, '0') ;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}
