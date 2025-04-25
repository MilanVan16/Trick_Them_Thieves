using NUnit.Framework;
using UnityEngine;

public static class General_Game : object
{
    [HideInInspector]
    public static float _timer;

    [HideInInspector]
    public static int CurrentDoneObjectives = 0;

    [HideInInspector]
    public static int ObjectivesCount = 5;

    [HideInInspector]
    public static float ObjectiveRadius = 2;

    [HideInInspector]
    public static bool IsHidden;
}
