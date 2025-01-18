using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestTrigger : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  If the player gets close enough, this script triggers the animations/particles for the treasure chest to appear
    //
    //*---------------------------------------------*

    public GameObject TreasureChest;
    public ParticleSystem TreasureSandParticles;
    public ParticleSystem TreasureDustParticles;

    bool is_dug_up = false;
    Animator animator;
    Rigidbody rb;

    private void Awake()
    {
        animator = TreasureChest.GetComponent<Animator>();
        animator.enabled = false;

        rb = TreasureChest.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            if (!is_dug_up)
            {
                TreasureSandParticles.Play();
                TreasureDustParticles.Play();
                animator.enabled = true;

                is_dug_up = true;
                StartCoroutine(StopAnim());
            }
        }
    }

    IEnumerator StopAnim()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        TreasureSandParticles.Stop();
        TreasureDustParticles.Stop();
        animator.enabled = false;
        StopCoroutine(StopAnim());
    }
}
