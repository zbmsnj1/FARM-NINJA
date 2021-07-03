using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;

//mlagents-learn config/trainer_config.yaml --env=C:/Users/yifan/unity_projects/GAIT/buildings/GAIT.exe --run-id=wolf_2 --train
public class WolfAgent : Agent
{
    public Transform sheep;
    public Text eNumber;
    public Text meNumber;
    public float lookAccurate;
    public float lookAngle;
    public float lookRange;
    Rigidbody rBody;
    private bool cSheep;
    private bool cFence;
    private bool cBoss;
    private float halfGroudSize = 18f;
    private float maxTurnSpeed = 90;
    private bool init;
    private float seeSheepDistance;
    private float seeBossDistance;
    private float seeFenceDistance;
    private float seeSheepPD;
    private float seeBossPD;
    private float seeFencePD;
    private float tempDistance;
    private float tempPD;
    private float maxTime;
    private int maxEatNumber;
    private int eatNumber;
    private float rewardNumber = 0;


    private Transform hitTransform;
    private Transform previousTarget;

    private RayP m_RayPer;
    void Start()
    {
        maxEatNumber = 0;
         for(int i = 0; i<20; i++)
        {
            Instantiate(sheep, new Vector3(Random.Range(-(halfGroudSize - 2), (halfGroudSize - 2)),
            0f, Random.Range(-(halfGroudSize - 2), (halfGroudSize - 2))), Quaternion.Euler(new Vector3(0f, Random.Range(0, 360))));
        }

        rBody = GetComponent<Rigidbody>();

        m_RayPer = transform.GetComponent<RayP>();
    }

    
   // public Transform Fence;

    public override void AgentReset()
    {
        eatNumber = 0;

        this.transform.position = new Vector3(Random.Range(-halfGroudSize, halfGroudSize),
        0f, Random.Range(-halfGroudSize, halfGroudSize));
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;

    }

    public override void CollectObservations()
    {
        // Target and Agent positions
        //Vector3 relativePosistion = Target.position - this.transform.position;


        const float rayDistance = 50f;
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        string[] detectableObjects = { "Boss", "Fence", "Sheep" };
        AddVectorObs(m_RayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        var localVelocity = transform.InverseTransformDirection(rBody.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);



    }


    public float speed ;
    private float previousDistance = float.MaxValue;
    public override void AgentAction(float[] vectorAction, string textAction)
    {

        /*float actionSpeed = Mathf.Clamp(vectorAction[1], 0, 1);
        float actionRotation = Mathf.Clamp(vectorAction[0], -1, 1);
        //transform.Translate(Vector3.forward * Time.deltaTime * speed*actionSpeed);
        rBody.velocity = transform.forward * actionSpeed * speed;
        transform.Rotate(0, actionRotation * maxTurnSpeed * Time.deltaTime, 0);*/

        /*Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp( vectorAction[1], -1, 1);
        rBody.AddForce(controlSignal * speed * 100);*/

        //maxTime -= Time.deltaTime / 2f;


        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        if (brain.brainParameters.vectorActionSpaceType == SpaceType.Continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(vectorAction[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(vectorAction[1], -1f, 1f);
        }else
        {
            var forwardAxis = (int)vectorAction[0];
            var rotateAxis = (int)vectorAction[1];


            switch (forwardAxis)
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                case 2:
                    dirToGo = -transform.forward;
                    break;
            }


            switch (rotateAxis)
            {
                case 1:
                    rotateDir = -transform.up;
                    break;
                case 2:
                    rotateDir = transform.up;
                    break;
            }


            rBody.AddForce(dirToGo * speed, ForceMode.VelocityChange);
            transform.Rotate(rotateDir, Time.fixedDeltaTime * maxTurnSpeed);
        }


        if (Look("Sheep"))           
        {
            eatNumber++;
            AddReward(1f);
        }

        if (maxEatNumber < eatNumber)
            maxEatNumber = eatNumber;

        eNumber.text = "EatSheep: " + eatNumber;
        meNumber.text = "MaxEatSheep: " + maxEatNumber;

    }

    public override void AgentOnDone()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sheep"))
        {
            //collision.gameObject.transform.position = new Vector3(Random.value * 90 - 45f, 0f, Random.value * 90 - 45f);
            AddReward(1f);
            cSheep = true;
            
        }else if (collision.gameObject.CompareTag("Fence"))
        {
            SetReward(-1.0f);
            Done();
            cFence = true;
        }
    }


    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sheep"))
        {
            
            cSheep = false;

        }
        else if (collision.gameObject.CompareTag("Fence"))
        {
            Debug.Log("Fence");
            cFence = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetReward(-1.0f);
        Done();
        cBoss = true;
        
    }

    public void OnTriggerExit(Collider other)
    {
        cBoss = false;
       
        
    }

    private bool Look(string target)
    {


        //one line front 
        if(LookAround(Quaternion.identity, Color.green, target))
        {
            return true;
        }
        
            

        //other lines
        float subAngle = (lookAngle / 2) / lookAccurate;
        for (int i = 0; i < lookAccurate; i++)
        {
            if(LookAround(Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), Color.green, target) || LookAround(Quaternion.Euler(0, subAngle * (i + 1), 0), Color.green, target))
            {
                return true;
            }
               
        }

        return false;
    }


    public bool LookAround( Quaternion eulerAnger, Color DebugColor, string target)
    {
        Debug.DrawRay(this.transform.position, eulerAnger * this.transform.forward.normalized * lookRange, DebugColor);

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, eulerAnger * this.transform.forward, out hit, lookRange) && hit.collider.CompareTag(target))
        {
            
            tempPD =  hit.distance /lookRange ;
            
            if (target == "Sheep")
            {
                
                hit.transform.position = new Vector3(Random.Range(-(halfGroudSize-2), (halfGroudSize - 2)),
            0f, Random.Range(-(halfGroudSize - 2), (halfGroudSize - 2)));
                

            }
            return true;
            
        }
        return false;
    }

    

}
