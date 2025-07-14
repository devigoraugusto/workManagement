namespace Domain.Entities
{
    public class PaginationParams
    {
        public required int PageNumber { get; set; } = 1;
        public required int PageSize { get; set; } = 10;
    }
}
