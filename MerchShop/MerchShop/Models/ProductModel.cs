namespace IvanovaShop.Models
{
    public class ProductModel : BaseResponce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
		public ProductModel() { }
		public ProductModel(int id, string name, int price, string image, string category, string description = "")
        {
            Id = id;
            Name = name;
            Price = price;
            Image = image;
            Category = category;
            Description = description;
        }
    }
}
