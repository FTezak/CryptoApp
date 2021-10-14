namespace CryptoAPI.Helpers
{
    public class PaginationParams
    {
        private const int MaxPageSize = 5000;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 1000;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
