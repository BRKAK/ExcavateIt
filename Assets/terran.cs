using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terran : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int a = 0;
        a += 10;
        GetComponent<Terrain>().terrainData.size.Set(a,0,0);
    }
}
