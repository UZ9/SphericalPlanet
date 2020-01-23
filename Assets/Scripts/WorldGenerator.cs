using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using System.Linq;

public class WorldGenerator : MonoBehaviour
{
    public int radius;

    public Material material;
    public int seed;
    public bool RandomizeSeed = true;

    private List<GameObject> cubes = new List<GameObject>();

    [Button]
    public void GeneratePlanet()
    {
        GenerateCubes();
    }

    void Start()
    {
        //GenerateCubes();
    }


    private void GenerateCubes()
    {
        if (RandomizeSeed) seed = Random.Range(0,10000);

        foreach(GameObject cube in cubes) 
        {
            StartCoroutine(Destroy(cube));
        }

        List<List<CombineInstance>> instances = new List<List<CombineInstance>>() {new List<CombineInstance>()};

        int count = 0;

        cubes.Clear();

        Vector3 center = Vector3.one * radius / 2;
        Debug.Log(center);

        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                for (int z = 0; z < radius; z++)
                {
                    float noiseValue = PlanetNoise.Generate(x * .1f + seed,y * .1f + seed, z * .1f + seed);

                    if (noiseValue >= .5 && (center - new Vector3(x, y, z)).sqrMagnitude < radius / 2 * radius / 2)
                    {
                        GameObject cube = (GameObject)Resources.Load("Prefabs/Cube", typeof(GameObject));
                        cube.transform.position = new Vector3(x, y, z);
                        if (count > 65533) 
                        {
                            instances.Add(new List<CombineInstance>());
                            count = 0;
                        }

                        CombineInstance instance = new CombineInstance();
                        instance.mesh = cube.GetComponent<MeshFilter>().sharedMesh;
                        instance.transform = cube.transform.localToWorldMatrix;
                        instances.Last().Add(instance);
                        //cubes.Add(cube);
                        count += 24;
                    
                    }




                }
            }
        }

        int totalMeshCount = 0;

        foreach (List<CombineInstance> list in instances)
        {
            totalMeshCount++;
            GameObject obj = new GameObject("Procedural Mesh " + totalMeshCount);
            MeshFilter filter = obj.AddComponent<MeshFilter>();
            filter.sharedMesh = new Mesh();
            MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
            renderer.material = material;
            filter.sharedMesh.CombineMeshes(list.ToArray());



            cubes.Add(obj);
        }

        
    }

    

    IEnumerator Destroy(GameObject obj) 
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(obj);
    }
    

}
