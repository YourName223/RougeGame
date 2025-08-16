using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMapController : MonoBehaviour
{
    public GameObject minimapCellPrefab; // The prefab for each cell (UI Image)
    public Transform minimapGridParent;  // The panel with GridLayoutGroup component
    public int mapWidth = 5;
    public int mapHeight = 5;
    private Vector2Int previousRoomPos = new Vector2Int(-1, -1);
    private Dictionary<Vector2Int, Image> roomCells = new();
    private Vector2Int currentRoomPos;

    void Start()
    {
        // Generate the minimap grid cells
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                GameObject cellGO = Instantiate(minimapCellPrefab, minimapGridParent);
                Image cellImage = cellGO.GetComponent<Image>();

                Vector2Int roomPos = new(x, y);
                roomCells.Add(roomPos, cellImage);
            }
        }
    }

    // Call this whenever player changes rooms
    public void UpdateCurrentRoom(Vector2Int newRoomPos)
    {
        if (previousRoomPos.x >= 0 && previousRoomPos.y >= 0)
        {
            if (roomCells.TryGetValue(previousRoomPos, out Image oldCell))
            {
                oldCell.color = Color.white;
            }
        }

        currentRoomPos = newRoomPos;

        // Highlight new current room
        if (roomCells.TryGetValue(currentRoomPos, out var newCell))
        {
            newCell.color = Color.green; // Highlight color
        }

        // Define the 4 neighbors (up, down, left, right)
        Vector2Int[] neighbors = new Vector2Int[]
        {
        new Vector2Int(currentRoomPos.x + 1, currentRoomPos.y),
        new Vector2Int(currentRoomPos.x - 1, currentRoomPos.y),
        new Vector2Int(currentRoomPos.x, currentRoomPos.y + 1),
        new Vector2Int(currentRoomPos.x, currentRoomPos.y - 1),
        };

        // Color neighbors gray if they exist
        foreach (var neighborPos in neighbors)
        {
            if (roomCells.TryGetValue(neighborPos, out var neighborCell))
            {
                if (neighborCell.color.a == 0f)
                {
                    neighborCell.color = Color.gray;
                }
            }
        }

        previousRoomPos = currentRoomPos;
    }
}