using System.Collections;
using UnityEngine;
using Photon.Pun;

public class baggageDoorsController : MonoBehaviour
{
    private BaggageDoor[] doors;

    [Header("Doors options")][SerializeField] private GameObject[] objs;
    [SerializeField] private float[] openSpeed, maxAngle;

    [Header("MP options")][SerializeField] private bool isMultiPlayer;
    [Tooltip("nullable")][SerializeField] private PhotonView sync;
    
    private adminMonitor monitor;

    void Start () {
        if (sync != null)
            monitor = sync.gameObject.GetComponent<adminMonitor>();

        doors = new BaggageDoor[objs.Length];
        for (int i = 0; i < objs.Length; i++)
            doors[i] = new BaggageDoor(objs[i], openSpeed[i], maxAngle[i], this);

        if (isMultiPlayer) StartCoroutine(UpdateMpInfromation(2f));
    }

    public void MoveDoor (int index) {
        if (!isMultiPlayer) doors[index].StartMove();
        else sendDoorsData(new[]{!doors[0].IsOpen, doors[1].IsOpen});
    }

    private void sendDoorsData(bool[] newOptions) {
        if (sync != null)
            sync.RPC("applyBaggageDoors", RpcTarget.All, newOptions);
    }

    public void recieveDoorData(bool[] doorOptions) {
        for (int i = 0; i < doorOptions.Length; i++)
            if (doors[i].IsOpen != doorOptions[i]) doors[i].StartMove();
    }

    IEnumerator UpdateMpInfromation(float delay)
    {
        while (true)
        {
            if (monitor.isAdmin)
                sendDoorsData(new[]{doors[0].IsOpen, doors[1].IsOpen});

            yield return new WaitForSeconds(delay);
        }
    }
}

public class BaggageDoor {
    private GameObject obj;
    private bool isOpen, moving;
    private float openSpeed, maxOpenDegrees;
    private float currientDeg;
    private MonoBehaviour _monoBehaviour;

    public BaggageDoor(GameObject obj, float openSpeed, float maxOpenDegrees, MonoBehaviour _monoBehaviour)
    {
        this.obj = obj;
        this.openSpeed = openSpeed;
        this.maxOpenDegrees = maxOpenDegrees;
        this._monoBehaviour = _monoBehaviour;
    }

    public void StartMove()
    {
        if (!moving) _monoBehaviour.StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        moving = true;
        
        if (isOpen)
        {
            while (currientDeg > 0)
            {
                obj.transform.Rotate(0, 0, (currientDeg - openSpeed < 0) ? -currientDeg : -openSpeed);
                currientDeg += (currientDeg - openSpeed < 0) ? -currientDeg : -openSpeed;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (currientDeg < maxOpenDegrees)
            {
                obj.transform.Rotate(0, 0, (currientDeg + openSpeed > maxOpenDegrees) ? maxOpenDegrees - currientDeg : openSpeed);
                currientDeg += (currientDeg + openSpeed > maxOpenDegrees) ? maxOpenDegrees - currientDeg : openSpeed;
                yield return new WaitForFixedUpdate();
            }
        }

        isOpen = !isOpen;
        moving = false;
    }

    public bool IsOpen => isOpen;
}
