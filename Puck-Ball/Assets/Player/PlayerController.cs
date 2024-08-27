using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration = 0.1f;

    private Rigidbody rg;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction, verticalDirection, horizontalDirection;
        verticalDirection = Vector3.forward * Input.GetAxis("Vertical");
        horizontalDirection = Vector3.right * Input.GetAxis("Horizontal");
        direction = verticalDirection + horizontalDirection;

        rg.AddForce(direction * acceleration, ForceMode.Force);
    }
}
