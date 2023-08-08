using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    private Vector3 _originScale;
    private Vector3 _targetScale;
    public Transform creamTransform;
    [SerializeField] private bool isScalingUp = false;
    public float whippingTimeMax = 10f;
    [SerializeField] private float leftTime ;
    public GameObject finishedWhippingGameObject;
    void Start()
    {
        _originScale = creamTransform.localScale;
        _targetScale = creamTransform.localScale * 3f;
        leftTime = whippingTimeMax;
    }

    private void Update()
    {
        // 스케일 증가 중이면 남은 시간 실시간 감소 ScaleUp()의 parameter로 남은 시간
        // 스케일 감소 중이면 남은 시간 실시간 증가 ScaleDown()의 parameter로 TimeMax - 남은시간
        CalculateLeftTime();
    }

    private void CalculateLeftTime()
    {
        if (isScalingUp && leftTime >= 0f)
        {
            leftTime -= Time.deltaTime;
            if(leftTime < 0) SpawnFinishedWhipping();
        }
        else if (!isScalingUp && leftTime <= whippingTimeMax)
        {
            leftTime += Time.deltaTime;
        }
    }
    
    [ContextMenu("Scale Up!")]
    public void ScaleUp()
    {
        Debug.Log("Scale Up!");
        isScalingUp = true;
        creamTransform.DOPause();
        creamTransform.DOScale(_targetScale,leftTime);
    }
    [ContextMenu("Scale Down!")]
    public void ScaleDown()
    {
        Debug.Log("Scale Down!");
        isScalingUp = false;
        // 완성까지 남은 시간 = 총 휘핑 시간 - 휘핑친 시간
        creamTransform.DOPause();
        creamTransform.DOScale(_originScale, whippingTimeMax-leftTime);
    }

    private void SpawnFinishedWhipping()
    {
        Instantiate(finishedWhippingGameObject,transform.position,Quaternion.identity);
        creamTransform.gameObject.SetActive(false);
    }
}
