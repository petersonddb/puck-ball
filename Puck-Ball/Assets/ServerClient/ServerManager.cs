namespace ServerClient
{
    public class ServerManager
    {
        public static ServerManager Instance { get; private set; }

        public API Api { get; set; }
        public Authentication Auth { get; set; }
        public Connection Conn { get; set; }
        public Matches Mats { get; set; }

        private ServerManager() {}

        public static ServerManager Initialize()
        {
            Instance ??= new ServerManager();

            return Instance;
        }
    }
}
