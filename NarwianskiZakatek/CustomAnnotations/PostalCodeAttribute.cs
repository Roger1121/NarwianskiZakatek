using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.CustomAnnotations
{
    public class PostalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var text = value as string;
            if(text == null || text.Length != 6 || text[2]!='-')
            {
                return false;
            }
            for(int i = 0; i < text.Length; i++)
            {
                if(i != 2 && (text[i] < '0' || text[i] > '9'))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
