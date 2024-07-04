using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x < -25 || transform.position.x > 15)
            Destroy(gameObject);
    }
}
