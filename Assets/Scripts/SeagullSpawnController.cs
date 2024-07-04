using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullSpawnController : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private Transform[] _spawnPositionss;
    [SerializeField] private GameObject _spawnPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnDelay;

    private float _spawnTimer = 0;
    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnDelay)
        {
            _spawnTimer = 0;

            Transform spawnTransform = _spawnPositionss[Random.Range(0, _spawnPositionss.Length)];
            Instantiate(_spawnPrefab, spawnTransform.position, spawnTransform.rotation);
        }
    }
}
