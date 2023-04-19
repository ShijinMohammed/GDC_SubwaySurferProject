using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPieceSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _availableRoadPieces;

    [SerializeField] private RoadPieceScript _currentRoadPiece;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomRoadPiece();
        SpawnRandomRoadPiece(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRandomRoadPiece()
    {
        int x = Random.Range(0, _availableRoadPieces.Length);

        GameObject g = Instantiate(_availableRoadPieces[x], _currentRoadPiece.SpawnPoint.position, _currentRoadPiece.SpawnPoint.rotation);

        _currentRoadPiece = g.GetComponent<RoadPieceScript>();
    }
}
