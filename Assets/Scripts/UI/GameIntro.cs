using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIntro : MonoBehaviour
{
    public void StartAnim()
    {
        GetComponent<Animation>().Play();
        GetComponentInChildren<Button>().gameObject.SetActive(false);
    }
}
