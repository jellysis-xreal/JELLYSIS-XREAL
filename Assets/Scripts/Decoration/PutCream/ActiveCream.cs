using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCream : MonoBehaviour
{
    public CreamMaker creamMaker;
    
    public void ActiveTrueCreamMaker()
    {
        creamMaker.enabled = true;
    }
    public void ActiveFalseCreamMaker()
    {
        creamMaker.enabled = false;
    }
}
