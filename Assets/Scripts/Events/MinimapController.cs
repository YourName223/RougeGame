using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MiniMapController : MonoBehaviour
{
    public GameObject minimapCellPrefab; // The prefab for each cell (UI Image)
    public Transform minimapGridParent;  // The panel with GridLayoutGroup component
    public int mapWidth;
    public int mapHeight;
    private Vector2Int previousRoomPos;
    private Dictionary<Vector2Int, Image> roomCells = new();
    private Vector2Int currentRoomPos;
    public Sprite normal;
    public Sprite boss;
    public Sprite shop;
    public Sprite hidden;
    public Sprite current;
    private Sprite sprite;

    void Start()
    {
        Vector2Int currentRoomPos = new Vector2Int(0,0);
        previousRoomPos = new Vector2Int(-1, -1);
        mapWidth = 5;
        mapHeight = 5;
        // Generate the minimap grid cells
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject cellGO = Instantiate(minimapCellPrefab, minimapGridParent);
                Image cellImage = cellGO.GetComponent<Image>();

                Vector2Int roomPos = new(x, y);
                roomCells.Add(roomPos, cellImage);
            }
        }
        UpdateRooms(currentRoomPos);
    }

    // Call this whenever player changes rooms
    public void UpdateRooms(Vector2Int newRoomPos)
    {
        DrawLastRoom();

        DrawCurrentRoom(newRoomPos);

        DrawNearbyRooms();

        previousRoomPos = currentRoomPos;
    }
    private void DrawLastRoom() 
    {
        if (previousRoomPos.x >= 0 && previousRoomPos.y >= 0)
        {
            if (roomCells.TryGetValue(previousRoomPos, out Image oldCell))
            {
                GetRoomVisuals(previousRoomPos, out sprite);
                oldCell.sprite = sprite;
                oldCell.color = Color.white;
            }
        }
    }
    private void DrawCurrentRoom(Vector2Int newRoomPos)
    {
        currentRoomPos = newRoomPos;
        if (roomCells.TryGetValue(currentRoomPos, out var newCell))
        {
            if (roomCells.TryGetValue(currentRoomPos, out Image oldCell))
            {
                newCell.sprite = current;
                newCell.color = Color.white;
            }
        }
    }
    private void DrawNearbyRooms() 
    {
        // Define the 4 neighbors (up, down, left, right)
        Vector2Int[] neighbors = new Vector2Int[]
        {
        new(currentRoomPos.x + 1, currentRoomPos.y),
        new(currentRoomPos.x - 1, currentRoomPos.y),
        new(currentRoomPos.x, currentRoomPos.y + 1),
        new(currentRoomPos.x, currentRoomPos.y - 1),
        };

        foreach (var neighborPos in neighbors)
        {
            //Checks if cell is a nearby cell
            if (!roomCells.TryGetValue(neighborPos, out var neighborCell))
            {
                continue;
            }
            //Checks if the nearby cell is visable
            if (neighborCell.color.a != 0f)
            {
                continue;
            }
            //Find the room type and draws that sprite on the room
            GetRoomVisuals(neighborPos, out sprite);
            if (sprite != hidden)
            {
                neighborCell.sprite = sprite;
                neighborCell.color = new Color(1f, 1f, 1f, 0.5f); // white with 50% alpha
            }
        }
    }
    private void GetRoomVisuals(Vector2Int roomPos, out Sprite sprite)
    {
        sprite = normal;

        if (RoomManager.Instance.savedRooms.TryGetValue(roomPos, out RoomData room))
        {
            switch (room.roomType)
            {
                case RoomType.Boss:
                    sprite = boss;
                    break;

                case RoomType.Shop:
                    sprite = shop;
                    break;

                case RoomType.Hidden:
                    sprite = hidden;
                    break;

                case RoomType.Normal:
                    sprite = normal;
                    break;
            }
        }
    }
}