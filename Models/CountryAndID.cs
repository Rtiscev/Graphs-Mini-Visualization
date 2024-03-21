namespace AI_Graphs.Models
{
	public class CountryAndID
	{
		public string CountryName { get; set; }
		public int CountryID { get; set; }
		public CountryAndID()
		{
		}

		public CountryAndID(string cName, int cID)
		{
			CountryName = cName;
			CountryID = cID;
		}
	}
}
