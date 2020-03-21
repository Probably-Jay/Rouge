//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;


////[CustomEditor(typeof(RoomWizard))]
////public class WizardEditor : Editor
////{
////    bool advancedFoldout = false;
////    SerializedProperty roomType;

////    void OnEnable()
////    {
////        roomType = serializedObject.FindProperty("roomType");
////    }
////    public override void OnInspectorGUI()
////    {
////        serializedObject.Update();

     

////        EditorGUILayout.PropertyField(roomType);

////        advancedFoldout = EditorGUILayout.Foldout(advancedFoldout, "Advanced");
////        //if (advancedFoldout) // fold down
////        //{

////        //}

////        serializedObject.ApplyModifiedProperties();
////    }
////}
////[System.Serializable]
//public class RoomWizard : ScriptableWizard
//{
//    //[SerializeField]
//    //string Title;

//    [SerializeField]
//    RoomInfo.RoomType roomType = RoomInfo.RoomType.Normal;

//    //[SerializeField]
//    //string RoomType = RoomInfo.RoomType.Unnasigned;

//  //  GameObject roomPrefab;

//    [MenuItem ("MyTools/New Room Wizard...")]
//    static void CreateWizard()
//    {
//        ScriptableWizard.DisplayWizard<RoomWizard>("Create Room", "Create New", "Update Selected");
//    }
    
//    private void OnWizardCreate()
//    {
        
//        bool passTest = true;

//        passTest &= roomType != RoomInfo.RoomType.Unnasigned;



//        if (passTest)
//        {
            
//            Object source = Resources.Load("Prefabs/Room");
//            GameObject room = (GameObject)PrefabUtility.InstantiatePrefab(source);

//            // avoid ovverwriting by crateing unique names
//            DirectoryInfo dir = new DirectoryInfo("Assets/Rooms");
//            FileInfo[] fileInfos = dir.GetFiles("*.prefab");
//            List<RoomInfo> roominfos = new List<RoomInfo>();
//            string rootName = "Room_";
//            string newName;
//            int roomNumber;
                
//            if (fileInfos.Length == 0) // there are no rooms
//            {
//                roomNumber = 0;
//            }
//            else
//            {
//                int maxRoomNumber = 0;
//                foreach(var o in fileInfos) { 
//                    string tempName = o.Name;                
//                    tempName = tempName.Substring(0,tempName.Length - ".prefab".Length); // remove .prefab
//                    int lengthOfNumber = tempName.Length - rootName.Length; // get the length of the number at the end of the name
//                    roomNumber = int.Parse(tempName.Substring(rootName.Length, lengthOfNumber)); // get the number at the end of the name
//                    maxRoomNumber = roomNumber > maxRoomNumber ? roomNumber : maxRoomNumber; // find max alorithm
//                }
//                roomNumber = maxRoomNumber + 1; // next number
//            }
//            newName = rootName + roomNumber.ToString();
//            room.name = newName;
//            RoomInfo info = room.GetComponent<RoomInfo>();
//            info.roomID = roomNumber;
//            info.roomType = roomType;



//            bool sucess;

            

//            Object prefab = PrefabUtility.SaveAsPrefabAsset(room, "Assets/Rooms/" + room.name + ".prefab", out sucess);
//            if (!sucess){ Debug.LogError("Prefab not created");}
           
//            AssetDatabase.OpenAsset(prefab);
            

//            DestroyImmediate(room);
        
//        }
//        else
//        {
//            Debug.LogError("Invalid Room Details");
//        }

           
//    }

//    private void OnWizardOtherButton()
//    {
//        if (Selection.activeTransform != null)
//        {
//            RoomInfo info = Selection.activeTransform.GetComponent<RoomInfo>();
//            if (info != null)
//            {
//                info.roomType = roomType;
//            }
//        }
//    }

//    private void OnWizardUpdate()
//    {
//        helpString = "Enter the room details:";
//    }

//}
