namespace AdvanceAPI.IServices
{
    public interface IGeneral
    {
        string EncryptOrDecrypt(string text);
        string CampusNameByCode(string code);

        string GetFinancialSession(DateTime dateTime);
        long StringToLong(string? input);
        bool IsValidCampusCode(string? campusCode);
        string GetReplace(string str);
        string GetIpAddress();
        Task<string> GetEmpName(string empCode);
        bool IsFileExists(string file);
        string EncryptWithKey(string clearText, string key);
        string ToTitleCase(string input);
        string ConvertToTwoDecimalPlaces(string input);

        double ConvertToDouble(string input);
        decimal ConvertToDecimal(string input);
        string AmountInWords(string amount);
        bool ViewCurrentStock(string? type);

        bool ViewPreviousRate(string? type);
        bool ValidatePdfFile(IFormFile file);
        string Encrypt(string clearText);
        string EncryptWithoutHour(string clearText);

        Task UploadFile(IFormFile file, string filePath, string fileName);
        Task DeleteFile(string filePath, string fileName);
    }
}
