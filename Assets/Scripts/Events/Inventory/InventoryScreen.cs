using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    public bool active;

    private void Start()
    {
        active = false;
    }
    public void Set()
    {
        if (active)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;
            active = false;
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            active = true;
        }
    }
}
