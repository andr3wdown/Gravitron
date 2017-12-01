using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        //base.OnInspectorGUI();
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        GUILayout.Space(10);

        if(GUILayout.Button("Generate Map"))
        {
            mapGen.GenerateMap();
        }
        if(GUILayout.Button("Generate Texture"))
        {
            mapGen.GenerateImage();
        }
    }
}
