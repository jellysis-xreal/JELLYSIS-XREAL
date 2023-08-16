using System;
using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;

    public bool isPouring = false;
    public Stream currentStream = null;

    public delegate void StreamColorChanged(Color newColor);

    public event Action<Color> OnStreamColorChanged;

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }
   
    private void StartPour()
    {
        currentStream = CreateStream();
        //currentStream.SetLineColor(new Color(1.0f, 0.0f, 1.0f)); // Purple color
        currentStream.Begin();
    }
    private void EndPour()
    {
        currentStream.End();
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
        return streamObject.GetComponent<Stream>();
    }
    public void SetStreamColor(Color newColor)
    {
        if (currentStream != null)
        {
            currentStream.SetLineColor(newColor);
        }

        // 이벤트 호출
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