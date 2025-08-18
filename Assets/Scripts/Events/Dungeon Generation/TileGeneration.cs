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

        currentRoomPos = new(0, 0);
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateRooms(currentRoomPos);
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
    }

    public void RoomGeneration()
    {
        for (int x = -_width / 2 - 1; x < _width / 2 + (_width % 2) + 1; x++)
        {
            for (int y = -_height / 2 - 2; y < _height / 2 + (_height % 2) + 2; y++)
            {
                _pos3D = new(x, y, 0);
                tilemap.SetTile(_pos3D, tileToPlace);

                //Places a tile outside of the room which will at a door
                if (x == 0 && y == _height / 2 + (_height % 2) + 1)
                {
                    _pos3D = new(x, y + 1, 0);
                    tilemap.SetTile(_pos3D, tileToPlace);
                }
                else if (x == 0 && y == -_height / 2 - 2)
                {
                    _pos3D = new(x, y - 1, 0);
                    tilemap.SetTile(_pos3D, tileToPlace);
                }
                else if (x == _width / 2 + (_width % 2)&& y == 0)
                {
                    _pos3D = new(x + 1, y, 0);
                    tilemap.SetTile(_pos3D, tileToPlace);
                }
                else if (x == -_width / 2 - 1 && y == 0)
                {
                    _pos3D = new(x - 1, y, 0);
                    tilemap.SetTile(_pos3D, tileToPlace);
                }
            }
        }
    }
}