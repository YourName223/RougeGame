using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics; // For Stopwatch

public class TileGeneration : MonoBehaviour
{
    public MiniMapController miniMap;
    public Tilemap tilemap;
    public TileBase tileToPlace;
    public Vector2Int currentRoomPos;
    public int minimapwidth;
    public int minimapheight;

    private int _size;
    private Stopwatch stopwatch;

    private void Start()
    {
        stopwatch = new Stopwatch();
        GenerateFloor(); 
    }
    private void Update()
    {
        // Room navigation controls (numpad)
        if (Input.GetKeyDown(KeyCode.Keypad2)) miniMap.TeleportToRoom(currentRoomPos + Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.Keypad8)) miniMap.TeleportToRoom(currentRoomPos + Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.Keypad6)) miniMap.TeleportToRoom(currentRoomPos + Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.Keypad4)) miniMap.TeleportToRoom(currentRoomPos + Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            stopwatch.Restart();
            stopwatch.Start();
            GenerateFloor();
            stopwatch.Stop();
            UnityEngine.Debug.Log("Time taken: " + stopwatch.ElapsedMilliseconds + " ms");
        }
    }
    private void GenerateFloor()
    {
        RoomManager.Instance.ClearSavedRooms();
        miniMap.ClearRooms();

        //Generate room floor and walls
        for (int x = 0; x < minimapwidth; x++)
            for (int y = 0; y < minimapheight; y++)
            {
                currentRoomPos = new Vector2Int(x, y);
                RoomGeneration();

                GenerateRoomWalls();

                float rand = Random.value;
                if (rand < 0.1f) RoomManager.Instance.savedRooms[currentRoomPos].roomType = RoomType.Boss;
                else if (rand < 0.2f) RoomManager.Instance.savedRooms[currentRoomPos].roomType = RoomType.Shop;
                else if (rand < 0.3f) RoomManager.Instance.savedRooms[currentRoomPos].roomType = RoomType.Hidden;
                else RoomManager.Instance.savedRooms[currentRoomPos].roomType = RoomType.Normal;
            }

        //Generate doors and room type
        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new Vector2Int(x, y);
                GenerateRoomDoors();
            }

        currentRoomPos = Vector2Int.zero;
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateRooms(currentRoomPos);
    }
    private void RoomGeneration()
    {
        var room = new RoomData();
        switch (Random.Range(1, 12))
        {
            case 1:
                GenerateSquareRoom(room);
                break;
            case 2:
                GenerateLineRoom(room);
                break;
            case 3:
                GenerateCircleRoom(room);
                break;
            case 4:
                GenerateTwoSquareRoom(room);
                break;
            case 5:
                GenerateCrossRoom(room);
                break;
            case 6:
                GenerateCrossMiddleRoom(room);
                break;
            case 7:
                GenerateCrossEndsRoom(room);
                break;
            case 8:
                GenerateLadderRoom(room);
                break;
            case 9:
                GenerateCrossCircleRoom(room);
                break;
            case 10:
                GenerateMNRoom(room);
                break;
            case 11:
                GenerateTwoLineRoom(room);
                break;
        }
        RoomManager.Instance.savedRooms[currentRoomPos] = room;
    }
    private void GenerateRoomWalls()
    {
        RoomManager.Instance.savedRooms.TryGetValue(currentRoomPos, out RoomData room);

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
                TryAddTile(pos + Vector2Int.up, placedWalls);
                TryAddTile(pos + Vector2Int.up * 2, placedWalls);

                TryAddTile(pos + new Vector2Int(-1, 1), placedWalls);
                TryAddTile(pos + new Vector2Int(1, 1), placedWalls);
                TryAddTile(pos + new Vector2Int(-1, 2), placedWalls);
                TryAddTile(pos + new Vector2Int(1, 2), placedWalls);
            }

            // Bottom walls
            if (!hasDown)
            {
                TryAddTile(pos + Vector2Int.down, placedWalls);
                TryAddTile(pos + Vector2Int.down * 2, placedWalls);

                TryAddTile(pos + new Vector2Int(-1, -1), placedWalls);
                TryAddTile(pos + new Vector2Int(1, -1), placedWalls);
                TryAddTile(pos + new Vector2Int(-1, -2), placedWalls);
                TryAddTile(pos + new Vector2Int(1, -2), placedWalls);
            }

            // Left wall
            if (!hasLeft)
                TryAddTile(pos + Vector2Int.left, placedWalls);

            // Right wall
            if (!hasRight)
                TryAddTile(pos + Vector2Int.right, placedWalls);
        }
    }
    private void GenerateRoomDoors()
    {

        RoomManager.Instance.savedRooms.TryGetValue(currentRoomPos, out RoomData currentRoom);

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
            if (currentRoom.roomType == RoomType.Hidden) return;

            Vector2Int doorPos = dir switch
            {
                { x: -1, y: 0 } => new Vector2Int(0, maxY + 1),   // Up
                { x: 1, y: 0 } => new Vector2Int(0, minY - 1),    // Down
                { x: 0, y: 1 } => new Vector2Int(maxX + 1, 0),    // Right
                { x: 0, y: -1 } => new Vector2Int(minX - 1, 0),   // Left
                _ => Vector2Int.zero
            };

            if (doorPos != Vector2Int.zero) 
            {
                AddTile(doorPos, currentRoomPos);
                currentRoom.doorPositions[dir] = doorPos; // Save door position
            }
        }
    }
    private void TryAddTile(Vector2Int pos, HashSet<Vector2Int> placedTile)
    {
        if (!placedTile.Contains(pos))
        {
            AddTile(pos, currentRoomPos);
            placedTile.Add(pos);
        }
    }
    private void AddTile(Vector2Int pos, Vector2Int roomPosition)
    {
        var room = RoomManager.Instance.savedRooms[roomPosition];

        room.tiles[pos] = new TileData { position = pos, tile = tileToPlace };
    }
    private void GenerateSquareRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.Square;
        _size = Random.Range(15, 15);
        AddBoxTiles(_size, _size, room, Vector2Int.zero);
    }
    private void GenerateTwoSquareRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.TwoSquare;
        _size = 9;

        int halfSize = _size / 2;
        int extra = _size % 2;

        Vector2Int offSet = new(halfSize - 1, halfSize - 1);

        for (int i = 0; i < 2; i++)
        {
            AddBoxTiles(_size, _size, room, offSet);
            offSet = -offSet;
        }
    }
    private void GenerateLineRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.Line;
        if (Random.Range(0, 2) == 1)
        {
            AddBoxTiles(5, 15, room, Vector2Int.zero);
        }
        else
        {
            AddBoxTiles(15, 5, room, Vector2Int.zero);
        }
    }
    private void GenerateCircleRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.Circle;
        int radius = Random.Range(5, 9);

        AddCircleTiles(radius, room, Vector2Int.zero);
    }
    private void GenerateCrossRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.Cross;
        AddBoxTiles(5, 25, room, Vector2Int.zero);
        AddBoxTiles(25, 5, room, Vector2Int.zero);
    }
    private void GenerateCrossEndsRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.CrossEnds;
        AddBoxTiles(3, 21, room, Vector2Int.zero);
        AddBoxTiles(21, 3, room, Vector2Int.zero);
        Vector2Int[] offsets = new Vector2Int[]
        {
            new(-11, 0),  // Left
            new(11, 0),   // Right
            new(0, 11),   // Up
            new(0, -11),  // Down
        };
        for (int i = 0; i < 4; i++)
        {
            AddBoxTiles(9, 9, room, offsets[i]);
        }
    }
    private void GenerateCrossMiddleRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.CrossMiddle;
        if (currentRoomPos.x == 0)
        {
            AddBoxTiles(3, 13, room, new Vector2Int(0, -5));
        }
        else if (currentRoomPos.x == minimapwidth)
        {
            AddBoxTiles(3, 13, room, new Vector2Int(0, 5));
        }
        else
        {
            AddBoxTiles(3, 13, room, new Vector2Int(0, 0));
        }
        if (currentRoomPos.y == 0)
        {
            AddBoxTiles(13, 3, room, new Vector2Int(5, 0));
        }
        else if (currentRoomPos.y == minimapheight)
        {
            AddBoxTiles(13, 3, room, new Vector2Int(-5, 0));
        }
        else
        {
            AddBoxTiles(13, 3, room, new Vector2Int(0, 0));
        }
        AddBoxTiles(9, 9, room, Vector2Int.zero);
    }
    private void GenerateLadderRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.Ladder;
        if (Random.Range(0, 2) == 1)
        {
            AddBoxTiles(3, 25, room, Vector2Int.zero);
            AddBoxTiles(9, 5, room, Vector2Int.zero);
            AddBoxTiles(9, 5, room, new Vector2Int(0, 10));
            AddBoxTiles(9, 5, room, new Vector2Int(0, -10));
        }
        else
        {
            AddBoxTiles(25, 3, room, Vector2Int.zero);
            AddBoxTiles(5, 9, room, Vector2Int.zero);
            AddBoxTiles(5, 9, room, new Vector2Int(10, 0));
            AddBoxTiles(5, 9, room, new Vector2Int(-10, 0));
        }
    }
    private void GenerateCrossCircleRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.CrossCircle;
        AddBoxTiles(3, 25, room, Vector2Int.zero);
        AddBoxTiles(25, 3, room, Vector2Int.zero);
        AddCircleTiles(6, room, Vector2Int.zero);
    }
    private void GenerateMNRoom(RoomData room)
    {
        room.roomLayout = RoomLayout.MN;
        AddBoxTiles(25, 25, room, Vector2Int.zero);
        RemoveBoxTiles(19, 15, room, Vector2Int.zero);
        RemoveBoxTiles(15, 19, room, Vector2Int.zero);
        AddBoxTiles(3, 21, room, Vector2Int.zero);
        AddBoxTiles(21, 3, room, Vector2Int.zero);
        AddBoxTiles(9, 9, room, Vector2Int.zero);
    }
    private void GenerateTwoLineRoom(RoomData room) 
    {
        room.roomLayout = RoomLayout.TwoLine;
        if (Random.Range(0, 2) == 1)
        {
            AddBoxTiles(5, 13, room, new Vector2Int(1, 4)); 
            AddBoxTiles(5, 13, room, new Vector2Int(-1, -4));
        }
        else
        {
            AddBoxTiles(13, 5, room, new Vector2Int(4, 1));
            AddBoxTiles(13, 5, room, new Vector2Int(-4, -1));
        }
    }
    private void AddBoxTiles(int width, int height, RoomData room, Vector2Int offSet) 
    {
        int halfWidth = width / 2;
        int widthExtra = width % 2;
        int halfHeight = height / 2;
        int heightExtra = height % 2;

        for (int x = -halfWidth + offSet.x; x < halfWidth + widthExtra + offSet.x; x++)
            for (int y = -halfHeight + offSet.y; y < halfHeight + heightExtra + offSet.y; y++)
                room.tiles[new Vector2Int(x, y)] = new TileData { position = new Vector2Int(x, y), tile = tileToPlace };
    }
    private void AddCircleTiles(int radius, RoomData room, Vector2Int offSet)
    {
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                if (x * x + y * y <= radius * radius)
                    room.tiles[new Vector2Int(x, y)] = new TileData { position = new Vector2Int(x, y), tile = tileToPlace };
    }
    private void RemoveBoxTiles(int width, int height, RoomData room, Vector2Int offSet)
    {
        int halfWidth = width / 2;
        int widthExtra = width % 2;
        int halfHeight = height / 2;
        int heightExtra = height % 2;

        for (int x = -halfWidth + offSet.x; x < halfWidth + widthExtra + offSet.x; x++)
            for (int y = -halfHeight + offSet.y; y < halfHeight + heightExtra + offSet.y; y++)
                room.tiles.Remove(new Vector2Int(x, y));
    }
    private void RemoveCircleTiles(int radius, RoomData room, Vector2Int offSet)
    {
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                if (x * x + y * y <= radius * radius)
                    room.tiles.Remove(new Vector2Int(x, y));
    }
}
