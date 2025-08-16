using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Door;
using static UnityEngine.ParticleSystem;

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

    private TileGeneration TileGeneration;

    void Start()
    {
        TileGeneration = FindObjectOfType<TileGeneration>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2Int moveDir = GetDirectionOffset();

            TileGeneration.loadRoomX += moveDir.x;
            TileGeneration.loadRoomY += moveDir.y;

            Transform _player = GameObject.FindWithTag("Player").transform;
            _player.position = new Vector3(1.5f, 1.5f, 0);

            TileGeneration.currentRoomPos = new(TileGeneration.loadRoomX, TileGeneration.loadRoomY);
            RoomManager.Instance.LoadRoom(TileGeneration.tilemap, TileGeneration.currentRoomPos);
            TileGeneration.miniMap.UpdateRooms(TileGeneration.currentRoomPos);
        }
    }
    private Vector2Int GetDirectionOffset()
    {
        return doorDirection switch
        {
            DoorDirection.Up => new Vector2Int(-1, 0),
            DoorDirection.Down => new Vector2Int(1, 0),
            DoorDirection.Left => new Vector2Int(0, 1),
            DoorDirection.Right => new Vector2Int(0, 1),
            _ => Vector2Int.zero
        };
    }
}
