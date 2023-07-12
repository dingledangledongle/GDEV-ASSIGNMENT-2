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
        int seed = (int)System.DateTime.Now.Ticks; //using system ticks to get a random number to use as seed
        currentSeed = seed.ToString();
        displaySeed();
        Random.InitState(seed); // set the seed for the random number generator
    }
    public void SetSeed(int seed)
    {
        currentSeed = seed.ToString();
        Random.InitState(seed);
        displaySeed();
    }

    public void displaySeed()
    {
        seedText.text = "Seed : " + currentSeed;
    }
}
