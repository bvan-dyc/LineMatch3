using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardController : StateMachine
{
    [Header("BoardData")]
    [SerializeField] private int xSize = 7;
    [SerializeField] private int ySize = 6;
    [SerializeField] private GameObject tilePrefab = null;
    [SerializeField] private GameObject[] blockPrefabs = null;
    [Serializable]
    public class MatchEvent : UnityEvent<int>
    { }
    public MatchEvent OnMatch;

    public static BoardController Instance
    {
        get { return _instance; }
    }
    private static BoardController _instance;
    private Tile[,] tiles;
    private List<Tile> selectedTiles = new List<Tile>();
    private float xTileSize;
    private float yTileSize;
    private float startX;
    private float startY;
    private InputController input;
    private int boardLayerMask;
    private bool isSelecting = false;

    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            throw new UnityException("There cannot be more than one BoardController.  The instances are " + _instance.name + " and " + name + ".");
        xTileSize = tilePrefab.GetComponent<Tile>().Width;
        yTileSize = tilePrefab.GetComponent<Tile>().Height;
        startX = -xTileSize * xSize / 1.5f;
        startY = -yTileSize * ySize / 2f;
        tiles = new Tile[xSize, ySize];
        boardLayerMask = 1 << gameObject.layer;
    }

    public void Start()
    {
        input = InputController.Instance;
        MakeBoard();
    }

    public void Update()
    {
        if (isSelecting)
        {
            CheckForTileAtLocation(Camera.main.ScreenToWorldPoint(input.PressLocation));
        }
    }

    private void MakeBoard()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y] = MakeNewTile(x, y);
            }
        }
        SwitchState<WaitingState>();
    }

    public void SelectTiles(bool isPressed)
    {
        if (isPressed)
        {
            isSelecting = true;
        }
        else
        {
            isSelecting = false;
            ValidateSelection();
        }
    }

    private void CheckForTileAtLocation(Vector3 location)
    {
        RaycastHit2D hit = Physics2D.Raycast(location, Vector2.zero, boardLayerMask);
        if (hit)
        {
            Tile hitTile = hit.collider.gameObject.GetComponent<Tile>();
            Tile selectedTile = selectedTiles.Find(tile => tile.Coordinates == hitTile.Coordinates);
            if (selectedTile == null)
            {
                // If the new tile has not been selected yet and matches with the former tile,
                // we add the new tile to the selection
                if (selectedTiles.Count == 0 || selectedTiles[selectedTiles.Count - 1].TryToMatchWith(hitTile))
                {
                    selectedTiles.Add(hitTile);
                    hitTile.SelectTile();
                }
            }
            else
            {
                // If the tile is already in the selection and is different from the previous one we clean the selection
                int tileIndex = selectedTiles.IndexOf(selectedTile);
                if (tileIndex < selectedTiles.Count - 1)
                {
                    for (int i = selectedTiles.Count - 1; i > tileIndex; i--)
                    {
                        selectedTiles[i].UnselectTile();
                        selectedTiles.Remove(selectedTiles[i]);
                    }
                }
            }
        }
    }

    public void ValidateSelection()
    {
        isSelecting = false;
        int totalMatches = selectedTiles.Count;
        OnMatch.Invoke(totalMatches);
        if (totalMatches >= 3)
        {
            SwitchState<TileDropState>();
            selectedTiles.Sort((p1, p2) => p1.OffsetCoords.y.CompareTo(p2.OffsetCoords.y));
            foreach (Tile matchedTile in selectedTiles)
            {
                DropTiles(matchedTile);
            }
            ClearselectedTiles();
            SwitchState<WaitingState>();
        }
        else
        {
            ClearselectedTiles();
        }
    }

    private void DropTiles(Tile matchedTile)
    {
        int x = matchedTile.OffsetCoords.x;
        matchedTile.DestroyBlock();
        Tile currentTile = matchedTile;
        for (int y = matchedTile.OffsetCoords.y; y < ySize - 1; y++)
        {
            Tile nextTile = tiles[x, y + 1];
            if (!nextTile.IsFree)
                currentTile.AddNewBlock(nextTile.Block);
            else
                currentTile.AddNewBlock(MakeRandomBlock());
            currentTile = nextTile;
        }
        tiles[x, ySize - 1].AddNewBlock(MakeRandomBlock());
    }

    private void ClearselectedTiles()
    {
        foreach(Tile tile in selectedTiles)
        {
            tile.UnselectTile();
        }
        selectedTiles.Clear();
    }

    private Tile MakeNewTile(int x, int y)
    {
        Tile newTile = Instantiate(tilePrefab, transform).GetComponent<Tile>();

        newTile.Coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        newTile.OffsetCoords = new Point(x, y);
        newTile.transform.position = new Vector3(
                    startX + xTileSize * 1.5f * x,
                    startY + (x % 2) * yTileSize + yTileSize * 2f * y,
                    transform.position.z);
        newTile.AddNewBlock(MakeRandomBlock());
        return (newTile);
    }

    private Block MakeRandomBlock()
    {
        int index = UnityEngine.Random.Range(0, blockPrefabs.Length);
        GameObject newBlock = GameObject.Instantiate(blockPrefabs[index], transform);
        return (newBlock.GetComponent<Block>());
    }
}
