using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] InventorySlots;
    public GameObject InventoryItemPrefab;


    public void AddItem(Item item) 
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];

            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    void SpawnNewItem(Item item, InventorySlot slot) 
    { 
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
