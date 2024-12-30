namespace Udemy.Common.Attributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class EndpointFilterModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var query = bindingContext.HttpContext.Request.Query;

        var start = int.TryParse(query["start"], out var s) ? s : 0;
        var limit = int.TryParse(query["limit"], out var l) ? l : 10;
        var sortBy = query["sortBy"].FirstOrDefault();
        var sortOrder = query["sortOrder"].FirstOrDefault()?.ToLower();
        var filterBy = query["filterBy"].FirstOrDefault();
        var filterValue = query["filterValue"].FirstOrDefault();

        var filter = new EndpointFilter
        {
            Start = start,
            Limit = limit,
            SortBy = sortBy,
            SortOrder = sortOrder,
            FilterBy = filterBy,
            FilterValue = filterValue
        };

        bindingContext.Result = ModelBindingResult.Success(filter);
        return Task.CompletedTask;
    }
}

// Record to encapsulate filtering logic
[ModelBinder(BinderType = typeof(EndpointFilterModelBinder))]
public record EndpointFilter
{
    public int Start { get; init; }
    public int Limit { get; init; } = 10;
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? FilterBy { get; init; }
    public string? FilterValue { get; init; }
}