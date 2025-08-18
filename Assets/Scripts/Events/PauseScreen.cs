using UnityEngine;

public class PauseScreen : MonoBehaviour
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
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}