using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeagullController : MonoBehaviour
{
    Animator animator;
    public Animator HeadAnimator;
    [Header("Movement")]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private Transform _anchorPoint;

    public bool IsRoaming = true;
    public int HitPoints = 10;
    public int NumFishes = 0;
    public LayerMask fishyMask;


    [Header("Eating Settings")]
    [SerializeField] private float _aggroRange = 10f;
    [SerializeField] private List<GameObject> _guts;
    bool exploded;

    [Header("Skyla's section")]
    [Range(0f, 5f)]
    public float Fatness = 0;
    public List<Sprite> BellySprites = new List<Sprite>();
    public SpriteRenderer Belly;

    public Transform Head;
    public float LookRadius;
    public float EatRadius;

    public Transform Legs;
    public Transform LegPositions;

    public Transform Neck;
    public Transform NeckPositions;

    public Transform Wing1;
    public Transform Wing1Positions;

    public Transform Wing2;
    public Transform Wing2Positions;

    

    //public float angle;
    public int resultAngle;


    public void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {

        if (Fatness < 5)
        {
            int intFatness = Mathf.FloorToInt(Fatness);
            Belly.sprite = BellySprites[intFatness];
            Legs.transform.position = LegPositions.GetChild(intFatness).position;
            Neck.transform.position = NeckPositions.GetChild(intFatness).position;
            Wing1.transform.position = Wing1Positions.GetChild(intFatness).position;
            Wing2.transform.position = Wing2Positions.GetChild(intFatness).position;
        }
        else if (!exploded)
        {
            Explode();
        }

        //if (transform.position.x < -25 || transform.position.x > 15)
        //    Destroy(gameObject);


        Collider2D fishy = Physics2D.OverlapCircle(Head.transform.position, LookRadius, fishyMask);
        if (fishy)
        {
            Vector2 v = Head.position - fishy.transform.position;
            float angle = (Mathf.Rad2Deg * Mathf.Atan2(v.y, v.x)) - 72;
            angle = angle < 0 ? angle + 360 : angle;
            int resultIndex = (int)(angle / 72);
            resultAngle = resultIndex;
            HeadAnimator.SetFloat("Headpos", resultIndex);
            HeadSprite.sprite = HeadSprites[resultIndex];
            HeadSprite.flipX = (resultIndex == 1 || resultIndex == 3);
            if (v.magnitude < EatRadius)
            {
                fishy.GetComponent<FishController>().Eat();

                Collider[] fishes = Physics.OverlapSphere(Head.position, _aggroRange);
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));


        Vector2 v = Head.position - mousePos;
        if(v.magnitude < LookRadius)
        {
            float angle = (Mathf.Rad2Deg * Mathf.Atan2(v.y, v.x)) - 60;
            angle = angle < 0 ? angle + 360 : angle;
            resultAngle = (int)(angle / 60);
        } else
        {
            resultAngle = 6;
        }
        HeadAnimator.SetFloat("Headpos", resultAngle);
        if (Input.GetMouseButtonDown(0))
        {
            HeadAnimator.SetTrigger("Eat");
        }


    }

    private void Explode()
    {
        exploded = true;
        for (int i = 0; i < _guts.Count; i++) 
        {
            Vector3 v = Random.insideUnitSphere;
            Rigidbody rb = Instantiate(_guts[i],Belly.transform.position + v/4,Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(v.normalized,ForceMode.Impulse);
        }
        
        Belly.gameObject.SetActive(false);
        Legs.gameObject.SetActive(false);
        Neck.gameObject.SetActive(false);
        Wing1.gameObject.SetActive(false);
        Wing2.gameObject.SetActive(false);
    }

    public void CatchSeagull(Transform AnchorPoint)
    {
        IsRoaming = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _anchorPoint = AnchorPoint;
    }

}
