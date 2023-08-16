using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] private Transform tableTransform;
    [SerializeField] private Transform niddleTransform;
    [SerializeField] private Transform[] bearTransforms;
    public int jumpBearIndex = 0;
    
    private TableEventManager _tableEventManager;
    void Start()
    {
        _tableEventManager = GetComponent<TableEventManager>();
        _tableEventManager.tableRotationEvent += TableRotate90;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _tableEventManager.RaiseEvent();
        }    
    }
    [ContextMenu("Rotate")]
    public void TableRotate90(object sender, EventArgs e)
    {
        tableTransform.DORotate(tableTransform.rotation.eulerAngles + Quaternion.AngleAxis(90f, Vector3.up).eulerAngles,
            5f, RotateMode.Fast);
        niddleTransform.DORotate(
            niddleTransform.rotation.eulerAngles + Quaternion.AngleAxis(-10f, Vector3.right).eulerAngles, 2.5f,
            RotateMode.Fast).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
        bearTransforms[jumpBearIndex].DOLocalJump(bearTransforms[jumpBearIndex].localPosition, 0.05f, 1,2.5f).SetDelay(1);
        jumpBearIndex += 1;
    }
}
