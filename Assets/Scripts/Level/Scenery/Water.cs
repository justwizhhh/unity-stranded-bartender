using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  Water surface bobbing for when the player enters the water
    //
    //*---------------------------------------------*

    public float WaterForceMin;
    public float WaterForceMax;
    public float BounceRange;

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.currentGravity -= Random.Range(WaterForceMin, WaterForceMax);
            player.currentGravity = Mathf.Clamp(player.currentGravity, -BounceRange, BounceRange);
        }
        else if (other.attachedRigidbody != null)
        {
            if (other.GetComponent<Rigidbody>().isKinematic == false)
            {
                Rigidbody rb = other.attachedRigidbody;
                float force = Random.Range(WaterForceMin, WaterForceMax);
                rb.velocity += new Vector3(-rb.velocity.x / force, force, -rb.velocity.z / force);
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -BounceRange, BounceRange), rb.velocity.z);
            }
        }
    }
}
