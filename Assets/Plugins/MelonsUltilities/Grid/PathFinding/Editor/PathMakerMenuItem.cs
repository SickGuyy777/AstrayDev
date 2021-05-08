using UnityEditor;
using UnityEngine;

public class PathMakerMenuItem
{
    [ MenuItem( "GameObject/Create Other/PathMaker" ) ]
    public static void CreatePathMaker()
    {
        GameObject obj = new GameObject("PathMaker", typeof(PathMaker));
    }
}
