using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float EatValue;

    public float Speed;

    private void Update()
    {
        Vector3 v = ( Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - transform.position).normalized;

        transform.position += v * Speed * Time.deltaTime;
        
    }

    public void Eat()
    {
        Destroy(gameObject);
    }

}
