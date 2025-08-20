using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGeneration : MonoBehaviour
{
    public MiniMapController miniMap;
    public Tilemap tilemap;
    public TileBase tileToPlace;

    public Vector2Int currentRoomPos;

    private int _width, _height, _size;

    private void Start()
    {
        GenerateFloor();
    }

    private void Update()
    {
        // Room navigation controls (numpad)
        if (Input.GetKeyDown(KeyCode.Keypad2)) MoveRoom(1, 0);
        else if (Input.GetKeyDown(KeyCode.Keypad8)) MoveRoom(-1, 0);
        else if (Input.GetKeyDown(KeyCode.Keypad6)) MoveRoom(0, 1);
        else if (Input.GetKeyDown(KeyCode.Keypad4)) MoveRoom(0, -1);
        else if (Input.GetKeyDown(KeyCode.Keypad5)) GenerateFloor();
    }

    private void MoveRoom(int deltaX, int deltaY)
    {
        currentRoomPos += new Vector2Int(deltaX, deltaY);
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateRooms(currentRoomPos);
    }

    public void RoomGeneration()
    {
        //GenerateSquareRoom();
        //GenerateLineRoom();
        GenerateCircleRoom();
    }

    public void GenerateFloor()
    {
        RoomManager.Instance.ClearSavedRooms();
        miniMap.ClearRooms();

        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new Vector2Int(x, y);
                RoomGeneration();

                // Randomize room type
                RoomType type = RoomType.Normal;
                if (Random.value < 0.1f) type = RoomType.Boss;
                else if (Random.value < 0.2f) type = RoomType.Shop;
                else if (Random.value < 0.3f) type = RoomType.Hidden;

                RoomManager.Instance.savedRooms[currentRoomPos].roomType = type;

                GenerateRoomWalls();
            }

        // Load all rooms and generate doors
        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new Vector2Int(x, y);
                RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
                GenerateRoomDoors();
            }

        currentRoomPos = Vector2Int.zero;
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateRooms(currentRoomPos);
    }

    public void GenerateRoomDoors()
    {
        if (!RoomManager.Instance.savedRooms.TryGetValue(currentRoomPos, out RoomData currentRoom)) return;

        int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;

        foreach (var tileData in currentRoom.tiles.Values)
        {
            var pos = tileData.position;
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        Vector2Int[] directions =
        {
            new(-1, 0), // Up
            new(1, 0),  // Down
            new(0, 1),  // Right
            new(0, -1)  // Left
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = currentRoomPos + dir;
            if (!RoomManager.Instance.savedRooms.TryGetValue(neighborPos, out RoomData neighborRoom)) continue;
            if (neighborRoom.roomType == RoomType.Hidden) continue;

            Vector2Int doorPos = dir switch
            {
                var d when d == new Vector2Int(-1, 0) => new Vector2Int(0, maxY + 1), // Up
                var d when d == new Vector2Int(1, 0) => new Vector2Int(0, minY - 1),  // Down
                var d when d == new Vector2Int(0, 1) => new Vector2Int(maxX + 1, 0),  // Right
                var d when d == new Vector2Int(0, -1) => new Vector2Int(minX - 1, 0), // Left
                _ => Vector2Int.zero
            };

            if (doorPos != Vector2Int.zero)
                AddTile(doorPos, currentRoomPos);
        }
    }

    public void GenerateRoomWalls()
    {
        if (!RoomManager.Instance.savedRooms.TryGetValue(currentRoomPos, out RoomData room)) return;

        var floorPositions = new HashSet<Vector2Int>();
        foreach (var tileData in room.tiles.Values)
            floorPositions.Add(tileData.position);

        var placedWalls = new HashSet<Vector2Int>();

        foreach (var pos in floorPositions)
        {
            bool hasUp = floorPositions.Contains(pos + Vector2Int.up);
            bool hasDown = floorPositions.Contains(pos + Vector2Int.down);
            bool hasLeft = floorPositions.Contains(pos + Vector2Int.left);
            bool hasRight = floorPositions.Contains(pos + Vector2Int.right);

            if (hasUp && hasDown && hasLeft && hasRight) continue;

            // Top walls
            if (!hasUp)
            {
                TryAddWall(pos + Vector2Int.up, placedWalls);
                TryAddWall(pos + Vector2Int.up * 2, placedWalls);

                TryAddWall(pos + new Vector2Int(-1, 1), placedWalls);
                TryAddWall(pos + new Vector2Int(1, 1), placedWalls);
                TryAddWall(pos + new Vector2Int(-1, 2), placedWalls);
                TryAddWall(pos + new Vector2Int(1, 2), placedWalls);
            }

            // Bottom walls
            if (!hasDown)
            {
                TryAddWall(pos + Vector2Int.down, placedWalls);
                TryAddWall(pos + Vector2Int.down * 2, placedWalls);

                TryAddWall(pos + new Vector2Int(-1, -1), placedWalls);
                TryAddWall(pos + new Vector2Int(1, -1), placedWalls);
                TryAddWall(pos + new Vector2Int(-1, -2), placedWalls);
                TryAddWall(pos + new Vector2Int(1, -2), placedWalls);
            }

            // Left wall
            if (!hasLeft)
                TryAddWall(pos + Vector2Int.left, placedWalls);

            // Right wall
            if (!hasRight)
                TryAddWall(pos + Vector2Int.right, placedWalls);
        }
    }

    private void TryAddWall(Vector2Int pos, HashSet<Vector2Int> placedWalls)
    {
        if (placedWalls.Contains(pos)) return;
        AddTile(pos, currentRoomPos);
        placedWalls.Add(pos);
    }

    private void AddTile(Vector2Int pos, Vector2Int roomPosition)
    {
        var room = RoomManager.Instance.savedRooms[roomPosition];
        if (room.tiles.ContainsKey(pos))
        {
            Debug.LogWarning($"Tile already exists at {pos} in room {roomPosition}");
            return;
        }

        room.tiles[pos] = new TileData { position = pos, tile = tileToPlace };
        Debug.Log($"Placed tile at {pos} in room {roomPosition} with tile {tileToPlace} (Type: {tileToPlace.GetType()})");
    }

    private void GenerateSquareRoom()
    {
        _size = Random.Range(3, currentRoomPos.y + 3);
        var room = new RoomData();

        int halfSize = _size / 2;
        int extra = _size % 2;

        for (int x = -halfSize; x < halfSize + extra; x++)
            for (int y = -halfSize; y < halfSize + extra; y++)
                room.tiles[new Vector2Int(x, y)] = new TileData { position = new Vector2Int(x, y), tile = tileToPlace };

        RoomManager.Instance.savedRooms[currentRoomPos] = room;
    }

    private void GenerateLineRoom()
    {
        if (Random.Range(0, 2) == 1)
        {
            _width = Random.Range(5, 7);
            _height = Random.Range(10, 21);
        }
        else
        {
            _width = Random.Range(10, 21);
            _height = Random.Range(5, 7);
        }

        var room = new RoomData();

        int halfWidth = _width / 2;
        int widthExtra = _width % 2;
        int halfHeight = _height / 2;
        int heightExtra = _height % 2;

        for (int x = -halfWidth; x < halfWidth + widthExtra; x++)
            for (int y = -halfHeight; y < halfHeight + heightExtra; y++)
                room.tiles[new Vector2Int(x, y)] = new TileData { position = new Vector2Int(x, y), tile = tileToPlace };

        RoomManager.Instance.savedRooms[currentRoomPos] = room;
    }

    private void GenerateCircleRoom()
    {
        var room = new RoomData();
        int radius = Random.Range(4, 7);

        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                if (x * x + y * y <= radius * radius)
                    room.tiles[new Vector2Int(x, y)] = new TileData { position = new Vector2Int(x, y), tile = tileToPlace };

        RoomManager.Instance.savedRooms[currentRoomPos] = room;
    }
}
