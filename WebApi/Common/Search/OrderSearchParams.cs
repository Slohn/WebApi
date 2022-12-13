namespace WebApi.Common.Search
{
    public class OrderSearchParams
    {
        public DateTime? SrartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
    }
}
