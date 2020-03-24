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
        string newName = editor.tileName.stringValue;
        string fileName = TileInfo.AssetPath +editor.tileName.stringValue + ".asset";
        //if (AssetDatabase.FindAssets(fileName) != null)
        //{
        //    if (!EditorUtility.DisplayDialog("Tile already exists with that name",
        //        "It looks like you're trying to create a tile with the " +
        //        "same name as one that already exists, did you mean to do that?",
        //        "Yes, Overwrite the existing tile",
        //        "Cancel"))
        //    {
        //        return false; // cancel
        //    }
        //}

        DirectoryInfo dir = new DirectoryInfo(TileInfo.AssetPath);
        FileInfo[] fileInfos = dir.GetFiles("*.asset");

        foreach (FileInfo file in fileInfos)
        {
            TileInfo obj = (TileInfo)AssetDatabase.LoadAssetAtPath<ScriptableObject>(TileInfo.AssetPath + file.Name);
            if (obj.name == ((TileInfo)tileObject).tileName|| file.Name == ((TileInfo)tileObject).tileName)
            {
                if (!EditorUtility.DisplayDialog("Tile already exists with that name",
                   "It looks like you're trying to create a tile with the " +
                   "same name as one that already exists, did you mean to do that?",
                   "Yes, Overwrite the existing tile",
                   "Cancel"))
                {
                    return false; // cancel
                }
                else
                {
                    AssetDatabase.DeleteAsset(TileInfo.AssetPath + file.Name);

                }
            }
            if (obj.tile == ((TileInfo)tileObject).tile) {
                if(!EditorUtility.DisplayDialog("Tile is not unique", "This tile has already been used, would you like to overwrite it?", "Yes, Replace existing tile", "Cancel"))
                {
                    return false; // cancel
                }
                else
                {
                    AssetDatabase.DeleteAsset(TileInfo.AssetPath + file.Name);
                    
                }
            }
        }

        AssetDatabase.CreateAsset(tileObject, fileName);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        

        return true;


        


    }

}