namespace spatial_inventory_server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_id { get; set; }
        [Required]
        public string product_name { get; set; } = string.Empty;
        public string? description { get; set; }
        public string? measuring_unit { get; set; } 
        public double quantity { get; set; } = 0.0;
        public double min_quantity { get; set; } = 0.0;
        public double unit_price { get; set; } = 0.0;
        public string? location { get; set; }
        public string status { get; set; } = "Active";
        public DateTime created_date { get; set; } = DateTime.Now;
        public string created_by { get; set; } = string.Empty.ToString();
        public DateTime modified_date { get; set; } = DateTime.Now;
        public string? modified_by { get; set;} 

        public int categoryId { get; set; }
        public Category Category { get; set; } = null!;


        public int userId { get; set; }
        public User User { get; set; } = null!;
    }
}
