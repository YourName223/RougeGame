using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public Tilemap tilemap;
    public TileBase tileToPlace;

    public Dictionary<Vector2Int, RoomData> savedRooms = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ClearSavedRooms() 
    { 
        savedRooms.Clear();
    }

    public void LoadRoom(Tilemap tilemap, Vector2Int roomPosition)
    {
        tilemap.ClearAllTiles();

        if (!savedRooms.ContainsKey(roomPosition))
        {
            return;
        }

        RoomData room = savedRooms[roomPosition];

        foreach (TileData tileData in room.tiles)
        {
            Vector3Int pos = new(tileData.position.x, tileData.position.y, 0);
            tilemap.SetTile(pos, tileData.tile);
        }
    }
}