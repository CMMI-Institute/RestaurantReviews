using System.Collections.Generic;
using System.Linq;

public class Restaurant
{
	public Restaurant()
	{
	}

	public int ID { get; set; }
	public string Name { get; set; }
	public string City { get; set; }
	public IEnumerable<Review> Reviews
	{
		get
		{
			return DataStore.Instance.FindReviewsByRestaurant(ID);
		}
	}
	public double? Rating
	{
		get
		{
			if (Reviews != null && Reviews.Count() > 0)
			{
				double dTotal = 0;
				int nCount = 0;
				foreach (var review in Reviews)
				{
					nCount++;
					dTotal += review.Rating;
				}

				return dTotal / nCount;
			}

			return null;
		}
		set
		{
			//json serializer issue, could swap out to a better serializer that can handle read only properties
		}
	}
}