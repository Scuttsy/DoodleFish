using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 mousePos;
    public GameObject RopePiecePrefab;
    private Rigidbody rb;

    private GameObject LastRopePiece
    {
        get
        {
            if (ropePieces.Count - 1 >= 0)
                return ropePieces[ropePieces.Count - 1];
            else return null;
        }
    }

    public int Strength = 6000;

    List<GameObject> ropePieces = new List<GameObject>();
    HingeJoint ropeJoint;
    bool isStopped;

    public int MaxPieces = 10;

    private void Start()
    {
        ropeJoint = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isStopped && Input.GetMouseButton(1) && LastRopePiece)
        {
            Vector3 moveVector = transform.position - LastRopePiece.transform.position;
            LastRopePiece.transform.position += moveVector.normalized * Time.deltaTime;
            Vector3 v = transform.position - LastRopePiece.transform.position;
            if (v.magnitude < .2f)
            {
                GameObject piece = LastRopePiece;
                ropePieces.Remove(LastRopePiece);
                Destroy(piece);
                if(ropePieces.Count >= 1)
                    LastRopePiece.GetComponent<Rigidbody>().isKinematic = true;
            }
        }


        if (Input.GetMouseButtonDown(0) && ropePieces.Count == 0)
        {
            isStopped = false;
            ropePieces.Clear();
            Vector3 LaunchDirection = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            mousePos = Vector3.zero + LaunchDirection;
            LaunchDirection.Normalize();
            Debug.Log(transform.position);
            GameObject newRopePiece = Instantiate(RopePiecePrefab, transform.position,
                    Quaternion.Euler(0, 0, Mathf.Atan2(LaunchDirection.y, LaunchDirection.x) * Mathf.Rad2Deg - 90), transform.GetChild(0));

            Destroy(newRopePiece.GetComponent<HingeJoint>());
            newRopePiece.GetComponent<Rigidbody>().AddForce(-LaunchDirection * Strength, ForceMode.Force);
            
            ropePieces.Add(newRopePiece);
            Debug.Log(ropePieces.Count);
        }

        if (ropePieces.Count > 0)
        {
            if (ropePieces.Count >= MaxPieces || Input.GetKeyDown(KeyCode.Space))
            {
                isStopped = true;
                LastRopePiece.GetComponent<Rigidbody>().isKinematic = isStopped;

            }
            else if (!isStopped && LastRopePiece)
            {
                Vector3 v = transform.position - LastRopePiece.transform.position;
                if (v.magnitude >= 1 && ropePieces.Count < MaxPieces)
                {
                    GameObject newRopePiece = Instantiate(RopePiecePrefab, LastRopePiece.transform.position + (v.normalized * .4f),
                        Quaternion.Euler(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90), transform.GetChild(0));
                    newRopePiece.GetComponent<HingeJoint>().connectedBody = LastRopePiece.GetComponent<Rigidbody>();
                    ropePieces.Add(newRopePiece);
                }
            }
            
        }

        if (ropePieces.Count == 0 && UIManager.CurrentScore > 0)
        {
            CameraController.CameraState = CameraController.state.Smacking;
        }
        
    } 
}
