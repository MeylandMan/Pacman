using System;

namespace PacMan_Game;
class Program {

    static void Main(String[] args) {

        using(Game game = new Game()) {
            game.Run();

        }
        GameConsole.WriteLine("The Game window is running!");
    }
}