
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

            Transform _player = GameObject.FindWithTag("Player").transform;
            _player.position = new Vector3(0.5f, 0.5f, 0);
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
