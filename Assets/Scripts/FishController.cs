using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public enum FishState
    {
        Roaming,
        Chasing,
        Mouse
    }

    public float EatValue;
    public float Speed;
    [SerializeField] private  int AngerLimit;
    public int AngerValue; // Thew angrier the fish the larger the aggro range


    public FishState State = FishState.Roaming;

    private void Update()
    {
        switch (State)
        {
            case FishState.Roaming:
                Roam();
                break;
            case FishState.Chasing:
                Chase();
                break;
            default:
                Vector3 v = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - transform.position).normalized;
                transform.position += v * Speed * Time.deltaTime;
                break; 
        }

        if (transform.position.x < -25 || transform.position.x > 15)
            Destroy(gameObject);

        

    }

    private void Chase()
    {
        //Put Fish aggro in here
    }

    private void Roam()
    {
        transform.position += transform.right * Speed * Time.deltaTime;
    }

    public void Eat()
    {
        Destroy(gameObject);
    }

}
