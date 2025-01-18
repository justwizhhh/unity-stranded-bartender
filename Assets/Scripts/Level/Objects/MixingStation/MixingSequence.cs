using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MixingSequence : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  The UI where the player's mouse inputs will be used to trigger cutscenes
    //
    //*---------------------------------------------*

    [Header("Interactive Toggles")]
    public GameObject Player;
    public Vector3 PlayerFinalPos;
    public Quaternion PlayerFinalRot;

    [Space(10)]
    public Animator Fruit1;
    public Animator Fruit2;
    public ParticleSystem Fruit1Particles;
    public ParticleSystem Fruit2Particles;
    public float SqueezeSpeed;

    [Space(10)]
    public GameObject TreasureChest;

    [Space(10)]
    public GameObject RumBottle;
    public ParticleSystem RumParticles;
    public float PourSpeed;
    public float PourAngle;
    public float TiltSpeed;


    [Space(10)]
    public PlayableDirector[] Cutscenes;

    enum MixStates
    {
        SqueezeFruit1,
        SqueezeFruit2,
        OpenChest,
        PourRum,
        Win
    };
    MixStates currentMixState = MixStates.SqueezeFruit1;

    [Space(10)]
    public GameObject[] StateInterfaces;

    // Fruit squeezing progress
    bool fruit1_isSqueezing = false;
    bool fruit2_isSqueezing = false;

    float fruit1Progress;
    float fruit2Progress;
    float pourProgress;

    // Chest opening progress
    Vector2 mousePosStart;
    Vector2 mousePosEnd;

    CameraController camCon;

    private void Awake()
    {
        camCon = FindObjectOfType<CameraController>();
    }

    public void UITurnOn()
    {
        gameObject.SetActive(true);
    }

    public void UITurnOff()
    {
        gameObject.SetActive(false);
    }

    public void ChangeUI(int id)
    {
        for (int i = 0; i < StateInterfaces.Length; i++)
        {
            if (id == i)
            {
                StateInterfaces[i].SetActive(true);
            }
            else
            {
                StateInterfaces[i].SetActive(false);
            }
        }
    }

    public void SetPlayerPos()
    {
        Player.transform.position = PlayerFinalPos;
        Player.transform.GetChild(0).rotation = PlayerFinalRot;
    }

    void FixedUpdate()
    {
        switch (currentMixState)
        {
            default:
            case MixStates.SqueezeFruit1:
                if (fruit1_isSqueezing)
                {
                    fruit1Progress += SqueezeSpeed;
                }

                if (fruit1Progress > 1)
                {
                    Fruit1.SetTrigger("Unsqueeze");
                    currentMixState = MixStates.SqueezeFruit2;
                    ChangeUI(2);
                    Cutscenes[0].Play();
                    Fruit1Particles.Stop();
                }
                break;

            case MixStates.SqueezeFruit2:
                if (fruit2_isSqueezing)
                {
                    fruit2Progress += SqueezeSpeed;
                }

                if (fruit2Progress > 1)
                {
                    Fruit2.SetTrigger("Unsqueeze");
                    TreasureChest.GetComponent<Animator>().enabled = true;

                    currentMixState = MixStates.OpenChest;
                    ChangeUI(3);
                    Cutscenes[1].Play();
                    Fruit2Particles.Stop();
                }
                break;

            case MixStates.OpenChest:
                break;

            case MixStates.PourRum:
                float currentAngle = RumBottle.transform.eulerAngles.z;
                if (currentAngle >= PourAngle && currentAngle <= 180f)
                {
                    RumParticles.Play();
                    pourProgress += PourSpeed;
                }
                else
                {
                    RumParticles.Stop();
                }

                if (pourProgress > 1)
                {
                    currentMixState = MixStates.Win;
                    ChangeUI(5);
                    camCon.ChangeAnchor(3);
                    camCon.ChangeSpeed(0);
                    Cutscenes[3].Play();
                }

                break;

            case MixStates.Win:
                break;
        }
    }

    public void Squeeze1True()
    {
        fruit1_isSqueezing = true;
        Fruit1.SetTrigger("Squeeze");
        Fruit1Particles.Play();
    }

    public void Squeeze1False()
    {
        fruit1_isSqueezing = false;
        Fruit1.SetTrigger("Unsqueeze");
        Fruit1Particles.Stop();
    }

    public void Squeeze2True()
    {
        fruit2_isSqueezing = true;
        Fruit2.SetTrigger("Squeeze");
        Fruit2Particles.Play();
    }

    public void Squeeze2False()
    {
        fruit2_isSqueezing = false;
        Fruit2.SetTrigger("Unsqueeze");
        Fruit2Particles.Stop();
    }

    public void OpenChestStart()
    {
        mousePosStart = Mouse.current.position.ReadValue();
    }

    public void OpenChestEnd()
    {
        mousePosEnd = Mouse.current.position.ReadValue();
        if (mousePosStart != Vector2.zero && mousePosEnd != Vector2.zero)
        {
            if (mousePosEnd.y > mousePosStart.y)
            {
                TreasureChest.GetComponent<Animator>().enabled = true;

                currentMixState = MixStates.PourRum;
                ChangeUI(4);
                Cutscenes[2].Play();
            }
        }
    }

    public void TiltRumStart()
    {
        mousePosStart = Mouse.current.position.ReadValue();
    }

    public void TiltRum()
    {
        Vector2 mouse = Mouse.current.position.ReadValue();
        RumBottle.transform.eulerAngles = new Vector3(
            RumBottle.transform.eulerAngles.x,
            RumBottle.transform.eulerAngles.y,
            Mathf.Clamp((mouse.x - mousePosStart.x) * TiltSpeed, 0, PourAngle));
    }
}
