using System;
using System.Collections;
using ColorChanger;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;

    public bool isEmpty = false;
    public bool isPouring = false;
    public Stream currentStream = null;
    private LiquidCollider _liquidCollider;    
    public delegate void StreamColorChanged(Color newColor);

    public event Action<Color> OnStreamColorChanged;
    public MeshRenderer meshRenderer;
    public TriggerArea triggerArea;
    public Color color;

    public BearColorType bearColorType;
    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring && !isEmpty)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }
    
    // 0.42 최대 0.58 최소
    // 붓기 시작했을 때 부터 5초동안 액체 나옴.
    // 5초동안 Liquid Shader의 Fill Amount.SetFloat을 실시간으로 해줘야함.
    public IEnumerator PlusLiquidHeight()
    {
        isEmpty = false;
        float value = 0.58f;
        // 5초동안 0.42에서 0.58로 서서히 증가해야 함.    Time.deltaTime (0.58-0.42)/5
        while (true)
        {
            value -= Time.deltaTime * (0.58f - 0.42f) / 5f;
            if(value <= 0.42) break;
            meshRenderer.material.SetFloat("_FillAmount", value);
            yield return null;
            Debug.Log("plus, Value : "+value);
        }
        StopCoroutine(PlusLiquidHeight());
    }

    void StopPlusLiquidHeightRoutine()
    {
        
    }
    IEnumerator MinusLiquidHeight()
    {
        float value = 0.42f;
        // 5초동안 0.42에서 0.58로 서서히 증가해야 함.    Time.deltaTime (0.58-0.42)/5
        while (true)
        {
            value += Time.deltaTime * (0.58f - 0.42f) / 5f;
            meshRenderer.material.SetFloat("_FillAmount", value);
            if(value >= 0.58) EndPour();
            yield return null;
        }
    }
    
    
    private void StartPour()
    {
        
        currentStream = CreateStream();
        StartCoroutine(MinusLiquidHeight());
        //currentStream.SetLineColor(new Color(1.0f, 0.0f, 1.0f)); // Purple color
        currentStream.Begin();
    }
    // 끝났을 때 endpour 호출되지만 기울기때메 다시 부어짐.
    private void EndPour()
    {
        isEmpty = true;
        currentStream.End();
        StopCoroutine(MinusLiquidHeight());
        currentStream = null;
    }

    private float CalculatePourAngle()
    {
        float zAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.z);
        float xAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.x);
        return 90 - Mathf.Max(zAngle, xAngle);
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        Stream stream = streamObject.GetComponent<Stream>();
        Debug.Log("Color is "+triggerArea.color);
        stream.SetLineColor(triggerArea.color);
        color = triggerArea.color;
        return streamObject.GetComponent<Stream>();
    }
    public void SetStreamColor(Color newColor)
    {
        if (currentStream != null)
        {
            currentStream.SetLineColor(newColor);
        }

        // �̺�Ʈ ȣ��
        OnStreamColorChanged?.Invoke(newColor);
    }
    private void HandleStreamColorChanged(Color newColor)
    {
        if (currentStream != null)
        {
            currentStream.SetLineColor(newColor);
        }
    }

    public void ChangeStreamColor(Color newColor)
    {
        if (currentStream != null)
        {
            currentStream.SetLineColor(newColor);
        }
    }
}