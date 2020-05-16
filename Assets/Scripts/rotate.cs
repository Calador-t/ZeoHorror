using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private float theta;
    // Start is called before the first frame update
    void Start()
    {
        theta = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, theta, 0);
    }
}
