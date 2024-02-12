namespace PacMan.Main;
class Program
{
    
    static void Main(string[] args)
    {
        using (Game game = new Game())
        {
            game.Run();

        }
        GameConsole.WriteLine("The Game window is running!");
    }
}