using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public Tilemap tilemap;
    public TileBase tileToPlace;

    public Dictionary<Vector2Int, RoomData> savedRooms = new();

    [SerializeField] private EnemySpawner _enemySpawner;

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
        ClearInstantiatedObjects();
        tilemap.ClearAllTiles();

        if (!savedRooms.ContainsKey(roomPosition))
            return;

        RoomData room = savedRooms[roomPosition];

        foreach (var tileData in room.tiles.Values)
        {
            Vector3Int pos = new(tileData.position.x, tileData.position.y, 0);
            tilemap.SetTile(pos, tileData.tile);
        }

        _enemySpawner.SpawnEnemies(savedRooms[roomPosition].roomLayout);
    }
    public void ClearInstantiatedObjects()
    {
        for (int i = tilemap.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = tilemap.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}