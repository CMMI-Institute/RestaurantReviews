public class Review
{
	public Review()
	{
	}

	public int ID { get; set; }
	public int UserID { get; set; }
	public int RestaurantID { get; set; }
	public int Rating { get; set; }
	public string Comments { get; set; }
	public string UserDisplay
	{
		get
		{
			// display related User name, not really needed since we could Expand the User Navigation in our request if we really wanted this data
			var user = DataStore.Instance.FindUser(UserID);
			if (user == null)
				return "";

			return user.Name;
		}
		set { }
	}
	public string RestaurantDisplay
	{
		get
		{
			// display related Restaurant name, not really needed since we could Expand the Restaurant Navigation in our request if we really wanted this data
			var restaurant = DataStore.Instance.FindRestaurant(RestaurantID);
			if (restaurant == null)
				return "";

			return restaurant.Name;
		}
		set { }
	}
}