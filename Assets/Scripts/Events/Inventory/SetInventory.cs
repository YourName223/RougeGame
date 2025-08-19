using UnityEngine;

public class SetInventory : MonoBehaviour
{
    [SerializeField] private PauseScreen pauseScreen;
    [SerializeField] private InventoryScreen inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (pauseScreen.active == true)
            {
                inventory.Set();
                pauseScreen.Set();
                pauseScreen.active = false;
            }
            else
            {
                inventory.Set();
                pauseScreen.Set();
                pauseScreen.active = true;
            }
        }
    }
}
