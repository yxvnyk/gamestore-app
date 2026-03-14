using Gamestore.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gamestore.WebApi.Helpers.Helpers.Binders;

public class IdentityModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.CompletedTask;
        }

        if (Guid.TryParse(value, out var id))
        {
            bindingContext.Result = ModelBindingResult.Success(new Identity(id, null));
            return Task.CompletedTask;
        }

        if (int.TryParse(value, out var intId))
        {
            bindingContext.Result = ModelBindingResult.Success(new Identity(null, intId));
            return Task.CompletedTask;
        }

        bindingContext.ModelState.TryAddModelError(
            bindingContext.ModelName,
            "Invalid ID format. Must be Guid or integer.");
        return Task.CompletedTask;
    }
}
