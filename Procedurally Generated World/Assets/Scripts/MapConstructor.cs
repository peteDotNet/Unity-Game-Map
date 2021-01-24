using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapConstructor : MonoBehaviour
{
    // Start is called before the first frame update

    //public enum DrawMode { NoiseMap, ColourMap, Mesh };
    //public DrawMode drawmode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    //public bool autoUpdate;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public int octaves;
    [Range(0, 1)]
    public float persitence;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public TerrainType[] regions;



 
    public Element[] treeElements;

    public void GenerateMap()
    {


        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persitence, lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }


        MapDisplay display = FindObjectOfType<MapDisplay>();
        var meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve);
        display.DrawMesh(meshData, TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));

        PlaceTrees(meshData);
    }

    private void PlaceTrees(MeshData meshData)
    {
        if (treeElements.Length > 0)
        {
            foreach (Element vegElement in treeElements)
            {
                if (vegElement.prefab != null)
                {
                    var trees = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains(vegElement.prefab.name));

                    foreach (GameObject obj in trees)
                    {
                        DestroyImmediate(obj);
                    }

                    System.Random rnd = new System.Random();
                    IEnumerable<Vector3> allValidTreeLocations = meshData.vertices.Where(x => x.y < vegElement.maxAltitude).Where(x => x.y > vegElement.minAltitude);
                    IEnumerable<Vector3> treeLocations = allValidTreeLocations.OrderBy(x => rnd.Next()).Take(vegElement.count).ToList();
                   
                    foreach (Vector3 treeLocation in treeLocations)
                    {
                        GameObject newElement = Instantiate(vegElement.prefab);
                        newElement.transform.position = treeLocation;
                        newElement.transform.localScale = vegElement.scale;
                    }
                }

            }
        }
    }



    private void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;

        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 1)
            octaves = 1;

    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }

    [System.Serializable]
    public class Element
    {
        public string name;
        public GameObject prefab;
        public float maxAltitude;
        public float minAltitude;
        public int count;
        public Vector3 scale = new Vector3();
    }


}
