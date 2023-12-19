using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using Basics;

namespace PacMan_Game;

internal class Game : GameWindow {
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
        Console.WriteLine("Initializing the Game Window...");

        //Centering this on monitor
        this.CenterWindow(new Vector2i(Consts.SCREEN_WIDTH, Consts.SCREEN_HEIGHT));
        GameConsole.WriteLine("Resizing Window : " + Consts.SCREEN_WIDTH + ", " + Consts.SCREEN_HEIGHT);
    }
}

internal class GameConsole {
    public static void WriteLine(string message)  {
        Console.WriteLine("-> " + message);
    }
}