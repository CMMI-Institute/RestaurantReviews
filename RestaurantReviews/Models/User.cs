using System.Collections.Generic;

public class User
{
	public User()
	{
	}

	public int ID { get; set; }
	public string Name { get; set; }
	public string EmailAddress { get; set; }
	public IEnumerable<Review> Reviews
	{
		get
		{
			return DataStore.Instance.FindReviewsByUser(ID);
		}
	}
}