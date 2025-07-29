using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerinput;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private float _movingSpeed = 5, _rotatingSpeed = 1;

    //Audio
    [SerializeField]
    private AudioClip _runningSound;
    [SerializeField]
    private SoundManager _soundManager;

    private Vector2 _lastPosition, _velocity;

    private bool _canMove = true;

    private Vector3 _moveDirection, _rotateDirection;

    private float _currentCameraAngle, _cameraXAxis, _distanceFromPlayer, _yAxis, _xAxis;

    private Camera _playerCamera;

    private void Start()
    {
        if (_playerCamera == null) Debug.Log("No Camera assigned, please assign one!");

        _playerCamera = _playerinput.camera;

        _lastPosition = transform.position;

        if(_soundManager != null)_soundManager.LoadSoundWithOutPath("walking", _runningSound);

        _distanceFromPlayer = Vector3.Distance(transform.position, _playerCamera.transform.position);

        _rotateDirection = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        //if (!_canMove) return;

        //if (_moveDirection != Vector3.zero)
        //{
        //    float currentAngle = transform.rotation.eulerAngles.y;
        //    float targetAngle = Mathf.Atan2(-_moveDirection.z, _moveDirection.x) * Mathf.Rad2Deg + 90f;
        //    float angle = Mathf.LerpAngle(currentAngle, targetAngle, _rotatingSpeed * Time.deltaTime);
        //    Debug.Log(angle);
        //    _rb.Move(transform.position + ((_moveDirection * _movingSpeed) * Time.fixedDeltaTime), Quaternion.identity);
        //    transform.rotation = Quaternion.Euler(0, angle, 0);
        //}

        ////Small velocity calculation -- Nothing to worry about
        //Vector2  currentPosition = transform.position;
        //_velocity = (currentPosition - _lastPosition) / Time.deltaTime;
        //_lastPosition = currentPosition;

        //if(_soundManager != null) PlayerSound();


        float rotationChange = _xAxis * _rotatingSpeed * Time.fixedDeltaTime;
        Vector3 currentRotation = _rb.rotation.eulerAngles;
        currentRotation.y += rotationChange;
        Quaternion newRotation = Quaternion.Euler(currentRotation);

        
         

        //_rotateDirection += new Vector3(0,_xAxis * _rotatingSpeed * Time.deltaTime,0);
        _rb.Move(transform.position + ((_moveDirection * _movingSpeed) * Time.fixedDeltaTime)
            , newRotation);


        _currentCameraAngle += _cameraXAxis * _rotatingSpeed * Time.deltaTime;

        //Calculate cameras position 
        float radianAngle = _currentCameraAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Sin(radianAngle) * _distanceFromPlayer,
            11f,
            Mathf.Cos(radianAngle) * _distanceFromPlayer);

        _playerCamera.transform.position = transform.position + offset;
        _playerCamera.transform.LookAt(transform.position);

        //float yRotation = _playerCamera.transform.rotation.y;

        //transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
         _yAxis = ctx.ReadValue<Vector2>().y;
         _xAxis = ctx.ReadValue<Vector2>().x;

        _moveDirection = transform.forward * _yAxis;
        
    }

    public void MoveCamera(InputAction.CallbackContext ctx)
    {
        _cameraXAxis = ctx.ReadValue<Vector2>().x;
    }


    private void PlayerSound()
    {
        if (_runningSound == null) return;

        if (_velocity == Vector2.zero)
        {
            _soundManager.StopSound();
        }
        if (_velocity != Vector2.zero && !_soundManager.SfxSource.isPlaying)
        {
            _soundManager.SetSFXVolume(1);
            _soundManager.PlaySound("walking");
        }
    }

    //public void ChangeCanMove(Component sender, object obj)
    //{
    //    bool? canMove = obj as bool?;

    //    _canMove = (bool)canMove;
    //}

    //public void MoveDirectionChanged(Component sender, object obj)
    //{
    //    Vector2? movementInput = obj as Vector2?;
    //    _moveDirection = new Vector3(movementInput.Value.x, 0, movementInput.Value.y);
    //}
}
