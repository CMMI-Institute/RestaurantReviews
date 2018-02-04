using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
	/// <summary>
	/// Helper functions for use in tests
	/// </summary>
	public class TestHelpers
	{
		static TestHelpers()
		{
			BaseURL = "http://localhost:64966/odata/";
		}

		static public async Task<int> FindAnyUserID()
		{
			var client = new HttpClient();
			string strBase = BaseURL + "Users";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strBase),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				JObject item = JsonConvert.DeserializeObject(strResult) as Newtonsoft.Json.Linq.JObject;
				User user = item.Last.Last.Last.ToObject(typeof(User)) as User;

				return user.ID;
			}
		}

		static public async Task<int> FindAnyRestaurantID()
		{
			var client = new HttpClient();
			string strBase = BaseURL + "Restaurants";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strBase),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				JObject item = JsonConvert.DeserializeObject(strResult) as Newtonsoft.Json.Linq.JObject;
				Restaurant restaurant = item.Last.Last.Last.ToObject(typeof(Restaurant)) as Restaurant;

				return restaurant.ID;
			}
		}

		static public async Task<int> FindAnyReviewID()
		{
			var client = new HttpClient();
			string strBase = BaseURL + "Reviews";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strBase),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				JObject item = JsonConvert.DeserializeObject(strResult) as Newtonsoft.Json.Linq.JObject;
				Review review = item.Last.Last.Last.ToObject(typeof(Review)) as Review;

				return review.ID;
			}
		}

		static public string BaseURL { get; private set; }
	}
}

