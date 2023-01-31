using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector3 _movementInput;
    private Vector3 _mousePosition;

    [SerializeField] float epsilon = 0.1f;

    private Rigidbody _rb;
    private Animator _animator;

    public float Speed = 5f;
    [Range(0f, 1f)] public float SmoothRotation = 0.5f;

    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        if (plane.Raycast(ray, out float distance)) return ray.GetPoint(distance);
        else return transform.position;
    }

    private void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _animator = _rb.GetComponent<Animator>();
        GameState.Main.addStartEvent(StartRun, 1);
        GameState.Main.addPauseEvent(Pause, 1);
        GameState.Main.addUnpauseEvent(Unpause, 1);
    }

    public void StartRun()
    {
        transform.position = new Vector3(0, 1.01f, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        _animator.speed = 1f;
    }

    public void Pause()
    {
        _animator.speed = 0f;
    }

    public void Unpause()
    {
        _animator.speed = 1f;
    }

    void Update()
    {
        _mousePosition = GetMousePosition();
        _movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // animator
        // NOT MOVEMENT INPUT BUT RIGIDBODY
        if (_movementInput.y < -epsilon) _animator.SetBool("Fall", true);
        else _animator.SetBool("Fall", false);
        if (_movementInput.x < -epsilon) _animator.SetBool("GoLeft", true);
        else _animator.SetBool("GoLeft", false);
        if (_movementInput.x > epsilon) _animator.SetBool("GoRight", true);
        else _animator.SetBool("GoRight", false);
        if (_movementInput.z < -epsilon) _animator.SetBool("GoBackward", true);
        else _animator.SetBool("GoBackward", false);
        if (_movementInput.z > epsilon) _animator.SetBool("RunForward", true);
        else _animator.SetBool("RunForward", false);

        if (GameState.Main.State == GamePeriod.Running)
        {
            // go to FixedUpdates
            _rb.MovePosition(_rb.position + _rb.rotation * _movementInput * Speed * Time.deltaTime);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, _rb.rotation * Quaternion.FromToRotation(transform.forward, _mousePosition - transform.position).normalized, Time.deltaTime * 50 * SmoothRotation));
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    private void FixedUpdate()
    {

    }
}
