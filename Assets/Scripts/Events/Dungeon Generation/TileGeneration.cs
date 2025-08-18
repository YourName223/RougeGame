using UnityEngine;
using UnityEngine.Tilemaps;

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
        for (int x = -_width / 2 - 1; x < _width / 2 + (_width % 2) + 1; x++)
        {
            for (int y = -_height / 2 - 2; y < _height / 2 + (_height % 2) + 2; y++)
            {
                _pos3D = new(x, y, 0);
                tilemap.SetTile(_pos3D, tileToPlace);
            }
        }
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

                //Checks if theres a neightbor room, if there is and its not hidden, then place door
                if (dir == new Vector2Int(-1, 0) && neightborType != RoomType.Hidden) // Up
                    tilemap.SetTile(new Vector3Int(centerX, maxY + 1, 0), tileToPlace);
                else if (dir == new Vector2Int(1, 0) && neightborType != RoomType.Hidden) // Down
                    tilemap.SetTile(new Vector3Int(centerX, minY - 1, 0), tileToPlace);
                else if (dir == new Vector2Int(0, 1) && neightborType != RoomType.Hidden) // Right
                    tilemap.SetTile(new Vector3Int(maxX + 1, centerY, 0), tileToPlace);
                else if (dir == new Vector2Int(0, -1) && neightborType != RoomType.Hidden) // Left
                    tilemap.SetTile(new Vector3Int(minX - 1, centerY, 0), tileToPlace);
            }
        }
    }

    private void GenerateFloor() 
    {
        tilemap.ClearAllTiles();
        RoomManager.Instance.ClearSavedRooms();
        miniMap.ClearRooms();

        loadRoomY = 0;
        loadRoomX = 0;

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new(x, y);
                _width = Random.Range(3, 8);
                _height = Random.Range(3, 8);
                RoomGeneration();

                RoomType roomType = RoomType.Normal;

                if (Random.value < 0.3f)
                    roomType = RoomType.Hidden;
                if (Random.value < 0.2f)
                    roomType = RoomType.Shop;
                if (Random.value < 0.1f)
                    roomType = RoomType.Boss;

                RoomManager.Instance.SaveRoom(tilemap, currentRoomPos, roomType);
                tilemap.ClearAllTiles();
            }
        }

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                currentRoomPos = new(x, y);

                RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);

                RoomDoorGeneration();

                RoomManager.Instance.SaveRoom(tilemap, currentRoomPos, RoomManager.Instance.savedRooms[currentRoomPos].roomType);
            }
        }


        currentRoomPos = new(0, 0);
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateRooms(currentRoomPos);
    }
}