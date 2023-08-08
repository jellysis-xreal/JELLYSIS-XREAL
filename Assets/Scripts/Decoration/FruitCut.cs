using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCut : MonoBehaviour
{
    public GameObject onion1, onion2;
    public AudioSource cut_sound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "knife_collider")
        {
            if (this.gameObject.activeSelf == true)
            {
                cut_sound.Play();

                this.gameObject.SetActive(false);

                onion1.gameObject.SetActive(true);
                onion2.gameObject.SetActive(true);

                onion1.transform.parent = null;
                onion2.transform.parent = null;
            }

        }
    }
/*    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "knife_collider")
        {
            if (this.gameObject.activeSelf == true)
            {
                this.gameObject.SetActive(false);

                onion1.gameObject.SetActive(true);
                onion2.gameObject.SetActive(true);

                onion1.transform.parent = null;
                onion2.transform.parent = null;
            }
        }
    }*/
}
