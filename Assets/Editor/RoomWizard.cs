using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RoomWizard : ScriptableWizard
{
    //[SerializeField]
    //string Title;

    [SerializeField]
    RoomInfo.RoomType roomType = RoomInfo.RoomType.Normal;

    //[SerializeField]
    //string RoomType = RoomInfo.RoomType.Unnasigned;

    [MenuItem ("MyTools/Create Room Wizard...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<RoomWizard>("Create Room", "Create New");//, "Update Selected");

    }

    private void OnWizardCreate()
    {
        bool passTest = true;

        passTest &= roomType != RoomInfo.RoomType.Unnasigned;



        if (passTest)
        {
            for(int i = 0; i < 1; i++)
            { 
                GameObject room = new GameObject();

                DirectoryInfo dir = new DirectoryInfo("Assets/Rooms");
                FileInfo[] fileInfo = dir.GetFiles("*.prefab");

                foreach(var o in fileInfo)
                {
                    Debug.Log(o.Name);
                }

                // avoid ovverwriting by crateing unique names
                string newName;
                int roomNumber;
                string rootName = "Room_";
                if (fileInfo.Length == 0) // there are no rooms
                {
                    roomNumber = 0;
                }
                else
                {
                    string tempName = fileInfo[fileInfo.Length - 1].Name; // get the one at the bottom of the list                   
                    tempName = tempName.Substring(0,tempName.Length - ".prefab".Length); // remove .prefab
                    int lengthOfNumber = tempName.Length - rootName.Length; // get the length of the number at the end of the name
                ///    Debug.Log(tempName);
                    roomNumber = int.Parse(tempName.Substring(rootName.Length, lengthOfNumber)); // get the number at the end of the name
                    roomNumber++; // increment it
                }
                newName = rootName + roomNumber.ToString();

                room.name = newName;
                RoomInfo info = room.AddComponent<RoomInfo>();
                info.roomID = roomNumber;
                info.roomType = roomType;



                bool sucess;
                Object prefab = PrefabUtility.SaveAsPrefabAsset(room, "Assets/Rooms/" + room.name + ".prefab", out sucess);
                Debug.Log(sucess);
                // PrefabUtility.("Assets/Rooms/" + room.name + ".prefab");

                AssetDatabase.OpenAsset(prefab);

                DestroyImmediate(room);
                if (!sucess)
                {
                    Debug.LogError("Prefab not created");
                }
        }
        }
        else
        {
            Debug.LogError("Invalid Room Details:");
        }

           
    }

    private void OnWizardUpdate()
    {
        helpString = "Enter the room details";
    }

}
