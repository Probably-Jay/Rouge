using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;



[CustomEditor(typeof(TileInfo))]
public class TileEditor : Editor
{

    SerializedProperty tile;
    SerializedProperty tileName;
    SerializedProperty tileType;
    SerializedProperty isSolid;
    SerializedProperty canBeOpened;
    SerializedProperty locked;

    //SerializedProperty safeTileType;
    //SerializedProperty TileTypeToSafeEnum;

    //TileInfo.TileTypeEnum currentType = TileInfo.TileTypeEnum.none;

    int currentType = (int)TileInfo.TileTypeEnum.none;
    void OnEnable()
    {
        tile = serializedObject.FindProperty("tile");
        tileName = serializedObject.FindProperty("name");
        tileType = serializedObject.FindProperty("tileType");
        isSolid = serializedObject.FindProperty("isSolid");
        canBeOpened = serializedObject.FindProperty("canBeOpened");
        locked = serializedObject.FindProperty("locked");

        //safeTileType = serializedObject.FindProperty("safeTileTypeHack");
        //TileTypeToSafeEnum = serializedObject.FindProperty("TileTypeToSafeEnum");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        
        switch ((TileInfo.TileTypeEnum)currentType)
        {
            case TileInfo.TileTypeEnum.none:
                BaseTileEditorDraw();
                EditorGUILayout.HelpBox("Tile type cannot be none", MessageType.Warning);
                break;
            case TileInfo.TileTypeEnum.floor:
                FloorTileEditorDraw();
                break;
            case TileInfo.TileTypeEnum.wall:
                WallTileEditorDraw();
                break;
            case TileInfo.TileTypeEnum.door:
                DoorTileEditorDraw();
                break;
            case TileInfo.TileTypeEnum.solidOrnament:
                SolidOrnamentTileEditorDraw();
                break;
            case TileInfo.TileTypeEnum.decorativeOrnament:
                DecorativeOrnamentTileEditorDraw();
                break;
            default:
                BaseTileEditorDraw();
                EditorGUILayout.HelpBox("Tile type cannot be default, did you forget to implement this case: " + currentType.ToString(), MessageType.Error);
                break;
        }
        

        serializedObject.ApplyModifiedProperties();


        if (currentType != tileType.intValue)
        {
            ((TileInfo)serializedObject.targetObject).Polymorph();
            currentType = (int)tileType.intValue;
        }
        //currentType = (TileInfo.TileTypeEnum)tileType.enumValueIndex;
       
        serializedObject.ApplyModifiedProperties();
       
    }



    private void BaseTileEditorDraw()
    {
        EditorGUILayout.PropertyField(tile);
        EditorGUILayout.PropertyField(tileName);
        EditorGUILayout.PropertyField(tileType);

        
        //safeTileType.intValue = (int)(TileInfo.TileTypeEnum)EditorGUILayout.EnumPopup(currentType);

    }

    private void FloorTileEditorDraw()
    {
        BaseTileEditorDraw();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(isSolid);
        GUI.enabled = true;
    }

    private void WallTileEditorDraw()
    {
        BaseTileEditorDraw();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(isSolid);
        GUI.enabled = true;
    }

    private void DoorTileEditorDraw()
    {
        WallTileEditorDraw();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(canBeOpened);
        GUI.enabled = true;
        EditorGUILayout.PropertyField(locked);
    }

    private void SolidOrnamentTileEditorDraw()
    {
        WallTileEditorDraw();
        throw new NotImplementedException();
    }

    private void DecorativeOrnamentTileEditorDraw()
    {
        FloorTileEditorDraw();
        throw new NotImplementedException();
    }

}


[System.Serializable]
[CreateAssetMenu(menuName = "Tile Info") ]
public class TileInfo : ScriptableObject
{
    [SerializeField]
    public Tile tile;
    [SerializeField]
    public new string name;
    [SerializeField]
    public bool isSolid;
    [SerializeField]
    public bool canBeOpened;
    [SerializeField]
    public bool locked;

    [SerializeField]
    public TileTypeEnum tileType;

    
    //public int safeTileTypeHack;

    [SerializeField]
    public long tileAttributes = 0;

    
    public TileInfo(Tile _tile, string _name, TileTypeEnum type)
    {
        tile = _tile;
        name = _name;

        Polymorph();

    }
    

    public void Polymorph()
    {
       // TileTypeEnum safeIndex = (TileTypeEnum)safeTileTypeHack;
       // Debug.Log(safeIndex);

        //tileType = safeEnumToActualType[safeIndex]; // im sorry

        switch (tileType)
        {
            case TileTypeEnum.none:
                BaseTilePolymorph();
                break;
            case TileTypeEnum.floor:
                FloorTilePolymorph();
                break;
            case TileTypeEnum.wall:
                WallTilePolymorph();
                break;
            case TileTypeEnum.door:
                DoorTilePolymorph();
                break;
            case TileTypeEnum.solidOrnament:
                SolidOrnamentTilePolymorph();
                break;
            case TileTypeEnum.decorativeOrnament:
                DecorativeOrnamentTilePolymorph();
                break;
            default:
                throw new Exception("Unknown tile type value: " + tileType + " "+ name);
                break;
        }
    }

    private void BaseTilePolymorph()
    {
        isSolid = false;
        canBeOpened = false;
        locked = false;
        Debug.Log("Updeted");
    }
    private void FloorTilePolymorph() // simulated inheritance from base tile
    {
        BaseTilePolymorph();
        isSolid = false;
    }

    private void WallTilePolymorph() // simulated inheritance from base tile
    {
        BaseTilePolymorph();
        isSolid = true;
    }

    private void DoorTilePolymorph() // simulated inheritance from wall tile
    {
        WallTilePolymorph();
        canBeOpened = true;
        locked = true;
    }

    private void SolidOrnamentTilePolymorph() // simulated inheritance from wall tile
    {
        WallTilePolymorph();
        throw new NotImplementedException();
    }
    private void DecorativeOrnamentTilePolymorph() // simulated inheritance from floor tile
    {
        BaseTilePolymorph();
        throw new NotImplementedException();
    }


    static bool canOpen(TileInfo tile)
    {
        return tile.tileType.HasFlag(TileBase.openable);    
    }

    //Dictionary<TileTypeEnum, CommonTypeEnum> safeEnumToActualType = new Dictionary<TileTypeEnum, CommonTypeEnum>()
    //{
    //    {TileTypeEnum.none,CommonTypeEnum.none },
    //    {TileTypeEnum.floor,CommonTypeEnum.floor },
    //    {TileTypeEnum.wall,CommonTypeEnum.wall },
    //    {TileTypeEnum.door,CommonTypeEnum.door },
    //    {TileTypeEnum.solidOrnament,CommonTypeEnum.solidOrnament },
    //    {TileTypeEnum.decorativeOrnament,CommonTypeEnum.decorativeOrnament },
    //};
    

    //public enum TileTypeEnum
    //{
    //    none = 0,
    //    floor,
    //    wall,
    //    door,
    //    solidOrnament,
    //    decorativeOrnament,

    //    other,
    //}

    public enum TileTypeEnum
    {
        none = 0,
        floor = TileBase.walkable,
        wall = TileBase.solid,
        door = TileBase.solid | TileBase.openable | TileBase.lockable,
        solidOrnament = TileBase.solid | TileBase.ornament,
        decorativeOrnament = TileBase.walkable | TileBase.ornament,
    }

    [Flags] enum TileBase
    {
        none = 0,
        walkable =      (1 << 0),
        solid =         (1 << 1),
        openable =      (1 << 2),
        lockable =      (1 << 3),
        ornament =      (1 << 4),
    }
}








///[System.Serializable]
//public class TileInfo 
//{
//    public Tile tile;
//    public string tileName;

//    protected bool isSolid;

//    public TileInfo(Tile _tile, string _name, bool _isSolid)
//    {
//        tile = _tile;
//        tileName = _name;
//        isSolid = _isSolid;
//    }

//}

////[System.Serializable]
//public class FloorTile : TileInfo
//{
//    public FloorTile(Tile _tile, string _name) : base(_tile, _name, false)
//    {
//        //isSolid = false;
//    }

//    private new bool isSolid;

//}

////[System.Serializable]
//public class WallTile : TileInfo 
//{
//    public WallTile(Tile _tile, string _name) : base(_tile, _name, true)
//    {
//        //isSolid = true;
//    }

//    protected new bool isSolid;

//}

//[System.Serializable]
//public class DoorTile : WallTile 
//{
//    public DoorTile(Tile _tile, string _name) : base(_tile, _name)
//    {
//    }

//    new bool isSolid;

//}
