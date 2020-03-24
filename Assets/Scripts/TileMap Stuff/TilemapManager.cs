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
    ScriptableObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = ScriptableObject.CreateInstance("TileInfo");
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
