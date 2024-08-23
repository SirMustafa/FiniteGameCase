using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database")]
public class Database : ScriptableObject
{
    [SerializeField] List<Sprite> puzzleSprites = new List<Sprite>();
    [SerializeField] List<Vector2> puzzlePos = new List<Vector2>();
    [SerializeField] List<GameObject> Tiles = new List<GameObject>();
    int currentSpriteIndex;

    public Sprite GetNextPuzzleSprite()
    {
        currentSpriteIndex++;
        return puzzleSprites[currentSpriteIndex];
    }
    public void FillPos(Vector2 pos)
    {
        puzzlePos.Add(pos);
    }
    public void AddTileToList(GameObject obj)
    {
        Tiles.Add(obj);
    }
    public void ClearTiles()
    {
        foreach (GameObject item in Tiles)
        {
            item.SetActive(false);
        }
    }
    public Vector2 GetPositionByIndex(int index)
    {
        return puzzlePos[index];
    }

    public void Clear()
    {
        Tiles.Clear();
        puzzlePos.Clear();
        currentSpriteIndex = -1;
    }

    //Clear lists for new game
    private void OnDisable()
    {
        Clear();
    }
}
