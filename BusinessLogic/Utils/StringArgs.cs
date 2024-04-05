namespace AI_Graphs.Utils
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