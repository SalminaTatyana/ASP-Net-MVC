using System;

namespace IvanovaShop.Models
{
	public class Basket
	{
		public List<BasketItem> Items { get; set; }
		public int Sum
		{
			get;
			set;
		}

		public Basket()
		{
			this.Items = new List<BasketItem>();
		}

		public void Add(Product product)
		{
			var item = Items.Where(s => s.Product.Id == product.Id).ToList();
			if (item != null && item.Count() > 0)
			{
				item.SingleOrDefault().Count += 1;
				item.SingleOrDefault().UpdateTotalSum();
			}
			else
			{
				Items.Add(new BasketItem { Product = product, Count = 1, Sum = product.Price });
			}
			UpdateTotalSum();

		}
		public bool Contains(Product product)
		{
			if (Items.Count == 0) return false;

			if (Items.Where(s => s.Product.Id == product.Id).ToList().Count > 0)
			{
				return true;
			}

			return false;
		}
		public int ProductCount(Product product)
		{
			if (Contains(product))
			{
				var count = Items.Where(s => s.Product.Id == product.Id).FirstOrDefault().Count;
				return count;
			}
			return 0;
		}
		public void Delete(Product product)
		{
			var item = Items.Where(s => s.Product.Id == product.Id).ToList();
			if (item != null && item.Count() > 0)
			{
				if (item.SingleOrDefault().Count > 1)
				{
					item.SingleOrDefault().Count-=1;
					item.SingleOrDefault().UpdateTotalSum();
				}
				else
				{
					Items.Remove(item.SingleOrDefault());
				}
			}
			UpdateTotalSum();
		}
		public void Clear()
		{
			Items.Clear();

		}
		public void ClearItem(Product product)
		{
			if (Contains(product))
			{
				Items.Remove(Items.Where(s => s.Product.Id == product.Id).SingleOrDefault());
			}

		}
		public int GetBasketCount()
		{
			return this.Items.Count;
		}

		private void UpdateTotalSum()
		{
			Sum = Items.Sum(s => s.Sum);
		}
	}
	public class BasketItem
	{
		public Product Product { get; set; }
		public int Count { get; set; }
		public int Sum { get; set; }
		public void UpdateTotalSum()
		{
			Sum = Product.Price * Count;
		}

	}
}
