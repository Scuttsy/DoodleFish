using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnController : MonoBehaviour
{


    [Header("Fish")]
    [SerializeField] private Transform[] _fishSpawnPositions;
    [SerializeField] private List<GameObject> _fishPrefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnDelay;

    private float _spawnTimer = 0;
    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnDelay)
        {
            _spawnTimer = 0;

            //spawn Fish
            SpawnEntitiy(_fishSpawnPositions, _fishPrefabs[Random.Range(0, _fishPrefabs.Count)]);
        }
    }

    private void SpawnEntitiy(Transform[] positions, GameObject prefab)
    {
        Transform spawnTransform = positions[Random.Range(0, positions.Length)];
        Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);
    }
}
