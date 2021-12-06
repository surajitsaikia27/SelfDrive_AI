using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomRobo : MonoBehaviour
{
    // Start is called before the first frame update


    public float robospeed;
    private float randomizedSpeed = 0f;
    private float nextActionTime = -1f;
    private Vector3 targetPosition;

    private void FixedUpdate()
    {
        if (robospeed > 0f)
        {
            Move();
        }
    }

    private void Move()
    {
        // If it's time for the next action, pick a new speed and destination
        // Else, swim toward the destination
        if (Time.fixedTime >= nextActionTime)
        {
            // Randomize the speed
            randomizedSpeed = robospeed * UnityEngine.Random.Range(.5f, 1.5f);

            // Pick a random target
            targetPosition =EnvArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);

            // Rotate toward the target
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

            // Calculate the time to get there
            float timeToGetThere = Vector3.Distance(transform.position, targetPosition) / randomizedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            // Make sure that the fish does not swim past the target
            Vector3 moveVector = randomizedSpeed * transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(transform.position, targetPosition))
            {
                transform.position += moveVector;
            }
            else
            {
                transform.position = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }
}
