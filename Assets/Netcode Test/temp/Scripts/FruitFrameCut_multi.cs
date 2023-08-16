using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FruitFrameCut_multi : NetworkBehaviour
{
    public void printInfo()
    {
        Debug.Log("[TEST] framecut IsClient, IsServer, IsOwner: " + IsClient + ", " + IsServer + ", " + IsOwner);
    }

    public NetworkSyncObject circle, heart, star;
    public NetworkObject circle_NO, heart_NO, star_NO;

    private bool isCut = false; // sync 안 함.


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20 && !isCut)
        {
            //isCut = true; // knife 한 번만 작동
            if ((IsClient && !IsOwner))
            {
                circle.RequestOwnership(circle_NO);
                heart.RequestOwnership(heart_NO);
                star.RequestOwnership(star_NO);
            }
            circle.SetActiveNetworkObject(true);
            heart.SetActiveNetworkObject(false);
            star.SetActiveNetworkObject(false);
            if ((IsClient && IsOwner) && !IsServer)
            {
                circle.RequestRemoveOwnership(circle_NO);
                heart.RequestRemoveOwnership(heart_NO);
                star.RequestRemoveOwnership(star_NO);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "cut_star")
        {
            Debug.Log("[TEST] detected star");
            if ((IsClient && !IsOwner))
            {
                circle.RequestOwnership(circle_NO);
                heart.RequestOwnership(heart_NO);
                star.RequestOwnership(star_NO);
            }
            circle.SetActiveNetworkObject(false);
            heart.SetActiveNetworkObject(false);
            star.SetActiveNetworkObject(true);
            if ((IsClient && IsOwner) && !IsServer)
            {
                circle.RequestRemoveOwnership(circle_NO);
                heart.RequestRemoveOwnership(heart_NO);
                star.RequestRemoveOwnership(star_NO);
            }
        }

        else if (collision.gameObject.name == "cut_heart")
        {
            Debug.Log("[TEST] detected heart");
            if ((IsClient && !IsOwner))
            {
                circle.RequestOwnership(circle_NO);
                heart.RequestOwnership(heart_NO);
                star.RequestOwnership(star_NO);
            }
            circle.SetActiveNetworkObject(false);
            heart.SetActiveNetworkObject(true);
            star.SetActiveNetworkObject(false);
            if ((IsClient && IsOwner) && !IsServer)
            {
                circle.RequestRemoveOwnership(circle_NO);
                heart.RequestRemoveOwnership(heart_NO);
                star.RequestRemoveOwnership(star_NO);
            }
        }

        else if (collision.gameObject.name == "cut_circle")
        {
            Debug.Log("[TEST] detected circle");
            if ((IsClient && !IsOwner))
            {
                circle.RequestRemoveOwnership(circle_NO);
                heart.RequestRemoveOwnership(heart_NO);
                star.RequestRemoveOwnership(star_NO);
            }
            circle.SetActiveNetworkObject(true);
            heart.SetActiveNetworkObject(false);
            star.SetActiveNetworkObject(false);
            if ((IsClient && IsOwner) && !IsServer)
            {
                circle.RequestRemoveOwnership(circle_NO);
                heart.RequestRemoveOwnership(heart_NO);
                star.RequestRemoveOwnership(star_NO);
            }
        }
    }
}
