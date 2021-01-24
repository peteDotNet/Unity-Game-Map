using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    public int forestSize = 25;
    public int forestSpacing = 3;
    public Vector3 scale = new Vector3();

    public Element[] elements;

    private void Start()
    {
        for (int x = 0; x < forestSize; x += forestSpacing)
        {
            for(int z = 0; z < forestSize; z += forestSpacing)
            {
                Element element = elements[0];
                Vector3 postiion = new Vector3(x, 0f, z);

                GameObject newElement = Instantiate(element.prefab);

                newElement.transform.position = postiion;
                newElement.transform.localScale = scale;
            }
        }
    }


}

[System.Serializable]
public class Element
{
    public string name;
    public GameObject prefab;
}
