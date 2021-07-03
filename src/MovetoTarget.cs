using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoTarget : MonoBehaviour
{
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, Target.localPosition, 0.5f);
    }
}
