using NUnit.Framework;
using UnityEngine;

public static class General_Game : object
{
    //RESEY
    [HideInInspector]
    public static float Timer;

    [HideInInspector]
    public static float PoliceTimer;

    [HideInInspector]
    public static bool IsPoliceCalled;

    [HideInInspector]
    public static float PoliceCalledThievesMultiplier;

    [HideInInspector]
    public static int CurrentDoneObjectives = 0;

    [HideInInspector]
    public static int ObjectivesCount = 5;

    



    [HideInInspector]
    public static bool IsHidden;


    [HideInInspector]
    public static bool IsSmall;
    [HideInInspector]
    public static bool IsMedium;
    [HideInInspector]
    public static bool IsBig;
    [HideInInspector]
    public static bool IsChasing;
    [HideInInspector]
    public static Vector3 ChasingWaypoint;

    //DONT RESET 
    [HideInInspector]
    public static Vector3 UIOffset =  new Vector3(0,6,-2.5f);

    [HideInInspector]
    public static float ObjectiveRadius = 2;

    [HideInInspector]
    public static float DoorRadius = 3;

    [HideInInspector]
    public static float KeyRadius = 4;
}
