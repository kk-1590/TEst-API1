using System.Data;
using AdvanceAPI.Controllers;
using AdvanceAPI.IServices;
using System.Text;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants;

namespace AdvanceAPI.Services
{
    public class General : IGeneral
    {
        private readonly ILogger<General> _logger;
        private readonly IDBOperations _dBOperations;
        public General(ILogger<General> logger, IDBOperations dBOperations)
        {
            _logger = logger;
            _dBOperations = dBOperations;
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

        public async Task<bool> CheckColumn(string columnName, string EmpCode)
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
           // sqlParameters.Add(new SQLParameters("@ColumnName", columnName));
            sqlParameters.Add(new SQLParameters("@EmpCode", EmpCode));
            
            DataTable dataTable = await _dBOperations.SelectAsync(GeneralSql.CECK_ALLOWED_COLUMN.Replace("@ColumnName",columnName),sqlParameters,DBConnections.Advance);
            return dataTable.Rows.Count > 0;
        }
    }
}
