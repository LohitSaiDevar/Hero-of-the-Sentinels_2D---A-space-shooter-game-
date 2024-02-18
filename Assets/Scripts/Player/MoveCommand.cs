using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly Transform transform;
    private readonly float speed, horizontalInput, verticalInput;

    public MoveCommand(Transform _transform, float _speed, float _horizontalInput, float _verticalInput)
    {
        transform = _transform;
        speed = _speed;
        horizontalInput = _horizontalInput;
        verticalInput = _verticalInput;
    }

    public void Execute()
    {
        Vector3 movement = new(horizontalInput, 0, verticalInput);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
