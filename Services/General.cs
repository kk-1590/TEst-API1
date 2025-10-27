using AdvanceAPI.Controllers;
using AdvanceAPI.IServices;
using System.Text;

namespace AdvanceAPI.Services
{
    public class General : IGeneral
    {
        private readonly ILogger<General> _logger;
        public General(ILogger<General> logger)
        {
            _logger = logger;
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
    }
}
