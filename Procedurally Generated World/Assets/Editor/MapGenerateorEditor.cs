using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapConstructor))]
public class MapGenerateorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapConstructor mapGen = (MapConstructor)target;

        if (DrawDefaultInspector()){
                mapGen.GenerateMap();
        }

        if(GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }

    }

}
