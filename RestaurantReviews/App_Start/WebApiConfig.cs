using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace RestaurantReviews
{
	public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			ODataModelBuilder builder = new ODataConventionModelBuilder();
			builder.EntitySet<Restaurant>("Restaurants");
			builder.EntitySet<User>("Users");
			builder.EntitySet<Review>("Reviews");
			config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
			config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
		}
    }
}
