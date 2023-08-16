using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    public Transform XROriginTransform;

    private void Start()
    {
        // ReplacePlayer1();
    }

    [ContextMenu("SetPlayer")]
    public void InitPlayer()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < 4; i++)
        {
            player[i].transform.position = transforms[i].position;
            player[i].transform.rotation = transforms[i].rotation;
        }
    }

    public void ReplacePlayer1()
    {
        XROriginTransform.position = transforms[0].position;
        XROriginTransform.rotation = transforms[0].rotation;
    }
    [ContextMenu("Replace1")]
    public void ReplacePlayer2()
    {
        XROriginTransform.position = transforms[1].position;
        XROriginTransform.rotation = transforms[1].rotation;
    }
    [ContextMenu("Replace2")]
    public void ReplacePlayer3()
    {
        XROriginTransform.position = transforms[2].position;
        XROriginTransform.rotation = transforms[2].rotation;
    }
    [ContextMenu("Replace3")]
    public void ReplacePlayer4()
    {
        XROriginTransform.position = transforms[3].position;
        XROriginTransform.rotation = transforms[3].rotation;
    }
}
