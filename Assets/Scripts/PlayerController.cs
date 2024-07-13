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
    public static bool CanFire = true;

    public bool IsControllingCannon
    {
        get
        {
            return Camera.main.transform.position.y > _cannonTransform.position.y;
        }
        set
        {
            IsControllingCannon = value;

        }
    }

    void Update()
    {
        if (IsControllingCannon)
        {
            Vector2 mousePos = _cannonTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _cannonTransform.rotation = Quaternion.Euler(0, 0, MathF.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90);

            if (Input.GetKeyDown(_fire) && CanFire)
                IsControllingCannon = false;

        }
    }

    //private void Fire()
    //{
    //    CanFire = false;
        
    //    GameObject cannonRope = Instantiate(_cannonRope, _fireTransform.position, _fireTransform.rotation);
    //    _seagullLasso = cannonRope.transform;
    //    cannonRope.GetComponent<Rigidbody>().AddForce(_fireTransform.up * _fireForce, ForceMode.Impulse);
    //}
}
