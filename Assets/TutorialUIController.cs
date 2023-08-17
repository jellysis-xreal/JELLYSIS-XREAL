using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    public GameObject[] tutorialImage;
    public GameObject buttonNext;
    public GameObject buttonPrev;
    
    public int uiIndex;

    private void Start()
    {
        uiIndex = 0;
    }

    public void ShowNextUI()
    {
        for (int i = 0; i < tutorialImage.Length; i++)
        {
            tutorialImage[i].gameObject.SetActive(false);
        }
        tutorialImage[uiIndex+1].gameObject.SetActive(true);
        uiIndex += 1;
        if (uiIndex < tutorialImage.Length -1)
        {
            buttonPrev.SetActive(true);
        }
        if (uiIndex == tutorialImage.Length - 1)
        {
            buttonPrev.SetActive(true);
            buttonNext.SetActive(false);
        }
    }

    public void ShowPrevUI()    
    {
        for (int i = 0; i < tutorialImage.Length; i++)
        {
            tutorialImage[i].gameObject.SetActive(false);
        }
        tutorialImage[uiIndex-1].gameObject.SetActive(true);
        uiIndex -= 1;
        if (uiIndex > 0)
        {
            buttonNext.SetActive(true);
        }
        if (uiIndex == 0)
        {
            buttonPrev.SetActive(false);
            buttonNext.SetActive(true);
        }

    }
    
}
