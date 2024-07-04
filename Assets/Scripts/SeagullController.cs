using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullController : MonoBehaviour
{
    Animator animator;
    [Header("Movement")]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private Transform _anchorPoint;

    //public
    public bool IsRoaming = true;
    public int HitPoints = 10;
    public int NumFishes = 0;
    //internal
    [Range(0f, 3f)]
    public float Fatness = 0;
    public List<Sprite> BellySprites = new List<Sprite>();
    public SpriteRenderer Belly;

    public Transform Head;
    public SpriteRenderer HeadSprite;
    public List<Sprite> HeadSprites;

    public Transform Legs;
    public List<Transform> LegPositions;

    public Transform Neck;
    public List<Transform> NeckPositions;

    public Transform Wing1;
    public List<Transform> Wing1Positions;

    public Transform Wing2;
    public List<Transform> Wing2Positions;

    

    //public float angle;
    public int resultAngle;


    public void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {

        if (IsRoaming)
        {
            transform.position += transform.right * _speed * Time.deltaTime;
        }
        else
        {
            transform.position = _anchorPoint.position;
        }
        if (transform.position.x < -25 || transform.position.x > 15)
            Destroy(gameObject);


        if (Fatness < 3)
        {
            int intFatness = Mathf.FloorToInt(Fatness);
            Belly.sprite = BellySprites[intFatness];
            Legs.transform.position = LegPositions[intFatness].position;
            Neck.transform.position = NeckPositions[intFatness].position;
            Wing1.transform.position = Wing1Positions[intFatness].position;
            Wing2.transform.position = Wing2Positions[intFatness].position;
        }
        if (transform.position.x < -25 || transform.position.x > 15)
            Destroy(gameObject);

        Vector2 v = Head.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10));
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(v.y,v.x)) - 72;
        angle = angle < 0 ? angle + 360 : angle;
        int resultIndex = (int)(angle / 72);

        HeadSprite.sprite = HeadSprites[resultIndex];
        HeadSprite.flipX = (resultIndex == 1 || resultIndex == 3);
    }

    public void CatchSeagull(Transform AnchorPoint)
    {
        IsRoaming = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _anchorPoint = AnchorPoint;
    }

}
