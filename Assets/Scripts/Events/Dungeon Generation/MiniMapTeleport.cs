using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapTeleport : MonoBehaviour, IPointerClickHandler
{
    private static bool _canTeleport;
    public Vector2Int roomCoordinates;

    public void OnPointerClick(PointerEventData eventData)
    {
        MiniMapController minimap = FindFirstObjectByType<MiniMapController>();
        if (minimap != null && _canTeleport)
        {
            minimap.TeleportToRoom(roomCoordinates);
        }
    }
    public void SetActive(bool active) 
    { 
        _canTeleport = active;
    }
}
