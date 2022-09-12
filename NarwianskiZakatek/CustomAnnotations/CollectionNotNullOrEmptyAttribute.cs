using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.CustomAnnotations
{
    public class CollectionNotNullOrEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count != 0;
            }
            var enumerable = value as IEnumerable;
            return enumerable != null && enumerable.GetEnumerator().MoveNext();
        }
    }
}
