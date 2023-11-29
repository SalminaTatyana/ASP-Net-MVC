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
		public List<Filter> Filters { get; set; }
		public Basket basket { get; set; }
		public string Search { get; set; }
		public string Sort { get; set; }
		public int SortName { get; set; }
		public int SortPrice { get; set; }
		public int ChooseCategory { get; set; }
		public List<ChooseFilter> ChooseFilters { get; set; }
		public AllProductsData() { }
    }

	public class Filter
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<string> Value { get; set; }
		public List<int> ValueInt { get; set; }
		public List<DateTime> ValueDate { get; set; }
		public List<bool> ValueBool { get; set; }
		public List<double> ValueDouble { get; set; }
		public string ValueType { get; set; }

		
	}
	public class ChooseFilter
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public int ValueInt { get; set; }
		public DateTime ValueDate { get; set; }
		public bool ValueBool { get; set; }
		public double ValueDouble { get; set; }
		public string ValueType { get; set; }


	}
}
