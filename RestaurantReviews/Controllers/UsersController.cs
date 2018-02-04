using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

/// <summary>
/// Support basic CRUD plus navigation from a user to a collection of reviews
/// </summary>
public class UsersController : ODataController
{
	public UsersController()
	{
	}

	[EnableQuery]
	public IQueryable<User> Get()
	{
		return DataStore.Instance.Users;
	}

	[EnableQuery]
	public SingleResult<User> Get([FromODataUri] int key)
	{
		User result = DataStore.Instance.FindUser(key);
		var listResult = new List<User>() { result };
		return SingleResult.Create(listResult.AsQueryable());
	}

	[EnableQuery]
	public IQueryable<Review> GetReviews([FromODataUri] int key)
	{
		var result = DataStore.Instance.FindReviewsByUser(key);
		return result.AsQueryable();
	}

	public async Task<IHttpActionResult> Post(User User)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		DataStore.Instance.AddUser(User);
		await DataStore.Instance.SaveChangesAsync();
		return Created(User);
	}

	public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<User> User)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		var entity = DataStore.Instance.FindUser(key);
		if (entity == null)
		{
			return NotFound();
		}
		User.Patch(entity);
		await DataStore.Instance.SaveChangesAsync();
		return Updated(entity);
	}

	public async Task<IHttpActionResult> Put([FromODataUri] int key, User User)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		if (key != User.ID)
		{
			return BadRequest();
		}
		var existing = DataStore.Instance.FindUser(key);
		DataStore.Instance.RemoveUser(existing);
		DataStore.Instance.AddUser(User);
		await DataStore.Instance.SaveChangesAsync();
		return Updated(User);
	}

	public async Task<IHttpActionResult> Delete([FromODataUri] int key)
	{
		var User = DataStore.Instance.FindUser(key);
		if (User == null)
		{
			return NotFound();
		}
		DataStore.Instance.RemoveUser(User);
		await DataStore.Instance.SaveChangesAsync();
		return StatusCode(HttpStatusCode.NoContent);
	}
}