using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut_Banana : MonoBehaviour
{
    public GameObject origin_banana;
    public GameObject banana;
    public AudioSource cut_banana;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            cut_banana.Play();
            banana.transform.SetParent(null);
            origin_banana.gameObject.SetActive(false);
            //this.gameObject.SetActive(false);
            banana.gameObject.SetActive(true);
        }
    }
}
