using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  A moving platform that moves back-and-forth between two points
    //
    //*---------------------------------------------*

    public float MoveSpeed; // How quickly the platform moves between its two target points

    public Vector3 currentSpeed;
    bool isMoving = true;

    List<MovingPlatformNode> targetPoints = new List<MovingPlatformNode>();
    int currentTargetPoint;

    void Start()
    {        
        for (int i = 0; i <= transform.childCount - 1; i++)
        {
            targetPoints.Add(transform.GetChild(i).GetComponent<MovingPlatformNode>());
        }
        transform.DetachChildren();
        currentTargetPoint = 1;
    }

    void Update()
    {
        MovePlatform();
    }

    // Move the platform towards the next target object node
    private void MovePlatform()
    {
        if (isMoving)
        {
            currentSpeed = Vector3.MoveTowards(transform.position, targetPoints[currentTargetPoint].transform.position, MoveSpeed * Time.deltaTime) - transform.position;
            transform.Translate(currentSpeed, Space.World);

            if (transform.position == targetPoints[currentTargetPoint].transform.position)
            {
                StartCoroutine(Pause());
            }
        }
    }

    // Temporarily stop the platform from moving depending on a set timer
    private IEnumerator Pause()
    {
        isMoving = false;
        currentSpeed = Vector3.zero;

        yield return new WaitForSeconds(targetPoints[currentTargetPoint].PauseTime);

        if (currentTargetPoint >= targetPoints.Count - 1) { currentTargetPoint = 0; }
        else { currentTargetPoint++; }
        isMoving = true;

        StopCoroutine(Pause());
    }

    // Move the current object that is standing on the platform (that is NOT the player object)
    public void MoveObject(GameObject obj)
    {
        obj.transform.position += currentSpeed;
    }
}
