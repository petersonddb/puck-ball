namespace ServerClient
{
    public class API
    {
        public Nakama.IClient Client
        {
            get
            {
                return new Nakama.Client("http", "127.0.0.1", 7350, "defaultkey");
            }
        }
    }
}
