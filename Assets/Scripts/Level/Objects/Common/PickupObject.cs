using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  The base class for all pick-up-able objects in the game
    //
    //*---------------------------------------------*

    public float PickUpDistance;
    public float Weight;
    public Vector3 FinalPos;
    public Quaternion FinalRot;
    
    bool isPickedUp = false;

    bool setForce = false;

    Player player; // The object that we'll try to follow if we've been picked up

    MeshRenderer mesh;
    Rigidbody rb;
    Collider col;
    Animator anim;

    private void Awake()
    {
        player = FindObjectOfType<Player>();

        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>() == null ? null : GetComponent<Animator>();
    }

    void Start()
    {
        rb.isKinematic = true;
    }

    void Update()
    {
        if (isPickedUp)
        {
            if (player.pickUpInput && player.PickUpAnchor.childCount != 0)
            {
                setForce = true;
            }
        }
        else
        {
            // Check for if the player has gotten close enough to pick up
            if (player.pickUpInput && player.PickUpAnchor.childCount == 0)
            {
                if (Vector3.Distance(rb.position, player.transform.position) <= PickUpDistance && col.enabled)
                {
                    isPickedUp = true;
                    col.enabled = false;
                    rb.isKinematic = true;

                    // Trying to move the object so it doesn't cross into the player's collider
                    transform.position = player.PickUpAnchor.position;
                    transform.parent = player.PickUpAnchor;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (setForce)
        {
            isPickedUp = false;
            col.enabled = true;

            player.PickUpAnchor.DetachChildren();

            rb.isKinematic = false;
            rb.AddForce(new Vector3(
                    player.velocity.x * 2, player.velocity.y > 0 ? player.velocity.y : 0, player.velocity.z * 2),
                    ForceMode.VelocityChange);

            setForce = false;
        }
    }

    public void Deactivate()
    {
        col.enabled = false;
        rb.isKinematic = true;

        transform.position = FinalPos;
        transform.rotation = FinalRot;

        if (anim != null && gameObject.tag == "TreasureChest")
        {
            AnimatorOff();
            anim.SetTrigger("Deactivate");
        }
    }

    public void AnimatorOn()
    {
        anim.enabled = true;
    }

    public void AnimatorOff()
    {
        anim.enabled = false;
    }
}
