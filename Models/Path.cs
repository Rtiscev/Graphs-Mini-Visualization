namespace AI_Graphs.Models
{
	public class PathModel
	{
		public int Step { get; set; }
		public string CName { get; set; }
		public int CID { get; set; }

		public float BoxOpacity { get; set; }
		public PathModel()
		{
		}
		public PathModel(int step, string cname, int cid, float boxOpacity)
		{
			Step = step; CName = cname; CID = cid; BoxOpacity = boxOpacity;
		}
	}
}