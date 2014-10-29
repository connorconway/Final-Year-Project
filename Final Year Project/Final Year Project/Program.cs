namespace Multiplayer_Software_Game_Engineering
{
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
}