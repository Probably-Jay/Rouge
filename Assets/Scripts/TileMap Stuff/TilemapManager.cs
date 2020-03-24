using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;



public class TilemapManager : MonoBehaviour
{
    //[SerializeField]
    //TileInfo tile;
    [SerializeField]
    Tilemap tilemap;

    Dictionary<Tile, TileInfo> tileDictionary = new Dictionary<Tile, TileInfo>();

    [SerializeField]
    List<TileInfo> infoVeiw = new List<TileInfo>();

    public Tilemap Tilemap { get => tilemap;}


    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(TileInfo.AssetPath);
        FileInfo[] fileInfos = dir.GetFiles("*.asset");
        
        foreach(FileInfo file in fileInfos)
        {
            TileInfo obj = (TileInfo)AssetDatabase.LoadAssetAtPath<ScriptableObject>(TileInfo.AssetPath+ file.Name);
            tileDictionary.Add((obj).tile, obj);
            infoVeiw.Add((TileInfo)obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }



    public TileInfo GetTileInfo(Tile t)
    {
        return tileDictionary[t];
    }

    public TileInfo GetTileInfo(Vector3Int p)
    {
        Tile t = (Tile)tilemap.GetTile(p);  
        return GetTileInfo(t);
    }
}
