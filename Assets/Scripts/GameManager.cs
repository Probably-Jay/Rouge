using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Grid MainGrid { get => mainGrid;}

    private TilemapManager tilemapManager;
    public TilemapManager TilemapManager { get => tilemapManager; private set => tilemapManager = value;  }

    // Start is called before the first frame update
    void Start()
    {
        tilemapManager = mainGrid.GetComponentInChildren<TilemapManager>();
        Debug.Log(tilemapManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
