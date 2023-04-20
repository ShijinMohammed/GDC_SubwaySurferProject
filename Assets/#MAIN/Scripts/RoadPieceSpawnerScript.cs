using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPieceSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _availableRoadPieces;

    [SerializeField] private RoadPieceScript _currentRoadPiece;
    private List<GameObject> _spawnedRoadPieces = new List<GameObject>();
    [SerializeField] private int _numberOfDefaultRoadPieces = 3;

    private int _numberOfRoadPiecesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _numberOfDefaultRoadPieces; i++)
            SpawnRandomRoadPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRandomRoadPiece()
    {
        if(_numberOfRoadPiecesSpawned - 2 > _numberOfDefaultRoadPieces)
            DeleteOldestPiece();

        _numberOfRoadPiecesSpawned++;

        int x = Random.Range(0, _availableRoadPieces.Length);

        GameObject g = Instantiate(_availableRoadPieces[x], _currentRoadPiece.SpawnPoint.position, _currentRoadPiece.SpawnPoint.rotation);

        _spawnedRoadPieces.Add(g);
        _currentRoadPiece = g.GetComponent<RoadPieceScript>();
    }
    void DeleteOldestPiece()
    {
        if (_spawnedRoadPieces.Count > 0)
        {
            Destroy(_spawnedRoadPieces[0]);
            _spawnedRoadPieces.RemoveAt(0);
        }
    }
}
