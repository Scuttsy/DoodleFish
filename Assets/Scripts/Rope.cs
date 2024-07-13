using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rope : MonoBehaviour
{
    public Rigidbody2D Hook;
    public GameObject RopePrefab;
    public GameObject KnotPrefab;
    
    public List<GameObject> Ropes = new List<GameObject>();

    public int NumOfLinks = 10;

    public HingeJoint2D Top;

    private float _nextInput = 0;

    private void Start()
    {
        GenerateRope();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && Time.time > _nextInput)
        {
            _nextInput = Time.time + 0.5f;
            AddLink();
        }
        if (Input.GetKey(KeyCode.UpArrow) && Ropes.Count > 1 && Time.time > _nextInput)
        {
            _nextInput = Time.time + 0.5f;
            RemoveLink();
        }
    }

    private void AddLink()
    {
        GameObject newPiece = Instantiate(RopePrefab);
        newPiece.transform.parent = transform;
        newPiece.transform.position = transform.position;
        Ropes.Add(newPiece);

        HingeJoint2D hj = newPiece.GetComponent<HingeJoint2D>();
        hj.connectedBody = Hook;

        Top.connectedBody = newPiece.GetComponent<Rigidbody2D>();

        Top = hj;

    }
    private void RemoveLink()
    {
        HingeJoint2D newTop = Ropes[Ropes.Count - 2].GetComponent<HingeJoint2D>();
        newTop.connectedBody = Hook;
        newTop.gameObject.transform.position = Hook.gameObject.transform.position;
        Top = newTop;

        GameObject removeRope = Ropes[Ropes.Count - 1];
        Ropes.Remove(removeRope);
        Destroy(removeRope);


    }

    private void GenerateRope()
    {
        Rigidbody2D prevBod = Hook;
        for (int i = 0; i < NumOfLinks; i++)
        {
            GameObject newPiece = Instantiate(RopePrefab);
            newPiece.transform.parent = transform;
            newPiece.transform.position = transform.position;
            newPiece.transform.GetChild(0).position += Vector3.forward * i * 0.01f;
            Ropes.Add(newPiece);
            HingeJoint2D hj = newPiece.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;
            if (Hook == prevBod)
                Top = hj;

            prevBod = newPiece.GetComponent<Rigidbody2D>();
        }
        GameObject knot = Instantiate(KnotPrefab);
        knot.transform.parent = transform;
        knot.transform.position = transform.position;
        Ropes.Add(knot);
        knot.GetComponent<HingeJoint2D>().connectedBody = prevBod;

        Ropes.Reverse();
    }
}
