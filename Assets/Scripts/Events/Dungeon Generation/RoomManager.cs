using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public Dictionary<Vector2Int, RoomData> savedRooms = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SaveRoom(Tilemap tilemap, Vector2Int roomPosition)
    {
        RoomData room = new();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                Vector2Int pos2D = new(pos.x, pos.y);
                room.tiles.Add(new TileData
                {
                    position = pos2D,
                    tile = tile
                });
            }
        }

        savedRooms[roomPosition] = room;
        Debug.Log($"Room saved at position {roomPosition}. Total rooms: {savedRooms.Count}");
    }

    public void LoadRoom(Tilemap tilemap, Vector2Int roomPosition)
    {
        tilemap.ClearAllTiles();

        if (!savedRooms.ContainsKey(roomPosition))
        {
            Debug.LogWarning($"No room saved at position {roomPosition}!");
            return;
        }

        RoomData room = savedRooms[roomPosition];

        foreach (TileData tileData in room.tiles)
        {
            Vector3Int pos3D = new(tileData.position.x, tileData.position.y, 0);
            tilemap.SetTile(pos3D, tileData.tile);
        }

        Debug.Log($"Room loaded at position {roomPosition}.");
    }
}