using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Valume
{
    //ボリュームは共有で使う
    public static float MasterVal = 1.0f;
    public static float BGMVal = 1.0f;
    public static float SEVal = 1.0f;

    //タイトルの演出は一回だけでいい
    public static bool First_Title = false;

}
