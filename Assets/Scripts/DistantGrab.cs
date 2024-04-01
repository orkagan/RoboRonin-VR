using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.UI;

public class DistantGrab : MonoBehaviour
{
    UxrLaserPointer _laserPointer;

    void Start()
    {
        _laserPointer = GetComponent<UxrLaserPointer>();
    }

    void Update()
    {
        
    }
}
