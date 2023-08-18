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

    
    public bool isPouring = false;
    private LiquidCollider _liquidCollider;    
    public delegate void StreamColorChanged(Color newColor);

    public event Action<Color> OnStreamColorChanged;
    public MeshRenderer meshRenderer;
    public TriggerArea triggerArea;
    public Color color;

    public BearColorType bearColorType;
    private GameObject streamObject;
    private Coroutine minusHeightRoutine;
    
    
    [Space]
    [SerializeField]private bool pourCheck;
    [SerializeField]private bool isEmpty = true;
    [SerializeField]private float value;
    public Stream currentStream = null;
    private void Start()
    {
        isEmpty = true;
        value = 0.58f;
        meshRenderer.material.SetFloat("_FillAmount", value);
    }

    private void Update()
    {
        pourCheck = CalculatePourAngle() < pourThreshold;

        if (pourCheck && !isEmpty && currentStream == null)
        {
            StartPour();
        }
        else if(!pourCheck && !isEmpty)
        {
            EndPour();
        }
    }
    
    // 0.42 최대 0.58 최소
    // 붓기 시작했을 때 부터 5초동안 액체 나옴.
    // 5초동안 Liquid Shader의 Fill Amount.SetFloat을 실시간으로 해줘야함.
    public IEnumerator PlusLiquidHeight()
    {
        
        isEmpty = false;
        // 5초동안 0.42에서 0.58로 서서히 증가해야 함.    Time.deltaTime (0.58-0.42)/5
        while (true)
        {
            value -= Time.deltaTime * (0.58f - 0.42f) / 5f;
            if(value <= 0.42) break;
            meshRenderer.material.SetFloat("_FillAmount", value);
            yield return null;
            //Debug.Log("plus Value : "+value);
        }
        StopCoroutine(PlusLiquidHeight());
    }

    IEnumerator MinusLiquidHeight()
    {
        // 5초동안 0.42에서 0.58로 서서히 증가해야 함.    Time.deltaTime (0.58-0.42)/5
        while (true)
        {
            //Debug.Log("minus Height : "+value);
            value += Time.deltaTime * (0.58f - 0.42f) / 5f;
            meshRenderer.material.SetFloat("_FillAmount", value);
            if (value >= 0.58)
            {
                isEmpty = true;
                currentStream.End();
                Destroy(streamObject);
                //currentStream = null;
                break;
            }
            yield return null;
        }
        StopCoroutine(minusHeightRoutine);
    }
    
    
    private void StartPour()
    {
        isPouring = true;
        Debug.Log("Start Pour!");
        currentStream = CreateStream();
        Debug.Log(currentStream);
        minusHeightRoutine = StartCoroutine(MinusLiquidHeight());
        currentStream.Begin();
    }
    // 끝났을 때 endpour 호출되지만 기울기때메 다시 부어짐.
    private void EndPour()
    {
        isPouring = false;
        // 붓는 거 멈추면 MinusLiquid 멈추고, stream 오브젝트 삭제
        if (currentStream != null)
        {
            Debug.Log("End Pour! Stop Minus Height Coroutine");
            StopCoroutine(minusHeightRoutine);
            Debug.Log( "height value is "+value);
            currentStream.End();
            Destroy(streamObject);
            //currentStream = null;

        }
    }

    private float CalculatePourAngle()
    {
        float zAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.z);
        float xAngle = 180 - Mathf.Abs(180 - transform.rotation.eulerAngles.x);
        return 90 - Mathf.Max(zAngle, xAngle);
    }

    private Stream CreateStream()
    {
        streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        Stream stream = streamObject.GetComponent<Stream>();
        Debug.Log("Color is "+triggerArea.color);
        stream.SetLineColor(triggerArea.color);
        color = triggerArea.color;
        return stream;
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