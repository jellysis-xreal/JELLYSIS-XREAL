using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddingSpawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public float resetDelay = 10.0f;
    public bool puddingReset = false;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (puddingReset)
        {
            ResetObject();

            
        }
    }

    private void ResetObject()
    {
        

        // 원래 위치와 회전으로 물체를 되돌림
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        puddingReset = false;
    }

}
