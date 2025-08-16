using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGeneration : MonoBehaviour
{
    public MiniMapController miniMap;
    public Tilemap tilemap;
    public TileBase tileToPlace;
    public Vector2Int currentRoomPos;
    private int width;
    private int height;
    public int loadRoomX;
    public int loadRoomY;
    private Vector3Int pos3D;

    void Start()
    {
        loadRoomY = 0;
        loadRoomX = 0;

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                width = Random.Range(3, 8);
                height = Random.Range(3, 8);
                RoomGeneration();
                currentRoomPos = new(x, y);

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
        for (int x = -width / 2; x < Mathf.RoundToInt(width / 2) + 3; x++)
        {
            for (int y = -height / 2; y < Mathf.RoundToInt(height / 2) + 5; y++)
            {
                pos3D = new(x, y, 0);
                tilemap.SetTile(pos3D, tileToPlace);
                if (x == 1 && y == Mathf.RoundToInt(height / 2) + 4)
                {
                    pos3D = new(x, y + 1, 0);
                    tilemap.SetTile(pos3D, tileToPlace);
                }
                if (x == 1 && y == -height / 2)
                {
                    pos3D = new(x, y - 1, 0);
                    tilemap.SetTile(pos3D, tileToPlace);
                }
            }
        }
    }
}