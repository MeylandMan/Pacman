using System;
using OpenTK.Mathematics;
using PacMan.Engine;
using PacMan.Graphics;
using static PacMan.Main.enums;

namespace PacMan.Main;
class Program
{
    static void Main(string[] args)
    {
        VAO mainSurface = new VAO();
        Obj ObjetctTest1 = new(16f, 16f, mainSurface, new Vector2(-0.2f, 0f));
        Obj ObjetctTest2 = new(16f, 16f, mainSurface, new Vector2(0.2f, 0f));
        int ActualRoom = (int)ROOM_ORDER.TEMP;

        Rooms temp_room = new((int)ROOM_ORDER.TEMP);

        using (Game game = new Game())
        {
            game.Run();

        }
        GameConsole.WriteLine("The Game window is running!");
    }
}