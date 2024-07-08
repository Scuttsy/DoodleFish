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
            if(ropePieces.Count > 0)
                return ropePieces[ropePieces.Count - 1];
            else return null;
        }
    }

    public int strength = 4;

    public List<GameObject> ropePieces = new List<GameObject>();
    public GameObject ropePiecePrefab;

    Transform ropeParent;
    Rigidbody ropeRB;

    public int MaxPieces = 10;

    private void Start()
    {
        ropeParent = transform.Find("Rope");
        ropeRB = ropeParent.GetComponent<Rigidbody>();
        //GetComponent<HingeJoint2D>().connectedBody = LastRopePiece.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(ropePieces.Count > 0)
        {
            if (ropePieces.Count >= MaxPieces)
            {
                if (GetComponent<HingeJoint2D>().connectedBody != LastRopePiece.GetComponent<Rigidbody2D>())
                {
                    GetComponent<HingeJoint2D>().connectedBody = LastRopePiece.GetComponent<Rigidbody2D>();
                }
            }
            else if (LastRopePiece)
            {
                Vector3 v = transform.position - LastRopePiece.transform.position;
                if (v.magnitude >= 1 && ropePieces.Count < MaxPieces)
                {

                    GameObject newRopePiece = Instantiate(ropePiecePrefab, LastRopePiece.transform.position +( v.normalized * .4f),
                        Quaternion.Euler(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90), ropeParent);
                   newRopePiece.GetComponent<HingeJoint2D>().connectedBody = LastRopePiece.GetComponent<Rigidbody2D>();
                    
                
                    ropePieces.Add(newRopePiece);
                }
            }
        } 
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 LaunchDirection = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                mousePos = Vector3.zero + LaunchDirection;
                LaunchDirection.Normalize();
                GameObject newRopePiece = Instantiate(ropePiecePrefab, transform.position,
                        Quaternion.Euler(0, 0, Mathf.Atan2(LaunchDirection.y, LaunchDirection.x) * Mathf.Rad2Deg - 90), ropeParent);

                Destroy(newRopePiece.GetComponent<HingeJoint2D>());
                newRopePiece.GetComponent<Rigidbody2D>().AddForce(-LaunchDirection * strength, ForceMode2D.Force);
                ropePieces.Add(newRopePiece);
            }
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, mousePos);
    }
}
