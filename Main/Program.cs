using PacMan.Engine;
using System.Collections.Generic;
using static PacMan.Main.enums;
namespace PacMan.Main;

class Program
{
    // Create Rooms
    public static Room temp_room = new((int)ROOM_ORDER.TEMP);
    public static Room title_room = new((int)ROOM_ORDER.TITLE);
    public static Room credit_room = new((int)ROOM_ORDER.CREDITS);
    public static Room level1_room = new((int)ROOM_ORDER.LEVEL1);
    public static Room level2_room = new ((int)ROOM_ORDER.LEVEL2);
    public static Room level3_room = new ((int)ROOM_ORDER.LEVEL3);

    public static int ActualRoom;

    // Add rooms to the list
    public static List<Room> rooms = [
        temp_room,
        title_room,
        credit_room,
        level1_room,
        level2_room,
        level3_room
    ];
    static void Main(string[] args)
    {
        using (Game game = new Game())
        {
            game.Run();

        }
        GameConsole.WriteLine("The Game window is running!");
    }
}