using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullController : MonoBehaviour
{
    Animator animator;

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



    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        if(Fatness < 3)
        {
            int intFatness = Mathf.FloorToInt(Fatness);
            Belly.sprite = BellySprites[intFatness];
            Legs.transform.position = LegPositions[intFatness].position;
            Neck.transform.position = NeckPositions[intFatness].position;
            Wing1.transform.position = Wing1Positions[intFatness].position;
            Wing2.transform.position = Wing2Positions[intFatness].position;
        }

        Vector2 v = Head.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10));
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(v.y,v.x)) - 72;
        angle = angle < 0 ? angle + 360 : angle;
        int resultIndex = (int)(angle / 72);

        HeadSprite.sprite = HeadSprites[resultIndex];
        HeadSprite.flipX = (resultIndex == 1 || resultIndex == 3);

    }
}
