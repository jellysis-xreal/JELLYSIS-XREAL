using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerPour : MonoBehaviour
{
    public GameObject streamPrefab = null;
    public Transform origin = null;
    public int handleRotation = 2;

    public bool isPouring = false;
    public Stream currentStream = null;

    private void Update()
    {
        bool pourCheck = handleRotation > 0 ;

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
        Debug.Log("ColorChanger Pour stream.End()!");
        currentStream.End();
        currentStream = null;
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}
