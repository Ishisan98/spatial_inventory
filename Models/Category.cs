namespace spatial_inventory_server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int category_id { get; set; }
        [Required]
        public string category_name { get; set; } = string.Empty;
        public string? description { get; set; }
        public string status { get; set; } = "Active";
        public DateTime created_date { get; set; } = DateTime.Now;
        public string created_by { get; set; } = string.Empty.ToString();
        public DateTime? modified_date { get; set; } = DateTime.Now;
        public string? modified_by { get; set; } = string.Empty.ToString();

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public int userId { get; set; }
        public User User { get; set; } = null!;
    }
}
