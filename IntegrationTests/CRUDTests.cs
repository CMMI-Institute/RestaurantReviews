using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
	/// <summary>
	/// Test CRUD operations for all 3 types
	/// </summary>
	public class UseCaseTets
	{
		public UseCaseTets()
		{
		}

		[Fact]
		public async Task RestaurantTests()
		{
			var client = new HttpClient();
			string strRestaurantBase = TestHelpers.BaseURL + "Restaurants";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strRestaurantBase),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}

			// post new item
			var post = new HttpRequestMessage
			{
				RequestUri = new Uri(strRestaurantBase),
				Method = HttpMethod.Post,
				Content = new StringContent("{\"Name\": \"New Place\",\"City\": \"Gibsonia\"}", System.Text.Encoding.UTF8, "application/json"),
			};

			int nNewID = -1;
			using (var response = await client.SendAsync(post))
			{
				Assert.Equal(HttpStatusCode.Created, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(Restaurant)) as Restaurant;
				Assert.Equal("New Place", item.Name);
				Assert.Equal("Gibsonia", item.City);
				nNewID = item.ID;
			}

			// update item
			var patch = new HttpRequestMessage
			{
				RequestUri = new Uri(strRestaurantBase + "(" + nNewID + ")"),
				Method = new HttpMethod("PATCH"),
				Content = new StringContent("{\"City\": \"Lawrenceville\"}", System.Text.Encoding.UTF8, "application/json"),
			};

			using (var response = await client.SendAsync(patch))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			var updated = new HttpRequestMessage
			{
				RequestUri = new Uri(strRestaurantBase + "(" + nNewID + ")"),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(updated))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(Restaurant)) as Restaurant;
				Assert.Equal("Lawrenceville", item.City);
			}

			// delete item
			var delete = new HttpRequestMessage
			{
				RequestUri = new Uri(strRestaurantBase + "(" + nNewID + ")"),
				Method = HttpMethod.Delete				
			};

			using (var response = await client.SendAsync(delete))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			// try to get deleted item
			var notFound = new HttpRequestMessage
			{
				RequestUri = new Uri(strRestaurantBase + "(" + nNewID + ")"),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(notFound))
			{
				Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			}
		}

		[Fact]
		public async Task UserTests()
		{
			var client = new HttpClient();
			string strUserBase = TestHelpers.BaseURL + "Users";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strUserBase),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}

			// post new item
			var post = new HttpRequestMessage
			{
				RequestUri = new Uri(strUserBase),
				Method = HttpMethod.Post,
				Content = new StringContent("{\"Name\": \"Joe\", \"EmailAddress\": \"joe@hotmail.com\"}", System.Text.Encoding.UTF8, "application/json"),
			};

			int nNewID = -1;
			using (var response = await client.SendAsync(post))
			{
				Assert.Equal(HttpStatusCode.Created, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(User)) as User;
				Assert.Equal("Joe", item.Name);
				Assert.Equal("joe@hotmail.com", item.EmailAddress);
				nNewID = item.ID;
			}

			// update item
			var patch = new HttpRequestMessage
			{
				RequestUri = new Uri(strUserBase + "(" + nNewID + ")"),
				Method = new HttpMethod("PATCH"),
				Content = new StringContent("{\"Name\": \"Not Really Joe\"}", System.Text.Encoding.UTF8, "application/json"),
			};

			using (var response = await client.SendAsync(patch))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			var updated = new HttpRequestMessage
			{
				RequestUri = new Uri(strUserBase + "(" + nNewID + ")"),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(updated))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(User)) as User;
				Assert.Equal("Not Really Joe", item.Name);
				Assert.Equal("joe@hotmail.com", item.EmailAddress);
			}

			// delete item
			var delete = new HttpRequestMessage
			{
				RequestUri = new Uri(strUserBase + "(" + nNewID + ")"),
				Method = HttpMethod.Delete
			};

			using (var response = await client.SendAsync(delete))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			// try to get deleted item
			var notFound = new HttpRequestMessage
			{
				RequestUri = new Uri(strUserBase + "(" + nNewID + ")"),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(notFound))
			{
				Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			}
		}

		[Fact]
		public async Task ReviewTests()
		{
			// first find a valid User and Restaurant record to test with
			int nUserID = await TestHelpers.FindAnyUserID();
			int nRestaurantID = await TestHelpers.FindAnyRestaurantID();

			var client = new HttpClient();
			string strReviewBase = TestHelpers.BaseURL + "Reviews";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}

			// post new item
			string strNewRating = string.Format("\"UserID\": {0}, \"RestaurantID\": {1}, \"Rating\": 2", nUserID, nRestaurantID);
			var post = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase),
				Method = HttpMethod.Post,
				Content = new StringContent("{" + strNewRating + "}", System.Text.Encoding.UTF8, "application/json"),
			};

			int nNewID = -1;
			using (var response = await client.SendAsync(post))
			{
				Assert.Equal(HttpStatusCode.Created, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(Review)) as Review;
				Assert.Equal(nUserID, item.UserID);
				Assert.Equal(nRestaurantID, item.RestaurantID);
				Assert.Equal(2, item.Rating);
				nNewID = item.ID;
			}

			// update item
			var patch = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase + "(" + nNewID + ")"),
				Method = new HttpMethod("PATCH"),
				Content = new StringContent("{\"Rating\": 4}", System.Text.Encoding.UTF8, "application/json"),
			};

			using (var response = await client.SendAsync(patch))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			var updated = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase + "(" + nNewID + ")"),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(updated))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(Review)) as Review;
				Assert.Equal(nUserID, item.UserID);
				Assert.Equal(nRestaurantID, item.RestaurantID);
				Assert.Equal(4, item.Rating);
			}

			// delete item
			var delete = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase + "(" + nNewID + ")"),
				Method = HttpMethod.Delete
			};

			using (var response = await client.SendAsync(delete))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			// try to get deleted item
			var notFound = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase + "(" + nNewID + ")"),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(notFound))
			{
				Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			}
		}
	}
}

