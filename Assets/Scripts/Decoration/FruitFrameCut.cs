using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFrameCut : MonoBehaviour
{
    public GameObject circle, heart, star;
    //public GlobalObjects circle_obejct, heart_object, star_object;
    private InfoSender _infoSender;
    void Start()
    {
        //_infoSender = GetComponent<InfoSender>();
        circle.SetActive(true);
        star.SetActive(false);
        heart.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Debug.Log(circle_obejct.GUID + circle_obejct.Name);

        if (collision.gameObject.name == "cut_star")
        {
            //_infoSender.ChangeShape(star_object);
            circle.SetActive(false);
            heart.SetActive(false);
            star.SetActive(true);
            //Debug.Log("cut_star");
        }

        if (collision.gameObject.name == "cut_heart")
        {
            //_infoSender.ChangeShape(heart_object);
            circle.SetActive(false);
            heart.SetActive(true);
            star.SetActive(false);
            //Debug.Log("cut_heart");
        }

        if (collision.gameObject.name == "cut_circle")
        {
            //_infoSender.ChangeShape(circle_obejct);
            circle.SetActive(true);
            heart.SetActive(false);
            star.SetActive(false);
            //Debug.Log("cut_circle");
        }

        if (star.activeSelf == true)
        {
            //Debug.Log(star_object.GUID + star_object.Name);
        }

        if(heart.activeSelf == true)
        {
            //Debug.Log(heart_object.GUID + heart_object.Name);
        }

        if (circle.activeSelf == true)
        {
            //Debug.Log(circle_obejct.GUID + circle_obejct.Name);
        }
    }
}
