using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  A template class for all of the different states the player can be in
    //
    //*---------------------------------------------*       

    protected PlayerController controller;
    protected Player player;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        player = GetComponent<Player>();
    }

    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
