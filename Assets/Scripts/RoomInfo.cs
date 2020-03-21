using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RoomInfo))]
public class RoomEditor : Editor
{
    //// internal
    //bool advancedFoldout = false;
    

    // from roomInfo
    SerializedProperty roomID;
    SerializedProperty roomType;

    void OnEnable()
    {
        roomID = serializedObject.FindProperty("roomID");
        roomType = serializedObject.FindProperty("roomType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(roomID);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(roomType);

        //advancedFoldout = EditorGUILayout.Foldout(advancedFoldout, "Advanced");
        //if (advancedFoldout) // fold down
        //{

        //}

        serializedObject.ApplyModifiedProperties();
    }
}


[System.Serializable]
public class RoomInfo : MonoBehaviour
{
    [SerializeField]
    public int roomID = -1;

    [SerializeField]
    public RoomType roomType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public enum RoomType
    {
        Unnasigned,
        Normal,

    }
}
