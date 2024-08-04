using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{

    [Range(.1f, 10f)]
    public float _wiggleRoom;
    
    [Range(.1f, 10f)]
    public float _wiggleSpeed;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleRoom);
    }
}
