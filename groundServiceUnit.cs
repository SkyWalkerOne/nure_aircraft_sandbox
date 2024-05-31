using UnityEngine;
using Photon.Pun;

public class groundServiceUnit : MonoBehaviour
{
    public bool isAttached;
    public GameObject onAttachedSkin, onDisabledSkin;
    public float xMove, zMove;
    public int maxTime = 1000;

    private bool moving;
    private int time;

    public bool isMultiPlayer;
    public PhotonView sync; //nullable
    public int mpUpdateTime;
    public int mpId;

    int mpTime = 0;
    private adminMonitor monitor;

    void Start() {
        if (sync != null)
            monitor = sync.gameObject.GetComponent<adminMonitor>();
    }

    public void move() {
        if (!isMultiPlayer) moving = (!moving) ? true : moving;
        else sendGUnitData(!isAttached);
    }

    void FixedUpdate()
    {
        if (moving) {
            time++;

            transform.Translate(((isAttached) ? - xMove : xMove), 0, ((isAttached) ? - zMove : zMove));
            if (onAttachedSkin != null) onAttachedSkin.SetActive(false);
            if (onDisabledSkin != null) onDisabledSkin.SetActive(true);

            if (time >= maxTime) {
                time = 0;
                moving = false;
                if (!isAttached) {
                    isAttached = true;
                } else {
                    if (onAttachedSkin != null) onAttachedSkin.SetActive(true);
                    if (onDisabledSkin != null) onDisabledSkin.SetActive(false);
                    isAttached = false;
                }
            }
        }

        if (isMultiPlayer) {
            mpTime++;
            if(mpTime == mpUpdateTime) {
                if (monitor.isAdmin) sendGUnitData(isAttached);
                mpTime = 0;
            }
        }
    }

    private void sendGUnitData(bool attached) {
        if (sync != null)
            sync.RPC("applyGroundUnit", RpcTarget.All, mpId, attached);
    }

    public void recieveGUnitData(bool attached) {
        if (attached != isAttached) moving = (!moving) ? true : moving;
    }
}
