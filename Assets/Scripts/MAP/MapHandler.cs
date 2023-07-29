using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapHandler : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameObject map;
    private void Awake()
    {
        NodeObject.OnClick += StartEncounter;
    }
    public void OpenMap()
    {
        if (map.active)
        {
            map.SetActive(false);
        }else if (!map.active)
        {
            map.SetActive(true);
        }
    }
    private void StartEncounter(Node node)
    {
        
    }
}
