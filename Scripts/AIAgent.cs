using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using TMPro;


public class AIAgent : Agent
{

    public Animator anim;
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    new private Rigidbody rigidbody;
    private EnvArea envArea;
    public GameObject goal;
    public GameObject win;
    private float initial_distance;
    private float current_distance;
    // Start is called before the first frame update
    public float action;
    public float rotate;
    public override void Initialize()
    {
        base.Initialize();
        envArea = GetComponentInParent<EnvArea>();
        rigidbody = GetComponent<Rigidbody>();
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        float forwardAmount = vectorAction[0];

        // Convert the second action to turning left or right
        float turnAmount = 0f;
        if (vectorAction[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            turnAmount = 1f;
        }

        // Apply movement
        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        action = forwardAmount;
        rotate = turnAmount;
        // Apply a tiny negative reward every step to encourage action

    }

    public override void Heuristic(float[] actionsOut)
    {
       
        int forward = 0;
        int turn = 0;
        if (Input.GetKey(KeyCode.W))
        {
            forward = 1;
            //anim.SetTrigger("Run");
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            forward = -1;
            //anim.SetTrigger("Run");

        }



        if  (Input.GetKey(KeyCode.A))
        {
            turn = 1;
            //anim.SetTrigger("Run");

        }
        else if (Input.GetKey(KeyCode.D))
        {
            turn = 2;
            //anim.SetTrigger("Run");
        }

        actionsOut[0] = forward;
        actionsOut[1] = turn;
    }

    public override void OnEpisodeBegin()
    {
        envArea.ResetArea();
        rigidbody = GetComponent<Rigidbody>();
        Vector3 offset = goal.transform.position.normalized - this.transform.position.normalized;
        float initial_distance = offset.sqrMagnitude;
        //initial_distance = Vector3.Distance(goal.transform.position, transform.position.normalized);
        initial_distance = ret_distance();
    }


    public float ret_distance()
    {
        float distance_EF = Vector3.Distance(goal.transform.position.normalized, this.transform.position.normalized);

        //reward_distance = (starting_distance - distance_EF)/starting_distance; //Negative if it goes far away: max reward is the starting distance

        Vector3 offset = goal.transform.position - this.transform.position;
        float rewardDist = offset.sqrMagnitude;
        //rewardDist = rewardDist/initial_distance;

        rewardDist = 1 - rewardDist * 0.0001f;
        return rewardDist+0.60f;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
       //(1 float = 1 value)
        //sensor.AddObservation(Vector3.Distance(goal.transform.position, transform.position));

        // Direction to goal (1 Vector3 = 3 values)
        //sensor.AddObservation((goal.transform.position - transform.position).normalized);
        sensor.AddObservation(ret_distance());
        // Direction agent is facing (1 Vector3 = 3 values)
        sensor.AddObservation(transform.forward);

        current_distance = ret_distance();
        //AddReward((current_distance-initial_distance) * 0.002f);
         
        
        float exp_reward = Mathf.Exp(3 * (ret_distance() - 1));
        AddReward((exp_reward/6)*0.06f);
        //AddReward(ret_distance() * 0.04f);

        if (ret_distance() > 1.60)
        {
            AddReward(2);
            EndEpisode();
        }

        if (this.transform.position.y < -3)
        {

            AddReward(-1.0f);
            EndEpisode();

        }

        if (MaxStep > 0) AddReward(-2f / MaxStep);
        // 1 + 3 + 3 = 7 total values
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("collider"))
        {
           
            AddReward(-1f);
             EndEpisode();
           
        }
        if (collision.transform.CompareTag("robos"))
        {

            AddReward(-0.5f);
            EndEpisode();

        }
        else if (collision.transform.CompareTag("goal"))
        {
            
            AddReward(2f);
            winR();
            EndEpisode();

        }
    } 
    private void winR()
    {
        GameObject dncr = Instantiate<GameObject>(win);
        dncr.transform.parent = transform.parent;
        dncr.transform.position = goal.transform.position;
        Destroy(dncr, 10f);
        
    }

}
