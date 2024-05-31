using System;
using System.Collections;
using UnityEngine;

public class PlaneDoorManager : MonoBehaviour
{
    [SerializeField] private float doorSpeed;
    
    [Header("Pax doors")]
    [SerializeField] private GameObject frontLeftPax;
    [SerializeField] private GameObject frontRightPax, backLeftPax, backRightPax;

    [Header("Cargo doors")] 
    [SerializeField] private GameObject frontCargo;
    [SerializeField] private GameObject backCargo;

    private Door FLPax, FRPax, BLPax, BRPax, FCargo, BCargo;

    void Start()
    {
        FLPax = new RotatingPaxDoor(this, frontLeftPax, doorSpeed, true);
        FRPax = new RotatingPaxDoor(this, frontRightPax, doorSpeed, false);
        BLPax = new RotatingPaxDoor(this, backLeftPax, doorSpeed, true);
        BRPax = new RotatingPaxDoor(this, backRightPax, doorSpeed, false);

        FCargo = new CargoDoor(this, frontCargo, doorSpeed);
        BCargo = new CargoDoor(this, backCargo, doorSpeed);
    }

    [ContextMenu("Move FL pax")]
    public void MoveFrontLeft() => FLPax.Switch();
    [ContextMenu("Move FR pax")]
    public void MoveFrontRight() => FRPax.Switch();
    [ContextMenu("Move BL pax")]
    public void MoveBackLeft() => BLPax.Switch();
    [ContextMenu("Move BR pax")]
    public void MoveBackRight() => BRPax.Switch();
    
    [ContextMenu("Move F cargo")]
    public void MoveFrontCargo() => FCargo.Switch();
    [ContextMenu("Move B cargo")]
    public void MoveBackCargo() => BCargo.Switch();
}

public class Door
{
    internal readonly MonoBehaviour _monoBehaviour;
    internal readonly GameObject _door;
    internal readonly float _speed;
    internal bool _isOpen, _inProcess;

    public Door(MonoBehaviour monoBehaviour, GameObject door, float speed)
    {
        _monoBehaviour = monoBehaviour;
        _door = door;
        _speed = speed;
    }

    protected internal virtual void Switch() {}
    
    internal void RoundDoorRotation()
    {
        Quaternion currentRotation = _door.transform.rotation;

        float x = Mathf.Round(currentRotation.eulerAngles.x);
        float y = Mathf.Round(currentRotation.eulerAngles.y);
        float z = Mathf.Round(currentRotation.eulerAngles.z);

        _door.transform.rotation = Quaternion.Euler(x, y, z);
    }
}

public class RotatingPaxDoor : Door
{
    private readonly bool _clockwiseRotation;

    public RotatingPaxDoor(MonoBehaviour monoBehaviour, GameObject door, float speed, bool clockwiseRotation)
        : base(monoBehaviour, door, speed)
    {
        _clockwiseRotation = clockwiseRotation;
    }

    private IEnumerator OpenDoor()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 165) > 0f)
        {
            float appendValue = (degrees + _speed < 165)
                ? _speed : (165 - degrees);
            
            _door.transform.Rotate(0, (_clockwiseRotation) ? appendValue : -appendValue, 0);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }

        RoundDoorRotation();
        _isOpen = true;
        _inProcess = false;
    }
    
    private IEnumerator CloseDoor()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 165) > 0f)
        {
            float appendValue = (degrees + _speed < 165)
                ? _speed : (165 - degrees);
            
            _door.transform.Rotate(0, (_clockwiseRotation) ? -appendValue : appendValue, 0);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }

        RoundDoorRotation();
        _isOpen = false;
        _inProcess = false;
    }

    protected internal override void Switch()
    {
        if (_inProcess) return;
        _inProcess = true;
        _monoBehaviour.StartCoroutine(_isOpen ? CloseDoor() : OpenDoor());
    }
}

public class CargoDoor : Door
{
    public CargoDoor(MonoBehaviour monoBehaviour, GameObject door, float speed)
        : base(monoBehaviour, door, speed) {}

    private IEnumerator OpenDoor()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 165) > 0f)
        {
            float appendValue = (degrees + _speed < 165)
                ? _speed : (165 - degrees);
            
            _door.transform.Rotate(0, 0, -appendValue);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }

        RoundDoorRotation();
        _isOpen = true;
        _inProcess = false;
    }
    
    private IEnumerator CloseDoor()
    {
        float degrees = 0f;
        
        while (Math.Abs(degrees - 165) > 0f)
        {
            float appendValue = (degrees + _speed < 165)
                ? _speed : (165 - degrees);
            
            _door.transform.Rotate(0, 0, appendValue);
            degrees += appendValue;
            yield return new WaitForFixedUpdate();
        }

        RoundDoorRotation();
        _isOpen = false;
        _inProcess = false;
    }

    protected internal override void Switch()
    {
        if (_inProcess) return;
        _inProcess = true;
        _monoBehaviour.StartCoroutine(_isOpen ? CloseDoor() : OpenDoor());
    }
}
