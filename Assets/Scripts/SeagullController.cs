using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullController : Entity
{
    [Header("Movement")]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private Transform _anchorPoint;

    //public
    public bool IsRoaming = true;
    public int HitPoints = 10;
    public int NumFishes = 0;
    //internal


    void Update()
    {
        if (IsRoaming)
        {
            transform.position += transform.right * _speed * Time.deltaTime;
        }
        else
        {
            transform.position = _anchorPoint.position;
        }
        if (transform.position.x < -25 || transform.position.x > 15)
            Destroy(gameObject);


    }

    public void CatchSeagull(Transform AnchorPoint)
    {
        IsRoaming = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _anchorPoint = AnchorPoint;
    }
}
