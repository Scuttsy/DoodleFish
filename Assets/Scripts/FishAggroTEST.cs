using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishAggroTEST : MonoBehaviour
{

    private void Update()
    {
        Collider[] fishes = Physics.OverlapSphere(transform.position, transform.localScale.x, LayerMask.GetMask("Fishy"));
        for (int i = 0; i < fishes.Length; i++)
        {
            FishController fish = fishes[i].gameObject.GetComponent<FishController>();

            if (fish.State == FishController.FishState.Roaming)
            {
                fish.TryAggro(transform);
            }
        }
    }
}
