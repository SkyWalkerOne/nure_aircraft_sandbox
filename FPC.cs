using UnityEngine;
using UnityEngine.UI;

public class FPC : MonoBehaviour
{
    public float speed;
    public Joystick j;
    public Button seat, noclip;
    public GameObject camera, imageLock;
    public float speedrot;
    private soundPlayerChecker _sChecker;
    private float _rotx, _roty;
    private bool _seated, _kinematic;

    void Start () {
        seat.onClick.AddListener(changeSeated);
        noclip.onClick.AddListener(changeKinematic);
        _sChecker = (soundPlayerChecker) FindObjectOfType(typeof(soundPlayerChecker));
    }

    void Update()
    {
        if (j != null && !GetComponent<Rigidbody>().isKinematic) {
            Vector3 movement = transform.forward * j.Vertical * ((_seated) ? speed * 0.5f : speed)
            + transform.right * j.Horizontal * ((_seated) ? speed * 0.2f : speed)
            + transform.up * GetComponent<Rigidbody>().velocity.y;

            GetComponent<Rigidbody>().velocity = movement;
        } else if (_kinematic)
            transform.Translate(j.Horizontal * ((_seated) ? speed * 0.2f : speed) * 0.04f, 0, j.Vertical * ((_seated) ? speed * 0.5f : speed) * 0.04f);

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
             
            if (t.phase == TouchPhase.Moved && j.Vertical == 0 && j.Horizontal == 0)
            {
                _rotx -= Input.touches[0].deltaPosition.y * speedrot;
                _roty += Input.touches[0].deltaPosition.x * speedrot;

                Vector3 e = new Vector3(_rotx, _roty, 0f);
            }
        }
        transform.eulerAngles = new Vector3(0f, _roty, 0f);
        camera.transform.localEulerAngles = new Vector3(_rotx, 0f, 0f);
    }

    public void changeSeated () {
        _seated = !_seated;
        transform.localScale = new Vector3(1f, ((_seated) ? 0.75f : 1f), 1f);
    }

    public void changeKinematic () {
        _kinematic = !_kinematic;
        GetComponent<Rigidbody>().detectCollisions = !_kinematic;
        GetComponent<Rigidbody>().isKinematic = _kinematic;
        imageLock.SetActive(_kinematic);

        if (!_kinematic && !_sChecker.inTrigger) {
            transform.SetParent(null);
            _sChecker.resetSounds();
        }
    }
}
