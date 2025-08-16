using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGeneration : MonoBehaviour
{
    public MiniMapController miniMap;
    public Tilemap tilemap;
    public TileBase tileToPlace;
    private Vector2Int currentRoomPos;
    private int width;
    private int height;
    private int loadRoomX;
    private int loadRoomY;

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
                RoomManager.Instance.SaveRoom(tilemap, currentRoomPos);
                tilemap.ClearAllTiles();
            }
        }

        currentRoomPos = new(0, 0);
        RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
        miniMap.UpdateCurrentRoom(currentRoomPos);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            loadRoomX += 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateCurrentRoom(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            loadRoomX -= 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateCurrentRoom(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            loadRoomY += 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateCurrentRoom(currentRoomPos);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            loadRoomY -= 1;
            currentRoomPos = new(loadRoomX, loadRoomY);
            RoomManager.Instance.LoadRoom(tilemap, currentRoomPos);
            miniMap.UpdateCurrentRoom(currentRoomPos);
        }
    }

    public void RoomGeneration()
    {
        for (int x = -width / 2; x < Mathf.RoundToInt(width / 2) + 3; x++)
        {
            for (int y = -height / 2; y < Mathf.RoundToInt(height / 2) + 4; y++)
            {
                Vector3Int pos3D = new(x, y, 0);
                tilemap.SetTile(pos3D, tileToPlace);
            }
        }
    }
}