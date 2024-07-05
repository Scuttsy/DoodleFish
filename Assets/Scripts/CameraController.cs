using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public enum state
    {
        Cannon,
        Fishing,
        Seagull
    }

    [Header("CameraControl")]
    [Range(0.01f, 1f)]
    [SerializeField] private float _lerpSpeed = 0.5f; // the fraction of distance to be covered every second
    [SerializeField] private float _seagullSpeed = 25f; //lerp speed when the seagull is being tracked
    [SerializeField] private Transform[] _stateTransforms = new Transform[2];
    [SerializeField] private float _thresholdDistance = 0.01f;
    [SerializeField] private Transform _currentGull; //currently tracked seagull
    [SerializeField] private float _minHeight;


    public static state CameraState = state.Cannon;
    private float _tempLerpSpeed;
    void Update()
    {
        CheckGullPosition();
        Vector3 destination = Vector3.zero;

        switch (CameraState)
        {
            case state.Cannon:
                destination = _stateTransforms[0].position;
                _tempLerpSpeed = _lerpSpeed;
                break;
            case state.Fishing:
                _tempLerpSpeed = _lerpSpeed;
                destination = _stateTransforms[1].position;
                break;
            case state.Seagull:
                _tempLerpSpeed = _seagullSpeed;
                destination = new Vector3(transform.position.x, _currentGull.position.y, transform.position.z);
                break;
        }

        if (Mathf.Abs((destination.y - transform.position.y)) > _thresholdDistance && transform.position.y > _minHeight)
            transform.position = Vector3.Lerp(transform.position, destination, _tempLerpSpeed * Time.deltaTime);
    }

    private void CheckGullPosition()
    {
        if (_currentGull.position.y < _stateTransforms[1].position.y)
            CameraState = state.Seagull;
    }
}
