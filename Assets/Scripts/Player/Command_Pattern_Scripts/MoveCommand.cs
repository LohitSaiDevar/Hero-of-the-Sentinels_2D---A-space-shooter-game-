using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly Rigidbody rb;
    private readonly float speed, horizontalInput, verticalInput;

    public MoveCommand(Rigidbody _rb, float _speed, float _horizontalInput, float _verticalInput)
    {
        rb = _rb;
        speed = _speed;
        horizontalInput = _horizontalInput;
        verticalInput = _verticalInput;
    }

    public void Execute()
    {
        Vector3 movement = new(horizontalInput, 0, verticalInput);
        movement.Normalize();
        rb.velocity = movement * speed;
    }
}
