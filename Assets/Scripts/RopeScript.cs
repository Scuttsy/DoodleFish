using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 mousePos;

    private GameObject LastRopePiece 
    {
        get
        {
            return ropePieces[ropePieces.Count - 1];
        }
    }

    public List<GameObject> ropePieces;
    public GameObject ropePiecePrefab;

    Transform ropeParent;
    Rigidbody ropeRB;

    public int MaxPieces = 10;

    private void Start()
    {
        ropeParent = transform.Find("Rope");
        ropeRB = ropeParent.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(ropePieces.Count >= MaxPieces)
        {
            if(GetComponent<HingeJoint2D>().connectedBody != LastRopePiece.GetComponent<Rigidbody2D>())
            {
                GetComponent<HingeJoint2D>().connectedBody = LastRopePiece.GetComponent<Rigidbody2D>();
            }
        } else
        {
            if (LastRopePiece.transform.position.y <= transform.position.y - 0.5f && ropePieces.Count < MaxPieces)
            {
                GameObject newRopePiece = Instantiate(ropePiecePrefab, LastRopePiece.transform.position + Vector3.up * .4f, Quaternion.identity);
                newRopePiece.GetComponent<HingeJoint2D>().connectedBody = LastRopePiece.GetComponent<Rigidbody2D>();
                ropePieces.Add(newRopePiece);
                newRopePiece.transform.SetParent(ropeParent);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,10)));
    }
}
