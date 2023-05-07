using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityOrnek
{
    public static class Extensions
    {
        public static void AddListErrors(this ModelStateDictionary keyValuePairs, IEnumerable<string> errors)
        {
            errors.ToList().ForEach(x => { keyValuePairs.AddModelError(string.Empty,x); });
            
        }
    }
}
