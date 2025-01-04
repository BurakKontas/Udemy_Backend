using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Udemy.Common.ModelBinder;

public class UserIdModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var id = bindingContext.HttpContext.Request.Headers["X-User-Id"];

        if (Guid.TryParse(id, out var userId))
        {
            bindingContext.Result = ModelBindingResult.Success(userId);
        }
        else
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }

        await Task.CompletedTask;
    }
}

[ModelBinder(BinderType = typeof(UserIdModelBinder))]
public record UserId(Guid Value);
