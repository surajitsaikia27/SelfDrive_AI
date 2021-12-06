using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvArea : MonoBehaviour
{
    public AIAgent aIAgent;
    // Start is called before the first frame update
    public TextMeshPro RewardText;
    public TextMeshPro actionText;

    public TextMeshPro rotateText;

    public void ResetArea()
    {
        PlaceAgent();
    }

    private void PlaceAgent()
    {
        Rigidbody rigidbody = aIAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;

    }

    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            // Pick a random radius
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            // Pick a random angle
            angle = UnityEngine.Random.Range(minAngle, maxAngle);
        }

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        return center + Quaternion.Euler(0f, 45f, 0f) * Vector3.forward * radius;
    }
    private void Start()
    {
        ResetArea();
    }

    // Update is called once per frame
    private void Update()
    {
       RewardText.text =  aIAgent.ret_distance().ToString("0.00");
       actionText.text = aIAgent.action.ToString("0.00");
       rotateText.text = aIAgent.rotate.ToString("0.00");
    }

}
