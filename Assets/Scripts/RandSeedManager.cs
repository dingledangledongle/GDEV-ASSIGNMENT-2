using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandSeedManager : MonoBehaviour
{
    string currentSeed;
    public Text seedText;
    public void GenerateSeed()
    {
        int seed = (int)System.DateTime.Now.Ticks;
        currentSeed = seed.ToString();
        displaySeed();
        Random.InitState(seed);
    }
    public void setSeed(int seed)
    {
        currentSeed = seed.ToString();
        Random.InitState(seed);
        displaySeed();
    }

    public void displaySeed()
    {
        seedText.text = "Seed : " + currentSeed;
        Debug.Log(currentSeed);
    }
}
