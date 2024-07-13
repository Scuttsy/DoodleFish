using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rope : MonoBehaviour
{
    public Rigidbody2D Hook;
    public GameObject RopePrefab;
    
    public List<GameObject> Ropes = new List<GameObject>();

    public int NumOfLinks = 10;

    private HingeJoint2D _top;

    private void Start()
    {
        GenerateRope();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddLink();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
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

        _top.connectedBody = newPiece.GetComponent<Rigidbody2D>();

    }
    private void RemoveLink()
    {
        HingeJoint2D newTop = Ropes[Ropes.Count - 2].GetComponent<HingeJoint2D>();
        newTop.connectedBody = Hook;
        newTop.gameObject.transform.position = Hook.gameObject.transform.position;
        Destroy(Ropes[Ropes.Count - 1]);
        Ropes.RemoveAt(Ropes.Count - 1);
    }

    private void GenerateRope()
    {
        Rigidbody2D prevBod = Hook;
        for (int i = 0; i < NumOfLinks; i++)
        {
            GameObject newPiece = Instantiate(RopePrefab);
            newPiece.transform.parent = transform;
            newPiece.transform.position = transform.position + i * Vector3.down;
            Ropes.Add(newPiece);
            HingeJoint2D hj = newPiece.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;
            if (Hook == prevBod)
                _top = hj;

            prevBod = newPiece.GetComponent<Rigidbody2D>();
        }
    }
}
