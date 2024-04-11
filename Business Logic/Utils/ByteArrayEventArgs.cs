namespace AI_Graphs.BusinessLogic.Utils
{
	public class ByteArrayEventArgs : EventArgs
	{
		public byte[] Data { get; set; }

		public ByteArrayEventArgs(byte[] data)
		{
			Data = data;
		}
	}
}
