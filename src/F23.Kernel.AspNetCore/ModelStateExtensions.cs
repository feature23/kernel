using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace F23.Kernel.AspNetCore;

public static class ModelStateExtensions
{
    public static void AddModelErrors(this ModelStateDictionary modelState, string key, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(key, error.Message);
        }
    }

    public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Key, error.Message);
        }
    }

    public static ModelStateDictionary ToModelState(this IEnumerable<ValidationError> errors)
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelErrors(errors);
        return modelState;
    }
}
