using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public GameObject RopePiecePrefab;
    public GameObject DavePrefab;

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
    public float ReturnStrength = 2.5f;

    public float snapDst = .4f;

    public List<GameObject> ropePieces = new List<GameObject>();
    bool isStopped = true;

    public int MaxPieces = 10;

    private void Awake()
    {
        //GameObject dave = Instantiate(RopePiecePrefab, transform.position,
        //            Quaternion.Euler(0, 0, 0), transform);
        //dave.GetComponent<Rigidbody>().isKinematic = true;
        //ropePieces.Add(dave);
    }
    private void Update()
    {
        if (LastRopePiece)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,ropePieces[0].transform.position,.4f) - Vector3.forward * 10;
        }


        if (isStopped && Input.GetMouseButton(1) && LastRopePiece)
        {
            Vector3 moveVector = transform.position - LastRopePiece.transform.position;
            LastRopePiece.transform.position += moveVector.normalized * Time.deltaTime * ReturnStrength;
            Vector3 v = transform.position - LastRopePiece.transform.position;
            if (v.magnitude < .2f)
            {
                GameObject piece = LastRopePiece;
                ropePieces.Remove(LastRopePiece);
                Destroy(piece);
                if(ropePieces.Count >= 1)
                    LastRopePiece.GetComponent<Rigidbody>().isKinematic = true;
                for (int i = 1; i < ropePieces.Count - 1; i++)
                {
                    ropePieces[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }


        if (ropePieces.Count == 0)
        {
            Vector3 LaunchDirection = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            LaunchDirection.Normalize();
            
            
            if (Input.GetMouseButtonDown(0))
            {
                isStopped = false;
                GameObject newRopePiece = Instantiate(RopePiecePrefab, transform.position, Quaternion.Euler(0, 0,
                Mathf.Atan2(LaunchDirection.y, LaunchDirection.x) * Mathf.Rad2Deg - 90), transform);
                Debug.Log("test");
                ropePieces.Add(newRopePiece);
                Destroy(LastRopePiece.GetComponent<HingeJoint>());
                LastRopePiece.GetComponent<Rigidbody>().useGravity = true;
                LastRopePiece.GetComponent<Rigidbody>().AddForce(-LaunchDirection * Strength, ForceMode.Force);

            }
            
        }

        if (ropePieces.Count > 0 && !isStopped)
        {
            if (ropePieces.Count >= MaxPieces || Input.GetKeyDown(KeyCode.Space))
            {
                isStopped = true;
                LastRopePiece.GetComponent<Rigidbody>().isKinematic = isStopped;
                for (int i = 1; i < ropePieces.Count - 1; i++)
                {
                    ropePieces[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            else if (LastRopePiece)
            {
                Vector3 v = transform.position - LastRopePiece.transform.position;
                if (v.magnitude >= snapDst && ropePieces.Count < MaxPieces)
                {

                    GameObject newRopePiece = Instantiate(RopePiecePrefab, transform.position,
                        Quaternion.Euler(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90), transform);
                    newRopePiece.GetComponent<HingeJoint>().connectedBody = LastRopePiece.GetComponent<Rigidbody>();
                    ropePieces.Add(newRopePiece);
                }
            }
            
        }
        
    } 
}
