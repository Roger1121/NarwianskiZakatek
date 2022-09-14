using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.CustomAnnotations
{
    public class RequiredNotNullOrWhitespaceAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var text = value as string;
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
