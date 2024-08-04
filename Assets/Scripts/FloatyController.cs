using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyController : MonoBehaviour
{
    [SerializeField] private Transform BeachedPositions;
    [SerializeField] private Transform Dave;

    [SerializeField] private Transform BeachCamera;
    [SerializeField] private Transform OceanCamera;
    
    [SerializeField] private Transform LeftPiece;
    [SerializeField] private Transform RightPiece;
    [SerializeField] private Rope RopeController;
    
    [SerializeField] private bool OnWater = false;

    bool Moving = false;
    Vector2 position;

    float _time = 0;

    private void Start()
    {
        Camera.main.transform.position = BeachCamera.position;
        position = BeachedPositions.position;
        transform.position = BeachedPositions.position;
        LeftPiece.rotation = BeachedPositions.GetChild(0).rotation;
        RightPiece.rotation = BeachedPositions.GetChild(1).rotation;
        RightPiece.GetChild(0).rotation = BeachedPositions.GetChild(1).GetChild(0).rotation;
    }
    private void FixedUpdate()
    {
        if (OnWater)
        {
            position.y = Mathf.Sin(Time.time) * .1f;
            LeftPiece.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time - 0.4f) * 4);
            RightPiece.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time + 0.4f) * 4);
        }

        if (Moving)
        {
            _time += Time.deltaTime;

            position.x = Mathf.Lerp(BeachedPositions.position.x,0,_time/10);
            Camera.main.transform.position = Vector3.Lerp(BeachCamera.position, OceanCamera.position, _time / 10);
            LeftPiece.rotation = Quaternion.Lerp(LeftPiece.rotation, Quaternion.Euler(0, 0, Mathf.Sin(Time.time - 0.4f) * 4), _time / 10);

            if (_time >= 3)
            {
                OnWater = true;
            } else 
            {
                position.y = Mathf.Lerp(BeachedPositions.position.y, Mathf.Sin(Time.time) * .1f, _time / 3);
                RightPiece.rotation = Quaternion.Lerp(RightPiece.rotation, Quaternion.Euler(0, 0, Mathf.Sin(Time.time + 0.4f) * 4), _time / 3);
            }

            
        }

        if (_time >= 10)
        {
            Moving = false;
        }

        Debug.Log(Moving);
        transform.position = position;
    }

    public void StartMoving()
    {
        Moving = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed " + Moving);
            
            if(!Moving && OnWater)
            {
                Dave.gameObject.SetActive(false);
                RopeController.GenerateRope();
            }
        }
    }

}
