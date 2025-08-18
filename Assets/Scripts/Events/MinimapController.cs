using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public GameObject minimapCellPrefab;
    public Transform minimapGridParent;
    public int mapWidth;
    public int mapHeight;
    public Sprite normal;
    public Sprite boss;
    public Sprite shop;
    public Sprite hidden;
    public Sprite current;

    private Vector2Int _previousRoomPos;
    private Dictionary<Vector2Int, Image> _roomCells;
    private Vector2Int _currentRoomPos;
    private Sprite _sprite;

    void Start()
    {
        _roomCells = new();
        _currentRoomPos = new(0,0);
        _previousRoomPos = new Vector2Int(-1, -1);
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
                _roomCells.Add(roomPos, cellImage);
            }
        }
    }

    //Updates the minimap
    public void UpdateRooms(Vector2Int newRoomPos)
    {
        DrawLastRoom();

        DrawCurrentRoom(newRoomPos);

        DrawNearbyRooms();

        _previousRoomPos = _currentRoomPos;
    }
    private void DrawLastRoom() 
    {
        if (_previousRoomPos.x >= 0 && _previousRoomPos.y >= 0)
        {
            if (_roomCells.TryGetValue(_previousRoomPos, out Image oldCell))
            {
                GetRoomVisuals(_previousRoomPos, out _sprite);
                oldCell.sprite = _sprite;
                oldCell.color = Color.white;
            }
        }
    }
    private void DrawCurrentRoom(Vector2Int newRoomPos)
    {
        _currentRoomPos = newRoomPos;
        if (_roomCells.TryGetValue(_currentRoomPos, out var newCell))
        {
            newCell.sprite = current;
            newCell.color = Color.white;
        }
    }
    private void DrawNearbyRooms() 
    {
        // Define the 4 neighbors (up, down, left, right)
        Vector2Int[] neighbors = new Vector2Int[]
        {
        new(_currentRoomPos.x + 1, _currentRoomPos.y),
        new(_currentRoomPos.x - 1, _currentRoomPos.y),
        new(_currentRoomPos.x, _currentRoomPos.y + 1),
        new(_currentRoomPos.x, _currentRoomPos.y - 1),
        };

        foreach (var neighborPos in neighbors)
        {
            //Checks if cell is a nearby cell
            if (!_roomCells.TryGetValue(neighborPos, out var neighborCell))
            {
                continue;
            }
            //Checks if the nearby cell is visable
            if (neighborCell.color.a != 0f)
            {
                continue;
            }
            //Find the room type and draws that sprite on the room
            GetRoomVisuals(neighborPos, out _sprite);

            if (_sprite != hidden)
            {
                neighborCell.sprite = _sprite;
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