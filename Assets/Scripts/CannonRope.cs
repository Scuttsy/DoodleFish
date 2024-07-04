using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRope : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("cannonRope collision");
        if (other.CompareTag("Seagull"))
        {
            Debug.Log("rope reached seagull");
            other.GetComponent<SeagullController>().CatchSeagull(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
