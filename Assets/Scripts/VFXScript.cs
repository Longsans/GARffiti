using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    public GameObject ARCam;

    void Update()
    {
        transform.position = ARCam.transform.position + Vector3.up + 5 * ARCam.transform.forward;
    }
}
