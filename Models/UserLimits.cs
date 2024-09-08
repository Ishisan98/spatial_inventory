namespace spatial_inventory_server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("user_limits")]
    public class UserLimits
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int limit_id { get; set; }

        public int category_limit { get; set; } = 10;
        public int product_limit { get; set; } = 10;

        public int userId { get; set; }
        public User User { get; set; } = null!;
    }
}
