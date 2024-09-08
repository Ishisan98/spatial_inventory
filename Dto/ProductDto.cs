namespace spatial_inventory_server.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }        
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? MeasuringUnit { get; set; }  
        public double Quantity { get; set; }        
        public double MinQuantity { get; set; }   
        public double Price { get; set; }        
        public string? Status { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; } = string.Empty;
        public int CategoryId { get; set; }       
    }
}