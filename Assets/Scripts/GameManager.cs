using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{

    // shh singleton good sometimes don;t belive the lieees
    
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [SerializeField]
    private Grid mainGrid;
    [SerializeField]
    private GameObject tilePalletObject;
    private Grid tilePallet;

    public Grid MainGrid { get => mainGrid;}

    private TilemapManager tilemapManager;
    public TilemapManager TilemapManager { get => tilemapManager; private set => tilemapManager = value;  }
    public Grid TilePallet { get => tilePallet; }

    // Start is called before the first frame update
    void Start()
    {
        tilemapManager = mainGrid.GetComponentInChildren<TilemapManager>();
        tilePallet = tilePalletObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
