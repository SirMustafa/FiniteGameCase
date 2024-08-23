using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    [SerializeField] private GameObject puzzlePrefab;
    [SerializeField] private Database database;

    private List<Vector2> availablePositions = new List<Vector2>();
    private List<GameObject> tilesToShuffle = new List<GameObject>();
    private GameObject[,] puzzleGrid;

    private const int gridSizeX = 4;
    private const int gridSizeY = 4;
    private readonly Vector2[] directions =
    {
        new Vector2(-1, 0), // Left
        new Vector2(1, 0),  // Right
        new Vector2(0, 1),  // Up
        new Vector2(0, -1)  // Down
    };

    private void Awake()
    {
        if (GameManagerInstance != null && GameManagerInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            GameManagerInstance = this;
            puzzleGrid = new GameObject[gridSizeX, gridSizeY];
        } 
    }

    private void Start()
    {
        InitializePuzzle();
    }

    // Initializes the puzzle by filling positions, creating tiles, and shuffling them.
    private void InitializePuzzle()
    {
        FillPositions();
        CreateTilesOrder();
        ShuffleTiles();
    }

    // Generates the tiles in order and sets their initial positions.
    private void CreateTilesOrder()
    {
        int tileCount = 0;
        for (int y = gridSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                if (tileCount == gridSizeX * gridSizeY - 1)
                {
                    SetTilePosition(x, y);
                    break;
                }

                CreateTile(x, y, tileCount++);
            }
        }
    }

    // Instantiates a tile and sets its position and properties.
    private void CreateTile(int x, int y, int tileCount)
    {
        GameObject tile = Instantiate(puzzlePrefab, new Vector2(x, y), Quaternion.identity);
        database.AddTileToList(tile);
        tile.GetComponent<TilesMovement>().InitializeTile(x, y, tileCount, database.GetNextPuzzleSprite());
        puzzleGrid[x, y] = tile;
    }

    // Sets the position of the free tile (the empty space in the grid).
    private void SetTilePosition(int x, int y)
    {
        puzzleGrid[x, y] = null;
    }

    // Fills available positions list with all the grid coordinates.
    private void FillPositions()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                availablePositions.Add(new Vector2(x, y));
                database.FillPos(new Vector2(x, y)); // Saves position to the database
            }
        }
    }

    // Shuffles the tiles by randomly repositioning them within the grid.
    private void ShuffleTiles()
    {
        ExtractForShuffling();
        ReassignTilesPositions();
    }

    // Extracts the tiles from the grid to shuffle them.
    private void ExtractForShuffling()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (puzzleGrid[x, y] != null)
                {
                    tilesToShuffle.Add(puzzleGrid[x, y]);
                    puzzleGrid[x, y] = null;
                }
            }
        }
    }

    // Reassigns random positions to the extracted tiles.
    private void ReassignTilesPositions()
    {
        foreach (GameObject tile in tilesToShuffle)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 randomPos = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);

            puzzleGrid[(int)randomPos.x, (int)randomPos.y] = tile;
            tile.transform.position = new Vector2(randomPos.x, randomPos.y);
            tile.GetComponent<TilesMovement>().SetPosition((int)randomPos.x, (int)randomPos.y);
        }

        SetTilePosition((int)availablePositions[0].x, (int)availablePositions[0].y);
    }

    // Checks whether a tile can be moved and moves it if possible.
    public void CheckTilesPosition(int x, int y, GameObject tile)
    {
        foreach (Vector2 direction in directions)
        {
            int newX = x + (int)direction.x;
            int newY = y + (int)direction.y;

            if (IsInBounds(newX, newY) && puzzleGrid[newX, newY] == null)
            {
                UiManager.UiManagerInstance.UpdateMoveCount();
                MoveTile(x, y, newX, newY, tile);
                break;
            }
        }
    }

    // Moves a tile to a new position within the grid.
    private void MoveTile(int oldX, int oldY, int newX, int newY, GameObject tile)
    {
        puzzleGrid[oldX, oldY] = null;
        puzzleGrid[newX, newY] = tile;
        tile.transform.position = new Vector2(newX, newY);
        tile.GetComponent<TilesMovement>().SetPosition(newX, newY);
    }

    // Checks if the given coordinates are within the grid boundaries.
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY;
    }
    public void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}