using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

static class Extension
{

    public static bool[] LeftShift(this bool[] a, int b)
    {
        bool[] res;


        res = new bool[165];

        res = res << (int)9;
    }
}