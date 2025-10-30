namespace AdvanceAPI.IServices
{
    public interface IGeneral
    {
        string EncryptOrDecrypt(string text);
        Task<string> CampusNameByCode(string code);

        string GetFinancialSession(DateTime dateTime);
        long StringToLong(string? input);
        bool IsValidCampusCode(string? campusCode);
        string GetReplace(string str);
        string GetIpAddress();
        Task<string> GetEmpName(string empCode);
        Task<bool> IsFileExists(string file);

    }
}
