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

    bool isValid = true;


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

        isValid = editor.IsValid();
        if (isValid)
        {
            if (GUILayout.Button("Create New Tile"))
            {
                if (Create()) {// true if sucessful
                    Close(); 
                }
            }
        }
        
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        
    }

    private bool Create()
    {
        string fileName = "Assets/Tile Infos/" + editor.tileName.stringValue + ".asset";
        if (AssetDatabase.FindAssets(fileName) != null)
        {
            if (!EditorUtility.DisplayDialog("Tile already exists with that name",
                "It looks like you're trying to create a tile with the " +
                "same name as one that already exists, did you mean to do that?",
                "Yes, Overwrite the existing tile",
                "Cancel"))
            {
                return false; // cancel
            }
        }
        AssetDatabase.CreateAsset(tileObject, fileName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = tileObject;
        return true;


        


    }

}