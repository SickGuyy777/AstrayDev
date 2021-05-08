using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathMaker))]
public class PathMakerEditor : Editor
{
    private PathMaker pathMaker;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        pathMaker = (PathMaker) target;
        
        GUILayout.Space(10);

        CreateBakeButton();
    }

    private void CreateBakeButton()
    {
        if(pathMaker.AutoBake)
            return;
        
        if (GUILayout.Button("Bake"))
        {
            pathMaker.Bake();
        }
    }
}
