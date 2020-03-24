using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;

public class TileMenu : EditorWindow
{
    //[SerializeField]
    //string Title;

    ScriptableObject tileObject;

    SerializedObject tile;
    TileEditor editor;




    [MenuItem("HelperTools/New tile..")]
    static void Init()
    {
        TileMenu window = (TileMenu)EditorWindow.GetWindow(typeof(TileMenu));
        window.tileObject = ScriptableObject.CreateInstance<TileInfo>();
        window.tile = new SerializedObject(window.tileObject);
        window.editor = (TileEditor)Editor.CreateEditor(window.tileObject);
        window.Show();
        

    }
    
    private void OnGUI()
    {
        GUILayout.Label("Create new tile", EditorStyles.boldLabel);
        editor.OnInspectorGUI();
        

        if (GUILayout.Button("Create New Tile"))
        {
            Create();
            Close();
        }
        
    }

    private void Create()
    {

        AssetDatabase.CreateAsset(tileObject, "Assets/Tile Infos/"+editor.tileName.stringValue + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = tileObject;



        


    }

}