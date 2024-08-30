using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public GameObject Player;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = playerTransform.position + new Vector3(0f, 10f, 0f);
    }
}
