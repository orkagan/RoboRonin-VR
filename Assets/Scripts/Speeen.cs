using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speeen : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(new Vector3(0f,10f,0f));
    }
}
