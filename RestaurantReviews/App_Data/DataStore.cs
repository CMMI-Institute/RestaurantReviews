using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// This is going to act like our database, just storing objects in memory
/// I imagine this haveing 3 tables: Restaurants, Users, Reviews.  Each with a primary key
/// The Reviews table will have 2 foreign keys one going to Uers, one to Restaurants
/// Instead of the linq queries below, there would be sql queries
/// </summary>
public class DataStore
{
	static DataStore()
	{
		sm_instance = new DataStore();
	}

	public DataStore()
	{
		// going to act like our autonumber ID field in the db, just using the same for all types, but that isnt necessary
		m_nNextID = 1;

		m_Restaurants = new List<Restaurant>();
		m_Users = new List<User>();
		m_Reviews = new List<Review>();

		LoadTestData();
	}

	public static DataStore Instance
	{
		get
		{
			return sm_instance;
		}
	}
	public Task SaveChangesAsync()
	{
		// does nothing since we are all in memory
		return Task.CompletedTask;
	}

	private void LoadTestData()
	{
		var hotDogShop = new Restaurant();
		hotDogShop.Name = "Hot Dog Shop";
		hotDogShop.City = "Cranberry";
		AddRestaurant(hotDogShop);

		var primantis = new Restaurant();
		primantis.Name = "Primantis";
		primantis.City = "Cranberry";
		AddRestaurant(primantis);

		var meatPotatoes = new Restaurant();
		meatPotatoes.Name = "Meat and Potatoes";
		meatPotatoes.City = "Pittsburgh";
		AddRestaurant(meatPotatoes);

		var billy = new User();
		billy.Name = "Billy";
		billy.EmailAddress = "Billy@yahoo.com";
		AddUser(billy);

		var bobby = new User();
		bobby.Name = "Bobby";
		bobby.EmailAddress = "bobby@gmail.com";
		AddUser(bobby);

		var r1 = new Review();
		r1.Rating = 4;
		r1.RestaurantID = hotDogShop.ID;
		r1.UserID = billy.ID;
		r1.Comments = "Liked it a lot";
		AddReview(r1);

		var r2 = new Review();
		r2.Rating = 3;
		r2.RestaurantID = hotDogShop.ID;
		r2.UserID = bobby.ID;
		r2.Comments = "It was okay";
		AddReview(r2);
	}

	public IQueryable<Restaurant> Restaurants
	{
		get { return m_Restaurants.AsQueryable(); }
	}
	public void AddRestaurant(Restaurant restaurant)
	{
		restaurant.ID = m_nNextID++;
		m_Restaurants.Add(restaurant);
	}
	public Restaurant FindRestaurant(int nID)
	{
		return m_Restaurants.Where(r => r.ID == nID).FirstOrDefault();
	}
	public void RemoveRestaurant(Restaurant toRemove)
	{
		if (toRemove != null)
		{
			m_Restaurants.Remove(toRemove);
		}
	}

	public IQueryable<User> Users
	{
		get { return m_Users.AsQueryable(); }
	}
	public void AddUser(User User)
	{
		User.ID = m_nNextID++;
		m_Users.Add(User);
	}
	public User FindUser(int nID)
	{
		return m_Users.Where(r => r.ID == nID).FirstOrDefault();
	}
	public void RemoveUser(User toRemove)
	{
		if (toRemove != null)
		{
			m_Users.Remove(toRemove);
		}
	}

	public IQueryable<Review> Reviews
	{
		get { return m_Reviews.AsQueryable(); }
	}
	public void AddReview(Review review)
	{
		review.ID = m_nNextID++;
		m_Reviews.Add(review);
	}
	public Review FindReview(int nID)
	{
		return m_Reviews.Where(r => r.ID == nID).FirstOrDefault();
	}
	public IEnumerable<Review> FindReviewsByRestaurant(int restaurantID)
	{
		return m_Reviews.Where(r => r.RestaurantID == restaurantID);
	}
	public IEnumerable<Review> FindReviewsByUser(int userID)
	{
		return m_Reviews.Where(r => r.UserID == userID);
	}
	public void RemoveReview(Review toRemove)
	{
		if (toRemove != null)
		{
			m_Reviews.Remove(toRemove);
		}
	}

	private List<Restaurant> m_Restaurants;
	private List<User> m_Users;
	private List<Review> m_Reviews;
	private int m_nNextID;

	private static DataStore sm_instance;
}