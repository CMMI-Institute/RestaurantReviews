using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
	/// <summary>
	/// Specificly test the 5 use cases called out in requirements
	/// slightly redundant since we already cover a couple of these in the basic CRUD test
	/// </summary>
	public class UseCaseTests
	{
		public UseCaseTests()
		{
		}

		[Fact]
		public async Task ListRestaurantsByCity()
		{
			var client = new HttpClient();
			string strURL = TestHelpers.BaseURL + "Restaurants?$filter=City eq 'Cranberry'";

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strURL),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();
				Assert.Contains("Cranberry", strResult);
				Assert.DoesNotContain("Pittsburgh", strResult);
			}
		}

		[Fact]
		public async Task AddNewRestaurant()
		{
			var client = new HttpClient();
			string strURL = TestHelpers.BaseURL + "Restaurants";

			var post = new HttpRequestMessage
			{
				RequestUri = new Uri(strURL),
				Method = HttpMethod.Post,
				Content = new StringContent("{\"Name\": \"Wendys\",\"City\": \"Butler\"}", System.Text.Encoding.UTF8, "application/json"),
			};

			using (var response = await client.SendAsync(post))
			{
				Assert.Equal(HttpStatusCode.Created, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(Restaurant)) as Restaurant;
				Assert.Equal("Wendys", item.Name);
				Assert.Equal("Butler", item.City);
			}
		}

		[Fact]
		public async Task AddNewReview()
		{
			int nUserID = await TestHelpers.FindAnyUserID();
			int nRestaurantID = await TestHelpers.FindAnyRestaurantID();

			var client = new HttpClient();
			string strReviewBase = TestHelpers.BaseURL + "Reviews";

			string strNewRating = string.Format("\"UserID\": {0}, \"RestaurantID\": {1}, \"Rating\": 5", nUserID, nRestaurantID);
			var post = new HttpRequestMessage
			{
				RequestUri = new Uri(strReviewBase),
				Method = HttpMethod.Post,
				Content = new StringContent("{" + strNewRating + "}", System.Text.Encoding.UTF8, "application/json"),
			};

			using (var response = await client.SendAsync(post))
			{
				Assert.Equal(HttpStatusCode.Created, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();

				var item = JsonConvert.DeserializeObject(strResult, typeof(Review)) as Review;
				Assert.Equal(nUserID, item.UserID);
				Assert.Equal(nRestaurantID, item.RestaurantID);
				Assert.Equal(5, item.Rating);
			}
		}

		[Fact]
		public async Task GetReviewsByUser()
		{
			int nUserID = await TestHelpers.FindAnyUserID();

			var client = new HttpClient();
			string strURL = TestHelpers.BaseURL + "Reviews?$filter=UserID eq " + nUserID;

			// get collection
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri(strURL),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(request))
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				string strResult = await response.Content.ReadAsStringAsync();
				Assert.Contains(nUserID.ToString(), strResult);
			}
		}

		[Fact]
		public async Task DeleteAReview()
		{
			int nNewID = await TestHelpers.FindAnyReviewID();
			var client = new HttpClient();

			string strURL = TestHelpers.BaseURL + "Reviews(" + nNewID + ")";

			// delete item
			var delete = new HttpRequestMessage
			{
				RequestUri = new Uri(strURL),
				Method = HttpMethod.Delete
			};

			using (var response = await client.SendAsync(delete))
			{
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

			// try to get deleted item
			var notFound = new HttpRequestMessage
			{
				RequestUri = new Uri(strURL),
				Method = HttpMethod.Get
			};

			using (var response = await client.SendAsync(notFound))
			{
				Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			}
		}
	}
}

