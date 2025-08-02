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

    private Vector2 _velocity;
    private Vector3 _moveDirection;
    private float _cameraXAxis, _yAxis, _xAxis;

    private Animator _playerAnimations;


    private void Start()
    {
        if(_soundManager != null)_soundManager.LoadSoundWithOutPath("walking", _runningSound);
        if (_playerAnimations == null)_playerAnimations = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        float rotationChange;
        //When there is no input the character should not move
        if (_cameraXAxis == 0 && _yAxis == 0 && _xAxis == 0)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rotationChange = _cameraXAxis * (_rotatingSpeed * 100) * Time.fixedDeltaTime;
            Vector3 currentRotation = _rb.rotation.eulerAngles;
            currentRotation.y += rotationChange;
            Quaternion newRotation = Quaternion.Euler(currentRotation);

            _rb.Move(transform.position + ((_moveDirection * _movingSpeed) * Time.fixedDeltaTime), newRotation);
        }
    }

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
         _yAxis = ctx.ReadValue<Vector2>().y;
         _xAxis = ctx.ReadValue<Vector2>().x;

        _moveDirection = transform.forward * _yAxis;
        _moveDirection += transform.right * _xAxis;

        if (_playerAnimations == null) return;
        if (_moveDirection != Vector3.zero) _playerAnimations.SetBool("isMoving", true);
        else _playerAnimations.SetBool("isMoving", false);

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

}
