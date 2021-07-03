using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepRandomMoving : MonoBehaviour
{
    public bool bekilled;
    public float speed;
    public float moveRange;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool start;
    // Start is called before the first frame update
    void Start()
    {
        resetPos();
    }

    // Update is called once per frame
    void Update()
    {
        movingLtoR();
        if (bekilled)
        {
            resetPos();
            bekilled = false;
        }
    }

    void movingLtoR()
    {
        if (transform.localPosition == startPosition)
        {
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, endPosition, speed * Time.deltaTime);
            transform.LookAt(endPosition);
            start = true;
        }
        else if (transform.localPosition == endPosition)
        {
            start = false;
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, startPosition, speed * Time.deltaTime);
            transform.LookAt(startPosition);
        }
        else if (start)
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

    public void OnCollisionExit(Collision collision)
    {
        
            resetPos();
        
    }

    

    void resetPos()
    {
        startPosition = transform.position;
        //endPosition = Quaternion.identity * this.transform.forward.normalized * moveRange;
        endPosition = new Vector3(transform.position.x + Random.value * moveRange, 0, transform.position.z + Random.value * moveRange);
    }
}
