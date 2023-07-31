using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameObject map;
    private void Awake()
    {
        EncounterManager.OnMapToggle += ToggleMap;
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
        if (map.active)
        {
            CloseMap();
        }else if (!map.active)
        {
            OpenMap();
        }
    }

}
