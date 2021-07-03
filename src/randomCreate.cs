using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomCreate : MonoBehaviour
{
    public Transform sheep;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<20; i++)
        {
            Instantiate(sheep, new Vector3(Random.value * 36 - 18f, 0f, Random.value * 36-18f), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
