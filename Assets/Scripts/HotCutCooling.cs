using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotCutCooling : MonoBehaviour
{
    public Material cutMat;

    void Start()
    {
        cutMat = GetComponent<MeshRenderer>().materials[^1];
        cutMat.SetFloat("_StartTime", Time.time);
    }
}
