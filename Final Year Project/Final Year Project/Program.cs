namespace Multiplayer_Software_Game_Engineering
{
#if WINDOWS || XBOX
    public static class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}