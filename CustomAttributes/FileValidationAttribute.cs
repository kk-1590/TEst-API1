using AdvanceAPI.Classes;
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileValidationAttribute : ValidationAttribute
    {
        public long MaxFileSizeBytes { get; }
        public string[] AllowedExtensions { get; }

        public FileValidationAttribute(long maxFileSizeBytes, params string[] allowedExtensions)
        {
            MaxFileSizeBytes = maxFileSizeBytes;
            AllowedExtensions = allowedExtensions.Select(e => e.StartsWith('.') ? e.ToLowerInvariant() : "." + e.ToLowerInvariant()).ToArray();
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success; // Not required, so skip if null

            if (file.Length > MaxFileSizeBytes)
                return new ValidationResult($"File size must be less than or equal to {MaxFileSizeBytes / (1024 * 1024)} MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return new ValidationResult($"File type must be: {string.Join(", ", AllowedExtensions)}.");

            if (!FileHelper.IsValidFile(file, extension.Replace(".", "")))
                return new ValidationResult($"Invalid file found. File may be corrupted or virus infected or unable to open/read file.");

            return ValidationResult.Success;
        }
    }
}
