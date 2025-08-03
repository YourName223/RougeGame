using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public void Set(bool active)
    {
        if (active)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}