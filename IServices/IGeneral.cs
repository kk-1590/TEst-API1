namespace AdvanceAPI.IServices
{
    public interface IGeneral
    {
        string EncryptOrDecrypt(string text);
        Task<string> CampusNameByCode(string code);

        string GetFinancialSession(DateTime dateTime);
        long StringToLong(string? input);
        bool IsValidCampusCode(string? campusCode);
        Task<bool> CheckColumn(string columnName, string EmpCode);
    }
}
