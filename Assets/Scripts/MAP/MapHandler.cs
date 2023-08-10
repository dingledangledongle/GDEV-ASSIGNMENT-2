using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameObject map;
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        eventManager.AddListener(Event.MAP_NODE_CLICKED, ToggleMap);
        eventManager.AddListener(Event.RAND_EVENT_END, ToggleMap); 
    }

    private void OpenMap()
    {
        //play animation?
        map.SetActive(true);
    }

    private void CloseMap()
    {
        //play animation?

        map.SetActive(false);
        
    }
    public void ToggleMap()
    {
        if (map.activeInHierarchy)
        {
            CloseMap();
        }else if (!map.activeInHierarchy)
        {
            OpenMap();
        }
    }

}
