namespace spatial_inventory_server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Required]
        public string username { get; set; } = string.Empty;

        [Required] 
        public string password { get; set; } = string.Empty;

        public string hax_password { get; set; } = string.Empty;

        [Required] 
        public string email { get; set; } = string.Empty;

        [Required]
        public string country_code {  get; set; } = string.Empty;

        [Required]
        public string contact_no { get; set; } = string.Empty;

        [Required]
        public string display_name { get; set; } = string.Empty ;

        [Required]
        public string surname { get; set; } = string.Empty;

        [Required]
        public string first_name { get; set; } = string.Empty;
        public string? last_name { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public string? gender { get; set; }
        public string? profile_picture { get; set; }
        public string status { get; set; } = "Active";
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime modified_date {  get; set; } = DateTime.Now;


        public UserLimits? UserLimits { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
