using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

/// <summary>
/// Support basic CRUD plus navigation from a review to the User or the Restaurant
/// </summary>
public class ReviewsController : ODataController
{
	public ReviewsController()
	{
	}

	[EnableQuery]
	public IQueryable<Review> Get()
	{
		return DataStore.Instance.Reviews;
	}

	[EnableQuery]
	public SingleResult<Review> Get([FromODataUri] int key)
	{
		Review result = DataStore.Instance.FindReview(key);
		var listResult = new List<Review>() { result };
		return SingleResult.Create(listResult.AsQueryable());
	}

	[EnableQuery]
	public SingleResult<Restaurant> GetRestaurant([FromODataUri] int key)
	{
		int nRestaurantID = DataStore.Instance.FindReview(key).RestaurantID;
		var result = DataStore.Instance.FindRestaurant(nRestaurantID);
		var listResult = new List<Restaurant>() { result };
		return SingleResult.Create(listResult.AsQueryable());
	}

	[EnableQuery]
	public SingleResult<User> GetUser([FromODataUri] int key)
	{
		int nUserID = DataStore.Instance.FindReview(key).UserID;
		var result = DataStore.Instance.FindUser(nUserID);
		var listResult = new List<User>() { result };
		return SingleResult.Create(listResult.AsQueryable());
	}

	public async Task<IHttpActionResult> Post(Review Review)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		DataStore.Instance.AddReview(Review);
		await DataStore.Instance.SaveChangesAsync();
		return Created(Review);
	}

	public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Review> Review)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		var entity = DataStore.Instance.FindReview(key);
		if (entity == null)
		{
			return NotFound();
		}
		Review.Patch(entity);
		await DataStore.Instance.SaveChangesAsync();
		return Updated(entity);
	}

	public async Task<IHttpActionResult> Put([FromODataUri] int key, Review Review)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		if (key != Review.ID)
		{
			return BadRequest();
		}
		var existing = DataStore.Instance.FindReview(key);
		DataStore.Instance.RemoveReview(existing);
		DataStore.Instance.AddReview(Review);
		await DataStore.Instance.SaveChangesAsync();
		return Updated(Review);
	}

	public async Task<IHttpActionResult> Delete([FromODataUri] int key)
	{
		var Review = DataStore.Instance.FindReview(key);
		if (Review == null)
		{
			return NotFound();
		}
		DataStore.Instance.RemoveReview(Review);
		await DataStore.Instance.SaveChangesAsync();
		return StatusCode(HttpStatusCode.NoContent);
	}
}