namespace FateGames
{
    public static class FacebookManager
    {
        public delegate void CallbackFunction();
        public static void Initialize(CallbackFunction CallbackFunction)
        {
            CallbackFunction();
        }
    }

}
