namespace AI_Graphs
{
	public class StringEventArgs : EventArgs
	{
		public string msg { get; set; }

		public StringEventArgs(string _msg)
		{
			msg = _msg;
		}
	}
}