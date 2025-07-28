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
    [SerializeField]
    private AudioClip _runningSound;
    [SerializeField]
    private SoundManager _soundManager;

    private Vector2 _lastPosition, _velocity;

    private bool _canMove = true;

    private Vector3 _moveDirection;

    private void Start()
    {
        _lastPosition = transform.position;

        if(_soundManager != null)_soundManager.LoadSoundWithOutPath("walking", _runningSound);
    }

    private void FixedUpdate()
    { 
        if (!_canMove) return;

        if (_moveDirection != Vector3.zero)
        {
            float currentAngle = transform.rotation.eulerAngles.y;
            float targetAngle = Mathf.Atan2(-_moveDirection.z, _moveDirection.x) * Mathf.Rad2Deg + 90f;
            float angle = Mathf.LerpAngle(currentAngle, targetAngle, _rotatingSpeed * Time.deltaTime);
            Debug.Log(angle);
            _rb.Move(transform.position + ((_moveDirection * _movingSpeed) * Time.fixedDeltaTime), Quaternion.identity);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        //Small velocity calculation -- Nothing to worry about
        Vector2  currentPosition = transform.position;
        _velocity = (currentPosition - _lastPosition) / Time.deltaTime;
        _lastPosition = currentPosition;

        if(_soundManager != null) PlayerSound();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        float yAxis = ctx.ReadValue<Vector2>().y;
        float xAxis = ctx.ReadValue<Vector2>().x;

        _moveDirection = new Vector3(xAxis, 0, yAxis);
        Debug.Log("Player is moving !!");
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

    public void ChangeCanMove(Component sender, object obj)
    {
        bool? canMove = obj as bool?;

        _canMove = (bool)canMove;
    }

    public void MoveDirectionChanged(Component sender, object obj)
    {
        Vector2? movementInput = obj as Vector2?;
        _moveDirection = new Vector3(movementInput.Value.x, 0, movementInput.Value.y);
    }
}
