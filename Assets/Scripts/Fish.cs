using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Fish : ScriptableObject
{
    public int EatValue = 0;
    public FishType Type;
    public FishColor Color;

}

public enum FishType
{
    Seahorse,
    Pufferfish,
    FastBoi,
    Annoyed,
    SmallFish,
    BigFish
}

public enum FishColor
{
    red,
    yellow,
    blue
}