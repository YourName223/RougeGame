
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public DoorDirection doorDirection;

    private TileGeneration _TileGeneration;

    void Start()
    {
        _TileGeneration = FindFirstObjectByType<TileGeneration>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2Int moveDir = GetDirectionOffset();

            _TileGeneration.currentRoomPos += moveDir;

            RoomManager.Instance.savedRooms.TryGetValue(_TileGeneration.currentRoomPos, out RoomData currentRoom);

            Vector2Int entryDir = new(-moveDir.x, -moveDir.y);
            Vector2Int doorTilePos;

            if (currentRoom.doorPositions.TryGetValue(entryDir, out doorTilePos))
            {
                // Set player to that door position with a small offset
                Transform _player = GameObject.FindWithTag("Player").transform;
                _player.position = new Vector3(doorTilePos.x + 0.5f + moveDir.y * 2, doorTilePos.y + 0.7f - moveDir.x * 3, 0);
            }
            else
            {
                Debug.LogWarning("No door position found for entry direction.");
            }

            RoomManager.Instance.LoadRoom(_TileGeneration.tilemap, _TileGeneration.currentRoomPos);
            _TileGeneration.miniMap.UpdateRooms(_TileGeneration.currentRoomPos);
        }
    }
    private Vector2Int GetDirectionOffset()
    {
        return doorDirection switch
        {
            DoorDirection.Up => new Vector2Int(-1, 0),
            DoorDirection.Down => new Vector2Int(1, 0),
            DoorDirection.Left => new Vector2Int(0, -1),
            DoorDirection.Right => new Vector2Int(0, 1),
            _ => Vector2Int.zero
        };
    }
}
