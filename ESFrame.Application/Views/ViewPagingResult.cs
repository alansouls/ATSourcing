namespace ESFrame.Application.Views;

public class ViewPagingResult<TData>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }

    public List<TData> Data { get; set; } = [];
}
