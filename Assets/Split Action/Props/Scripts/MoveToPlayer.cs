using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveToPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float minMovingSpeed = 1.0f;
    public float maxMovingSpeed = 3.0f;
    public float speed;

    private void Start()
    {
        speed = Random.Range(minMovingSpeed, maxMovingSpeed);
        playerTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.LookAt(transform);
        Vector3 dir = playerTransform.position - transform.position;
        transform.position += dir * speed * Time.deltaTime;
    }
}
