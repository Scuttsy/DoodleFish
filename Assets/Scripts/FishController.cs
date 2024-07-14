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
    public float Speed;
    public int AngerValue; // Thew angrier the fish the larger the aggro range
    public FishState State = FishState.Roaming;

    [SerializeField] private  int AngerLimit;
    [SerializeField] private float _roamStepDistance = 1f;
    [SerializeField] private Fish _fish;
    [SerializeField] private float _maxVerticalTravel = 5f;

    [SerializeField] private float _extendThreshold = 2f;
    [SerializeField] private List<Vector3> _bezierPoints = new List<Vector3>();
    [SerializeField] private float _aggroRange = 10f;
    [SerializeField] private float _aggroSpeedBoost = 2f;

    private Vector3 _startingPosition;
    [SerializeField] private Transform _seagullPosition;

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

        if ((_bezierPoints[0] - transform.position).magnitude > 1)
        {
            if (_bezierPoints[0].x > transform.position.x)
                gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            else
                gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    private void Chase()
    {
        //Put Fish aggro in here
        if (!_seagullPosition)
        {
            Debug.Log("Seagull not found");
            State = FishState.Roaming;
            return;
        }

        if ((_bezierPoints[0] - transform.position).magnitude < _extendThreshold)
        {
            _bezierPoints.RemoveAt(0);
            List<Vector3> points = new List<Vector3>();
        
            for (int i = 0; i < 8; i++)
            {
                points.Add(GetNextPosition(transform.position));
            }
            Vector3 closestToSeagull = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                if ((_seagullPosition.position - closestToSeagull).magnitude > (_seagullPosition.position - points[i]).magnitude)
                {
                    closestToSeagull = points[i];
                }
            }
            _bezierPoints.Add(closestToSeagull);
        }



        LerpPosition();
    }

    private void Roam()
    {

        //pick a list of points around the fish and if they aren't occluded then move towards that location at a given speed along a bezier curve
        if ((_bezierPoints[0] - transform.position).magnitude < _extendThreshold)
        {
            _bezierPoints.RemoveAt(0);
            _bezierPoints.Add(GetNextPosition(_bezierPoints[0]));
        }

        LerpPosition();

    }

    private void LerpPosition()
    {
        float speed = Speed;
        if (State == FishState.Chasing)
            speed = Speed * _aggroSpeedBoost;

        Vector3 lerpedPos = Vector3.Lerp(transform.position, _bezierPoints[0], speed * Time.deltaTime);
        lerpedPos = Vector3.Lerp(lerpedPos, _bezierPoints[1], speed * Time.deltaTime);

        transform.position = lerpedPos;
    }

    private Vector3 GetNextPosition(Vector3 start)
    {
        int searchLimit = 8;
        float radius = _roamStepDistance;
        Vector3 position = start;

        if (State == FishState.Chasing)
            radius = Math.Min(_roamStepDistance, (_seagullPosition.position - start).magnitude);
        

        int searchCount = 0;
        do
        {
            searchCount++;
            position = new Vector2(start.x, start.y) + UnityEngine.Random.insideUnitCircle * radius;

        } while (Physics.Raycast(start, (position - start), (position - start).magnitude, LayerMask.GetMask("Environment")) || 
                searchCount < searchLimit || 
                Mathf.Abs(position.y - _startingPosition.y) > _maxVerticalTravel);
            
        return position;
    }

    public void TryAggro(Transform seagull)
    {
        if ((seagull.position - transform.position).magnitude <= _aggroRange)
        {
            _seagullPosition = seagull;
            State = FishState.Chasing;
        }
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
        {
            Gizmos.DrawLine(transform.position, _bezierPoints[0]);
            Gizmos.DrawLine(transform.position, _bezierPoints[1]);
        }
    }

    public enum FishState
    {
        Roaming,
        Chasing,
        Mouse
    }
}
