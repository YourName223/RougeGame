using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapTeleport : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2Int roomCoordinates;

    [SerializeField] private GameObject hoverOverlay;
    public MiniMapController minimapController;

    private static bool _canTeleport;
    private static List<MiniMapTeleport> instances = new();
    void Awake()
    {
        instances.Add(this);
    }

    void OnDestroy()
    {
        instances.Remove(this);
    }
    public void Start()
    {
        if (hoverOverlay != null)
            hoverOverlay.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (minimapController != null && _canTeleport && IsCleared())
        {
            minimapController.TeleportToRoom(roomCoordinates);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverOverlay != null && _canTeleport && IsCleared())
            hoverOverlay.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverOverlay != null && _canTeleport && IsCleared())
            hoverOverlay.SetActive(false);
    }

    public void SetActive(bool active) 
    {
        _canTeleport = active;
    }
    public static void HideAllHoverOverlays()
    {
        foreach (var teleport in instances)
        {
            if (teleport.hoverOverlay != null)
                teleport.hoverOverlay.SetActive(false);
        }
    }

    private bool IsCleared()
    {
        if (RoomManager.Instance.savedRooms[roomCoordinates].cleared == true && 
            RoomManager.Instance.savedRooms[minimapController._currentRoomPos].cleared == true)
        {
            return true;
        }
        return false;
    }
    public void SetMinimapController(MiniMapController controller)
    {
        minimapController = controller;
    }
}
