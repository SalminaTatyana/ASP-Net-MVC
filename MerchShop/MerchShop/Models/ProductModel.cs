namespace IvanovaShop.Models
{
	public class ProductModel : BaseResponce
	{
		public Product? Product { get; set; }
        public ProductModel() { }
		public Basket basket { get; set; }


	}
	public class AllProductsData
	{
		public List<Product> Products { get; set; }
		public List<Category> Categories { get; set; }
		public Basket basket { get; set; }

		public AllProductsData() { }
    }
}
