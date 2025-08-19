using UnityEngine;

public class MinimapScreen : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;
    private Vector3 _miniMapScale;
    private Vector3 _miniMapOriginalPos;
    [SerializeField] private MiniMapTeleport _miniMapTeleport;
    [SerializeField] private PauseScreen _pauseScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _miniMapScale = new Vector3(2, 2, 2);
        _miniMapOriginalPos = miniMap.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_pauseScreen.active == true)
            {
                _miniMapTeleport.SetActive(true);
                miniMap.transform.localScale = _miniMapScale;
                miniMap.transform.localPosition = Vector3.zero;
                Time.timeScale = 0;
                _pauseScreen.Set();
            }
            else
            {
                _miniMapTeleport.SetActive(false);
                miniMap.transform.localScale = Vector3.one;
                miniMap.transform.localPosition = _miniMapOriginalPos;
                Time.timeScale = 1;
                MiniMapTeleport.HideAllHoverOverlays();
                _pauseScreen.Set();
            }
        }
    }
}
