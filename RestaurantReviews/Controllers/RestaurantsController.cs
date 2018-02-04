using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

/// <summary>
/// Support basic CRUD plus navigation from a restaurant to a collection of reviews
/// </summary>
public class RestaurantsController : ODataController
{
	public RestaurantsController()
	{
	}

	[EnableQuery]
	public IQueryable<Restaurant> Get()
	{
		return DataStore.Instance.Restaurants;
	}

	[EnableQuery]
	public SingleResult<Restaurant> Get([FromODataUri] int key)
	{
		Restaurant result = DataStore.Instance.FindRestaurant(key);
		var listResult = new List<Restaurant>() { result };
		return SingleResult.Create(listResult.AsQueryable());
	}

	[EnableQuery]
	public IQueryable<Review> GetReviews([FromODataUri] int key)
	{
		var result = DataStore.Instance.FindReviewsByRestaurant(key);
		return result.AsQueryable();
	}

	public async Task<IHttpActionResult> Post(Restaurant restaurant)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		DataStore.Instance.AddRestaurant(restaurant);
		await DataStore.Instance.SaveChangesAsync();
		return Created(restaurant);
	}

	public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Restaurant> restaurant)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		var entity = DataStore.Instance.FindRestaurant(key);
		if (entity == null)
		{
			return NotFound();
		}
		restaurant.Patch(entity);
		await DataStore.Instance.SaveChangesAsync();
		return Updated(entity);
	}

	public async Task<IHttpActionResult> Put([FromODataUri] int key, Restaurant restaurant)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		if (key != restaurant.ID)
		{
			return BadRequest();
		}
		var existing = DataStore.Instance.FindRestaurant(key);
		DataStore.Instance.RemoveRestaurant(existing);
		DataStore.Instance.AddRestaurant(restaurant);
		await DataStore.Instance.SaveChangesAsync();
		return Updated(restaurant);
	}

	public async Task<IHttpActionResult> Delete([FromODataUri] int key)
	{
		var restaurant = DataStore.Instance.FindRestaurant(key);
		if (restaurant == null)
		{
			return NotFound();
		}
		DataStore.Instance.RemoveRestaurant(restaurant);
		await DataStore.Instance.SaveChangesAsync();
		return StatusCode(HttpStatusCode.NoContent);
	}
}