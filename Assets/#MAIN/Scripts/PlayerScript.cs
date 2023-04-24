    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private int _minLaneIndex = -1;
    [SerializeField] private int _maxLaneIndex = 1;
    [SerializeField] private float _laneWidth;
    [SerializeField] private int _currentLaneIndex;
    [SerializeField] private float _laneSwitchSpeed = 3;
    [SerializeField] private Transform _groundRaycastPoint;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private RoadPieceSpawnerScript _roadPieceSpawner;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider[] _ragdollColliders;

    private bool _isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Time.timeScale = 1;
        ChangeStateOfRagdollColliders(false);
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        CheckIfGrounded();
        Animations();
        Movement();

    }
    void Movement()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _speed);

        Vector3 targetPosition = new Vector3(_currentLaneIndex * _laneWidth, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _laneSwitchSpeed);

    }

    void Animations()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, new Vector3(_currentLaneIndex * _laneWidth - transform.position.x, 0, 1), transform.up);
        _animator.SetFloat("XInput", angle / 75);
    }

    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.D))
            LaneChange(+1);
        else if (Input.GetKeyDown(KeyCode.A))
            LaneChange(-1);

        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    }

    void LaneChange(int x)
    {
        if (x > 0 && _currentLaneIndex == _maxLaneIndex)
            return;
        if (x < 0 && _currentLaneIndex == _minLaneIndex)
            return;

        _currentLaneIndex += x;
    }
    void ChangeStateOfRagdollColliders(bool val)
    {
        foreach(Collider r in _ragdollColliders)
        {
            r.enabled = val;
        }
    }
    void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _animator.SetTrigger("Jump");
    }
    void CheckIfGrounded()
    {
        if (Physics.Raycast(_groundRaycastPoint.position, Vector3.down, .1f, _groundLayers))
        {
            _isGrounded = true;
        }
        else
            _isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpawnTrigger"))
            _roadPieceSpawner.SpawnRandomRoadPiece();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }
    void GameOver()
    {
        _gameOverPanel.SetActive(true);
        _animator.enabled = false;
        ChangeStateOfRagdollColliders(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
