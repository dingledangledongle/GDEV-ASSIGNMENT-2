using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;
    [SerializeField] private int seed;
    public RandSeedManager randSeed;
    private GameObject[] tilesArray;

    public void GenerateGrid()
    {
        randSeed.GenerateSeed(); //setting the seed for the random number generator
        DeleteGrid(); //resetting the grid if a current grid exists

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int randNum = Random.Range(0, 6);
                var spawnedTile = Instantiate(tilePrefab, new Vector3(-32 + x * 10, -72 + y * 10), Quaternion.identity);
                spawnedTile.transform.SetParent(GameObject.FindGameObjectWithTag("MapScroll").transform, false);
                spawnedTile.name = $"Node-{x}-{y}";
                spawnedTile.Init(randNum);
            }
        }

    }

    void DeleteGrid()
    {
        tilesArray = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var item in tilesArray)
        {
            Destroy(item);
        }

    }

}
