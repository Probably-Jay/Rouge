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
    public SerializedProperty tileName;
    SerializedProperty tileType;
    SerializedProperty isSolid;
    SerializedProperty canBeOpened;
    SerializedProperty open;
    SerializedProperty locked;

    //SerializedProperty safeTileType;
    //SerializedProperty TileTypeToSafeEnum;

    //TileInfo.TileTypeEnum currentType = TileInfo.TileTypeEnum.none;

    int currentType;
    void OnEnable()
    {
        tile = serializedObject.FindProperty("tile");
        tileName = serializedObject.FindProperty("tileName");
        tileType = serializedObject.FindProperty("tileType");
        isSolid = serializedObject.FindProperty("isSolid");
        canBeOpened = serializedObject.FindProperty("canBeOpened");
        open = serializedObject.FindProperty("open");
        locked = serializedObject.FindProperty("locked");
        currentType = tileType.intValue;

        //safeTileType = serializedObject.FindProperty("safeTileTypeHack");
        //TileTypeToSafeEnum = serializedObject.FindProperty("TileTypeToSafeEnum");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (currentType != (int)TileInfo.TileTypeEnum.other)
        {

            foreach (TileInfo.TileTypeEnum mask in System.Enum.GetValues(typeof(TileInfo.TileFeature)))
            {
                if (mask == 0)
                {
                    BaseTileEditorDraw();
                    continue;
                }


                int match = currentType & (int)mask;

                //Debug.Log((int)currentType + " " + (int)mask + " " + match);

                switch ((TileInfo.TileFeature)match)
                {
                    case TileInfo.TileFeature.none:
                        break;
                    case TileInfo.TileFeature.walkable:
                        DrawWalkableEditor();
                        break;
                    case TileInfo.TileFeature.solid:
                        DrawSolidEditor();
                        break;
                    case TileInfo.TileFeature.openable:
                        DrawOpenableEditor();
                        break;
                    case TileInfo.TileFeature.lockable:
                        break;
                    case TileInfo.TileFeature.ornament:
                        break;
                    default:
                        EditorGUILayout.HelpBox("This tile feature (" + match + ": " + ((TileInfo.TileFeature)match).ToString() + " is not recognised, did you forget to implement it?", MessageType.None);
                        break;
                }
            }

            if(currentType == (int)TileInfo.TileTypeEnum.none)
            {
                EditorGUILayout.HelpBox("Tile cannot be of type 'none' please use other if type is not defined", MessageType.Warning);
            }
        }
        else
        {
            DrawAllEditor();
        }

        serializedObject.ApplyModifiedProperties();

        if (currentType != tileType.intValue)
        {
            ((TileInfo)serializedObject.targetObject).Polymorph();
            currentType = (int)tileType.intValue;
        }
        //serializedObject.ApplyModifiedProperties();
    }

    private void DrawAllEditor()
    {
        BaseTileEditorDraw();
        EditorGUILayout.PropertyField(isSolid);
        EditorGUILayout.PropertyField(canBeOpened);
        EditorGUILayout.PropertyField(open);
        EditorGUILayout.PropertyField(locked);
    }

    private void BaseTileEditorDraw()
    {
        EditorGUILayout.PropertyField(tile);
        EditorGUILayout.PropertyField(tileName);
        EditorGUILayout.PropertyField(tileType);

        
        //safeTileType.intValue = (int)(TileInfo.TileTypeEnum)EditorGUILayout.EnumPopup(currentType);

    }
    private void DrawWalkableEditor()
    {
        GUI.enabled = false;
        EditorGUILayout.PropertyField(isSolid);
        GUI.enabled = true;
    }

    private void DrawSolidEditor()
    {
        GUI.enabled = false;
        EditorGUILayout.PropertyField(isSolid);
        GUI.enabled = true;
    }


    private void DrawOpenableEditor()
    {
        GUI.enabled = false;
        EditorGUILayout.PropertyField(canBeOpened);
        GUI.enabled = true;
        EditorGUILayout.PropertyField(open);
        EditorGUILayout.PropertyField(locked);
    }

   

}

    [System.Serializable]
[CreateAssetMenu(menuName = "Tile Info") ]
public class TileInfo : ScriptableObject
{
    [SerializeField]
    public Texture2D tile;
    [SerializeField]
    public string tileName;
    [SerializeField]
    public bool isSolid;
    [SerializeField]
    public bool canBeOpened;
    [SerializeField]
    public bool open;
    [SerializeField]
    public bool locked;

    [SerializeField]
    public TileTypeEnum tileType;

    

    [SerializeField]
    public long tileAttributes = 0;

    

    public void Polymorph()
    {
       
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
            case TileTypeEnum.other:
                OtherTilePolymorph();
                break;
            default:
                Debug.LogError("Unknown tile type value: " + tileType + " at tile '"+ tileName+"'");
                break;
        }
    }

    private void OtherTilePolymorph()
    {
        BaseTilePolymorph();
    }

    private void BaseTilePolymorph()
    {
        isSolid = false;
        canBeOpened = false;
        open = false;
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

    private void DecorativeOrnamentTilePolymorph() // simulated inheritance from floor tile
    {
        FloorTilePolymorph();
        
    }

    private void SolidOrnamentTilePolymorph() // simulated inheritance from wall tile
    {
        WallTilePolymorph();
        
    }


    static bool canOpen(TileInfo tile)
    {
        return tile.tileType.HasFlag(TileFeature.openable);    
    }

    private void OnValidate()
    {
        name = this.tileName;
        //tileName = this.name;
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
        floor = TileFeature.walkable,
        wall = TileFeature.solid,
        door = TileFeature.solid | TileFeature.openable | TileFeature.lockable,
        solidOrnament = TileFeature.solid | TileFeature.ornament,
        decorativeOrnament = TileFeature.walkable | TileFeature.ornament,

        other = System.Int32.MaxValue
    }

    [Flags] public enum TileFeature
    {
        none = 0,
        walkable =      (1 << 0),
        solid =         (1 << 1),
        openable =      (1 << 2),
        //open = TileFeature.openable | TileFeature.walkable,
       // closed = TileFeature.openable | TileFeature.solid,
        lockable =      (1 << 3),
        //locked = TileFeature.lockable
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
