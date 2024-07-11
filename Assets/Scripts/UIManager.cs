using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("DepthSlider")]
    public bool isSeagullCaptive = true;
    [SerializeField] private RectTransform _seagullIcon;
    [SerializeField] private RectTransform _maxUIHeight;
    [SerializeField] private RectTransform _minUIHeight;
    [SerializeField] private Transform _maxWorldHeight;
    [SerializeField] private Transform _minWorldHeight;
    [SerializeField] private Transform _trackedSeagull;


    [Header("Oxygen Bubbles")]
    [SerializeField] private List<GameObject> _bubbles = new List<GameObject>();
    [SerializeField] private float _timeBetweenPop = 2f;
    private float _oxygenTimer = 0;
    private int _numPoppedBubbles = 0;

    [Header("Score")]
    [SerializeField] private int _goalScore = 15;
    [SerializeField] private int _currentScore = 0; //fish returned to the basket
    [SerializeField] private Transform _goalSlider;


    void Update()
    {
        if (isSeagullCaptive)
        {
            DrawSeagullHead();
            if (_trackedSeagull.position.y < _maxWorldHeight.position.y)
            {
                _oxygenTimer += Time.deltaTime;

                if (_oxygenTimer >  _timeBetweenPop)
                {
                    _oxygenTimer = 0;
                    if (_numPoppedBubbles < _bubbles.Count)
                    {
                        _bubbles[_bubbles.Count - 1 - _numPoppedBubbles].GetComponent<Image>().enabled = false;
                        _numPoppedBubbles++;
                    }
                    else
                    {
                        //seagull dies
                    }
                }
            }
        }

        _goalSlider.transform.localScale = new Vector3(1 + _currentScore / _goalScore, 1, 1);
    }

    private void DrawSeagullHead()
    {
        float height = map(_trackedSeagull.position.y, _maxWorldHeight.position.y, _minWorldHeight.position.y, _maxUIHeight.position.y, _minUIHeight.position.y);
        height = Math.Clamp(height, 0, 1);
        _seagullIcon.position = new Vector3(_seagullIcon.position.x, height);
    }

    public float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}
