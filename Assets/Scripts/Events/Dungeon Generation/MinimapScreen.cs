using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MinimapScreen : MonoBehaviour
{
    private bool _big;
    public GameObject miniMap;
    private Vector3 MiniMapScale;
    private Vector3 MiniMapOriginalPos;
    public MiniMapTeleport miniMapTeleport;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MiniMapScale = new Vector3(2, 2, 2);
        _big = false;
        MiniMapOriginalPos = miniMap.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_big == true)
            {
                miniMapTeleport.SetActive(true);
                miniMap.transform.localScale = MiniMapScale;
                miniMap.transform.localPosition = Vector3.zero;
                _big = false;
                Time.timeScale = 0;
            }
            else
            {
                miniMapTeleport.SetActive(false);
                miniMap.transform.localScale = Vector3.one;
                miniMap.transform.localPosition = MiniMapOriginalPos;
                _big = true;
                Time.timeScale = 1;
            }
        }
    }
}
