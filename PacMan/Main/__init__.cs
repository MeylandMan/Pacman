using System;

namespace PacMan.Main;

internal class Consts
{
    //Screen consts
    public static int SCREEN_WIDTH = 640;
    public static int SCREEN_HEIGHT = 480;

    public static float WIDTH_ASPECT = (float)SCREEN_WIDTH/SCREEN_HEIGHT;
}

internal class enums {

    public int ActualRoom = (int)ROOM_ORDER.TEMP;
    public enum ROOM_ORDER {
        TEMP,
        TITLE,
        CREDITS,
        LEVEL1,
        LEVEL2,
        LEVEL3
    }
}
