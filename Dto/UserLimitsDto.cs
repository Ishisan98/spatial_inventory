using spatial_inventory_server.Models;

namespace spatial_inventory_server.Dto
{
    public class UserLimitsDto
    {
        public int LimitId { get; set; }
        public int CategoryLimit { get; set; }
        public int ProductLimit { get; set; }
        public int UserId { get; set; }
    }
}
