namespace Final_Year_Project
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