using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoving : MonoBehaviour
{
    private Vector3 randomPosition;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed;
    private bool start;

    private float halfGroudSize = 50f;
    // Start is called before the first frame update
    void Start()
    {
        randomPosition = new Vector3(Random.value * (halfGroudSize / 2) * 2 - (halfGroudSize / 2),
                                         0.00f,
                                         Random.value * (halfGroudSize / 2) * 2 - (halfGroudSize / 2));
       // startPosition = new Vector3(14.9f, 1.647177f, 15.8f);
        //endPosition = new Vector3(-34.8f, 1.647177f, 15.8f);
    }
    
    // Update is called once per frame
    void Update()
    {

        movingLtoR();
    }

   

    void movingLtoR()
    {
        if (transform.localPosition == startPosition)
        {
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, endPosition, speed * Time.deltaTime);
            transform.LookAt(endPosition);
            start = true;
        }
        else if(transform.localPosition == endPosition)
        {
            start = false;
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, startPosition, speed * Time.deltaTime);
            transform.LookAt(startPosition);
        }
        else if(start)
        {
            transform.LookAt(endPosition);
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, endPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.LookAt(startPosition);
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, startPosition, speed * Time.deltaTime);
        }
    }
}
