using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class RoomMenu : EditorWindow 
{
    //[SerializeField]
    //string Title;

    [SerializeField]
    RoomInfo.RoomType roomType = RoomInfo.RoomType.Normal;

    [SerializeField]    
    int roomID;

    
    static readonly string rootName = "Room_";
    static int nextRoomNumber;

    //[SerializeField]
    //string RoomType = RoomInfo.RoomType.Unnasigned;

    //  GameObject roomPrefab;

    [MenuItem("HelperTools/New Room...")]
    static void Init()
    {
        Object source = Resources.Load("Prefabs/Room");
        GameObject room = (GameObject)PrefabUtility.InstantiatePrefab(source);

        // avoid ovverwriting by crateing unique names
        DirectoryInfo dir = new DirectoryInfo("Assets/Rooms");
        FileInfo[] fileInfos = dir.GetFiles("*.prefab");
        
        if (fileInfos.Length == 0) // there are no rooms
        {
            nextRoomNumber = 0;
        }
        else
        {
            int maxRoomNumber = 0;
            foreach (var o in fileInfos)
            {
                string tempName = o.Name;
                tempName = tempName.Substring(0, tempName.Length - ".prefab".Length); // remove .prefab
                int lengthOfNumber = tempName.Length - rootName.Length; // get the length of the number at the end of the name
                nextRoomNumber = int.Parse(tempName.Substring(rootName.Length, lengthOfNumber)); // get the number at the end of the name
                maxRoomNumber = nextRoomNumber > maxRoomNumber ? nextRoomNumber : maxRoomNumber; // find max alorithm
            }
            nextRoomNumber = maxRoomNumber + 1; // next number
        }

        RoomMenu window = (RoomMenu)EditorWindow.GetWindow(typeof(RoomMenu));
        window.roomID = nextRoomNumber;
        DestroyImmediate(room); // cleanup

        window.Show();
    }

   

    private void OnGUI()
    {
        GUILayout.Label("Create new room", EditorStyles.boldLabel);

        bool allValidInfo = true;

        allValidInfo &= roomType != RoomInfo.RoomType.Unnasigned;

        GUI.enabled = false;
        roomID = EditorGUILayout.IntField("Room ID:", roomID);
        GUI.enabled = true;
        if(roomID  < 0)
        {
            allValidInfo = false;
            EditorGUILayout.HelpBox("Room ID must be positive", MessageType.Error);
        }

        
        roomType = (RoomInfo.RoomType)EditorGUILayout.EnumPopup("Room Type: ", roomType);
        if(roomType == RoomInfo.RoomType.Unnasigned)
        {
            allValidInfo = false;
            EditorGUILayout.HelpBox("Please select a room type", MessageType.None);
        }

            //EditorGUILayout.HelpBox("Invalid Room Details", MessageType.Error);

        if (allValidInfo)
        {
            if (GUILayout.Button("Create New Room"))
            {
                Create();
                Close();
            }
        }
    }

    private void Create()
    {

        Object source = Resources.Load("Prefabs/Room");
        GameObject room = (GameObject)PrefabUtility.InstantiatePrefab(source);

        string newName = rootName + roomID.ToString();
        room.name = newName;
        RoomInfo info = room.GetComponent<RoomInfo>();
        info.roomID = roomID;
        info.roomType = roomType;



        bool sucess;
        Object prefab = PrefabUtility.SaveAsPrefabAsset(room, "Assets/Rooms/" + room.name + ".prefab", out sucess);
        if (!sucess) { Debug.LogError("Prefab not created"); }

        DestroyImmediate(room); // cleanup

        AssetDatabase.OpenAsset(prefab);




    }

}