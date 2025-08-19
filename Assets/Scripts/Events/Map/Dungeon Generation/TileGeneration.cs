using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class TileGeneration : MonoBehaviour
{
    public MiniMapController miniMap;
    public Tilemap tilemap;
    public TileBase tileToPlace;
    public int loadRoomX;
    public int loadRoomY;
    public Vector2Int currentRoomPos;

    private int _width;
    private int _height;
    private Vector3Int _pos3D;

    void Start()
    {
        GenerateFloor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            loadRoomX += 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateRooms(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            loadRoomX -= 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateRooms(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            loadRoomY += 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateRooms(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            loadRoomY -= 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateRooms(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            GenerateFloor();
        }
    }

    public void RoomGeneration()
    {
        RoomData room = new();
        room.roomType = RoomType.Normal; // Or assign later

        for (int x = -_width / 2; x < _width / 2 + (_width % 2); x++)
        {
            for (int y = -_height / 2; y < _height / 2 + (_height % 2); y++)
            {
                Vector2Int pos2D = new(x, y);
                var existingTile = room.tiles.Find(t => t.position == pos2D);
                if (existingTile != null)
                {
                    existingTile.tile = tileToPlace; // update tile
                }
                else
                {
                    room.tiles.Add(new TileData { position = pos2D, tile = tileToPlace });
                }
            }
        }

        RoomManager.Instance.savedRooms[currentRoomPos] = room;
    }

    public void RoomDoorGeneration() 
    {
        RoomData loadedRoom = RoomManager.Instance.savedRooms[currentRoomPos];

        // Variables for bounds of the room
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        // Find bounds of the room
        foreach (TileData tileData in loadedRoom.tiles)
        {
            Vector2Int pos = tileData.position;
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        // Find room center
        int centerX = (minX + maxX) / 2;
        int centerY = (minY + maxY) / 2;

        // Neighbor directions: Up, Down, Right, Left
        Vector2Int[] directions = new Vector2Int[]
        {
        new(-1, 0),  // Up
        new(1, 0), // Down
        new(0, 1),  // Right
        new(0, -1)  // Left
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = currentRoomPos + dir;

            if (RoomManager.Instance.savedRooms.ContainsKey(neighborPos))
            {
                RoomType neightborType = RoomManager.Instance.savedRooms[neighborPos].roomType;

                RoomData room = RoomManager.Instance.savedRooms[currentRoomPos];

                //Checks if theres a neightbor room, if there is and its not hidden, then place door
                if (dir == new Vector2Int(-1, 0) && neightborType != RoomType.Hidden) // Up
                {
                    Vector2Int doorPos = new(centerX, maxY + 1);
                    var existingTile = room.tiles.Find(t => t.position == doorPos);
                    if (existingTile != null)
                    {
                        existingTile.tile = tileToPlace; // update tile
                    }
                    else
                    {
                        room.tiles.Add(new TileData { position = doorPos, tile = tileToPlace });
                    }
                }
                else if (dir == new Vector2Int(1, 0) && neightborType != RoomType.Hidden) // Down
                {
                    Vector2Int doorPos = new(centerX, minY - 1);
                    var existingTile = room.tiles.Find(t => t.position == doorPos);
                    if (existingTile != null)
                    {
                        existingTile.tile = tileToPlace; // update tile
                    }
                    else
                    {
                        room.tiles.Add(new TileData { position = doorPos, tile = tileToPlace });
                    }
                }
                else if (dir == new Vector2Int(0, 1) && neightborType != RoomType.Hidden) // Right
                {
                    Vector2Int doorPos = new(maxX + 1, centerY);
                    var existingTile = room.tiles.Find(t => t.position == doorPos);
                    if (existingTile != null)
                    {
                        existingTile.tile = tileToPlace; // update tile
                    }
                    else
                    {
                        room.tiles.Add(new TileData { position = doorPos, tile = tileToPlace });
                    }
                }
                else if (dir == new Vector2Int(0, -1) && neightborType != RoomType.Hidden) // Left
                {
                    Vector2Int doorPos = new(minX - 1, centerY);
                    var existingTile = room.tiles.Find(t => t.position == doorPos);
                    if (existingTile != null)
                    {
                        existingTile.tile = tileToPlace; // update tile
                    }
                    else
                    {
                        room.tiles.Add(new TileData { position = doorPos, tile = tileToPlace });
                    }
                }
            }
        }
    }

    private void GenerateFloor() 
    {
        RoomManager.Instance.ClearSavedRooms();
        miniMap.ClearRooms();

        loadRoomY = 0;
        loadRoomX = 0;

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new(x, y);
                _width = Random.Range(5, 6);
                _height = Random.Range(5, 6);
                RoomGeneration();

                RoomType roomType = RoomType.Normal;

                if (Random.value < 0.3f)
                    roomType = RoomType.Hidden;
                if (Random.value < 0.2f)
                    roomType = RoomType.Shop;
                if (Random.value < 0.1f)
                    roomType = RoomType.Boss;

                RoomManager.Instance.savedRooms[currentRoomPos].roomType = roomType;

                RoomWallGeneration();
            }
        }

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new(x, y);

                RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);

                RoomDoorGeneration();
            }
        }

        currentRoomPos = new(0, 0);
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateRooms(currentRoomPos);
    }

    public void RoomWallGeneration()
    {
        RoomData loadedRoom = RoomManager.Instance.savedRooms[currentRoomPos];

        HashSet<Vector2Int> floorPositions = new();
        foreach (TileData tileData in loadedRoom.tiles)
            floorPositions.Add(tileData.position);

        foreach (Vector2Int pos in floorPositions)
        {
            bool hasUp = floorPositions.Contains(pos + Vector2Int.up);
            bool hasDown = floorPositions.Contains(pos + Vector2Int.down);
            bool hasLeft = floorPositions.Contains(pos + Vector2Int.left);
            bool hasRight = floorPositions.Contains(pos + Vector2Int.right);

            if (hasUp && hasDown && hasLeft && hasRight)
                continue;

            if (!hasUp)
            {
                SetWall(pos + Vector2Int.up);
                SetWall(pos + Vector2Int.up * 2);

                // Diagonal upper corners
                SetWall(pos + new Vector2Int(-1, 1));
                SetWall(pos + new Vector2Int(1, 1));
                SetWall(pos + new Vector2Int(-1, 2));
                SetWall(pos + new Vector2Int(1, 2));
            }

            if (!hasDown)
            {
                SetWall(pos + Vector2Int.down);
                SetWall(pos + Vector2Int.down * 2);

                // Diagonal bottom corners
                SetWall(pos + new Vector2Int(-1, -1));
                SetWall(pos + new Vector2Int(1, -1));
                SetWall(pos + new Vector2Int(-1, -2));
                SetWall(pos + new Vector2Int(1, -2));
            }

            if (!hasLeft)
            {
                SetWall(pos + Vector2Int.left);
            }

            if (!hasRight)
            {
                SetWall(pos + Vector2Int.right);
            }
        }
    }

    private void SetWall(Vector2Int pos)
    {
        RoomData room = RoomManager.Instance.savedRooms[currentRoomPos];
        var existingTile = room.tiles.Find(t => t.position == pos);

        if (existingTile != null)
        {
            existingTile.tile = tileToPlace;
        }
        else
        {
            room.tiles.Add(new TileData { position = pos, tile = tileToPlace });
        }
    }
}