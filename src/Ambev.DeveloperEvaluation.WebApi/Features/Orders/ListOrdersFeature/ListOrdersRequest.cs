namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.ListOrdersFeature;

public class ListOrdersRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public bool Desc { get; set; } = true;
}