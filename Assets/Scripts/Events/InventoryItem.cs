using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Item item;

    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    private Canvas canvas; // Reference to the canvas
    private RectTransform rectTransform;

    public void InitialiseItem(Item newItem) 
    { 
        item = newItem;
        image.sprite = newItem.image;
    }

    private void Start()
    {
        InitialiseItem(item);
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("DraggableItem must be a child of a Canvas.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
