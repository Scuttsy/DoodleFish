using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class FishController : MonoBehaviour
{
    public static int NumFish = 10;
    public enum FishState
    {
        Roaming,
        Chasing,
        Mouse
    }


    public float Speed;

    public int AngerValue; // Thew angrier the fish the larger the aggro range
    public FishState State = FishState.Roaming;

    [SerializeField] private  int AngerLimit;
    [SerializeField] private float _roamStepDistance = 1f;
    [SerializeField] private Fish _fish;
    [SerializeField] private float _maxVerticalTravel = 5f;

    [SerializeField] private float _extendThreshold = 2f;
    [SerializeField] private List<Vector3> _bezierPoints = new List<Vector3>();

    private Vector3 _startingPosition;

    private void Start()
    {
        _bezierPoints.Add(GetNextPosition(transform.position));
        _bezierPoints.Add(GetNextPosition(_bezierPoints[0]));
        _startingPosition = transform.position;
    }
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
        //transform.position += transform.right * Speed * Time.deltaTime;

        if ((_bezierPoints[0] - transform.position).magnitude < _extendThreshold)
        {
            _bezierPoints.RemoveAt(0);
            _bezierPoints.Add(GetNextPosition(_bezierPoints[0]));
            Debug.Log((_bezierPoints[0] - transform.position).magnitude);
        }

        Vector3 lerpedPos = Vector3.Lerp(transform.position, _bezierPoints[0], Speed * Time.deltaTime);
        lerpedPos = Vector3.Lerp(lerpedPos, _bezierPoints[1], Speed * Time.deltaTime);

        transform.position = lerpedPos;

        //pick a list of points around the fish and if they aren't occluded then move towards that location at a given speed
    }

    private Vector3 GetNextPosition(Vector3 Start)
    {
        int searchLimit = 8;
        Vector3 position = Start;
        
        int searchCount = 0;
        do
        {
            searchCount++;
            // _nextPosition1 = possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Length)];
            position = new Vector2(Start.x, Start.y) + UnityEngine.Random.insideUnitCircle * _roamStepDistance;
                
        } while (Physics.CheckSphere(position, gameObject.GetComponent<SphereCollider>().radius) && 
                searchCount < searchLimit && 
                Mathf.Abs(position.y - _startingPosition.y) > _maxVerticalTravel);

        return position;
    }

    public void Eat()
    {
        UIManager.Fishies.Add(_fish);
        Destroy(gameObject);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_bezierPoints.Count > 0)
            Gizmos.DrawLine(transform.position, _bezierPoints[0]);
    }
}
