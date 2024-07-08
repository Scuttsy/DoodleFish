using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Keybinds")]
    [SerializeField] private KeyCode _rotateLeft;
    [SerializeField] private KeyCode  _rotateRight;
    [SerializeField] private KeyCode  _fire;

    [Header("CannonControl")]
    [SerializeField] private Transform _cannonTransform;
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private Transform _seagullLasso;
    [SerializeField] private float _rotateSpeed = 2f;
    [SerializeField] private float _fireCooldown = 2f;
    [SerializeField] private float _fireForce = 10f;
    [SerializeField] private GameObject _cannonRope;

    //Public variables
    public bool CanFire = true;


    //private variables
    private float _fireCooldowntimer = 0;

    public bool IsControllingCannon
    {
        get
        {
            return Camera.main.transform.position.y > _cannonTransform.position.y;
        }
    }

    void Update()
    {
        //test camera swap
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CameraController.CameraState == CameraController.state.Cannon)
                CameraController.CameraState = CameraController.state.Fishing;
            else 
                CameraController.CameraState = CameraController.state.Cannon;
        }

        if (IsControllingCannon)
        {
            Vector2 mousePos = _cannonTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //_cannonTransform.LookAt(new Vector3(mousePos.x, mousePos.y, 0));
            //_cannonTransform.LookAt(mousePos);
            _cannonTransform.rotation = Quaternion.Euler(0, 0, MathF.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90);    
            if (Input.GetKeyDown(_fire) && CanFire)
                Fire();

            if (_fireCooldowntimer < _fireCooldown)
            {
                _fireCooldowntimer += Time.deltaTime;
                if (_fireCooldown < _fireCooldowntimer)
                    CanFire = true; 
            }
        }
    }

    private void Fire()
    {
        CanFire = false;
        _fireCooldowntimer = 0;
        
        GameObject cannonRope = Instantiate(_cannonRope, _fireTransform.position, _fireTransform.rotation);
        _seagullLasso = cannonRope.transform;
        cannonRope.GetComponent<Rigidbody>().AddForce(_fireTransform.up * _fireForce, ForceMode.Impulse);
    }
}
