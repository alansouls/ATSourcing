using FluentResults;

namespace ESFrame.Application.Views;

public class ViewPagingParameters
{
    public int Page { get; private set; }
    public int PageSize{ get; private set; }
    public string Sort { get; private set; } = string.Empty;
    public string SortDirection { get; private set; } = string.Empty;

    public static Result<ViewPagingParameters> Create(int page, int pageSize, string? sort, string? sortDirection,
        string[] allowedSortFields,
        int? maxPageSize = null)
    {
        var parameter = new ViewPagingParameters();

        if (page < 1)
        {
            return Result.Fail("Page must be greater than 0");
        }
        else
        {
            parameter.Page = page;
        }

        if (pageSize < 1)
        {
            return Result.Fail("PageSize must be greater than 0");
        }
        else
        {
            parameter.PageSize = pageSize;
        }

        if (maxPageSize.HasValue && pageSize > maxPageSize)
        {
            parameter.PageSize = maxPageSize.Value;
        }

        if (!string.IsNullOrEmpty(sort) && !allowedSortFields.Contains(sort))
        {
            return Result.Fail($"Sort field '{sort}' is not allowed");
        }
        else
        {
            parameter.Sort = string.IsNullOrEmpty(sort) ? 
                allowedSortFields.FirstOrDefault() ?? string.Empty : 
                sort;
        }

        if (!string.IsNullOrEmpty(sortDirection) && sortDirection != "asc" && sortDirection != "desc")
        {
            return Result.Fail("Sort direction must be 'asc' or 'desc'");
        }
        else
        {
            parameter.SortDirection = string.IsNullOrEmpty(sortDirection) ? "asc" : sortDirection;
        }

        return parameter;
    }

}
