using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] private Transform tableTransform;
    private TableEventManager _tableEventManager;
    void Start()
    {
        _tableEventManager = GetComponent<TableEventManager>();
        _tableEventManager.tableRotationEvent += TableRotate90;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _tableEventManager.RaiseEvent();
        }    
    }
    public void TableRotate90(object sender, EventArgs e)
    {
        tableTransform.DORotate(tableTransform.rotation.eulerAngles + Quaternion.AngleAxis(90f, Vector3.up).eulerAngles,
            5f, RotateMode.Fast);
    }
}
