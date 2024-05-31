using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMover : MonoBehaviour
{
    public float speedrot;
    public Joystick sidestick;
    private float rotx = 0, roty = 0;
    public bool inversionY, inversionX;

    cameraManager manager;

    void Start () {
        manager = Object.FindObjectOfType<cameraManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && !manager.scrollBarDrag && sidestick.Horizontal == 0 && sidestick.Vertical == 0)
        {
            Touch t = Input.GetTouch(0);
             
            if (t.phase == TouchPhase.Moved)
            {
                roty += (inversionY) ? Input.touches[0].deltaPosition.x * speedrot : -Input.touches[0].deltaPosition.x * speedrot;
                rotx += (inversionX) ? -Input.touches[0].deltaPosition.y * speedrot : Input.touches[0].deltaPosition.y * speedrot;
            }
        }
        transform.localEulerAngles = new Vector3(rotx, roty, 0f);
    }
}
