using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilesMovement : MonoBehaviour, IPointerClickHandler
{
    private int currentX;
    private int currentY;
    private SpriteRenderer spriteRenderer;
    private bool isInRightPosition;
    [SerializeField] private Vector2 rightPosition;
    public int TileIndex { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Set Tile objects position, index and sprite
    public void InitializeTile(int x, int y, int index, Sprite sprite)
    {
        SetPosition(x, y);
        rightPosition = new Vector2(x, y);
        TileIndex = index;
        spriteRenderer.sprite = sprite;
    }

    // Update Tile objects position
    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
        transform.position = new Vector2(x, y);
        CheckPosition();
    }

    public void CheckPosition()
    {
        bool isRightNow = (rightPosition == new Vector2(transform.position.x, transform.position.y));

        if (isRightNow != isInRightPosition)
        {
            isInRightPosition = isRightNow;
            UiManager.UiManagerInstance.ValidateTilePosition(isInRightPosition);
        }
    }

    // Checks for suitable for moving
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.GameManagerInstance.CheckTilesPosition(currentX, currentY, this.gameObject);
    }
}
