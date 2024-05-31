using UnityEngine;

public class PlaneLoader : MonoBehaviour
{
    [SerializeField] private AircraftContentData[] planes;
    [SerializeField] private Transform StartAirport;
    [SerializeField] private FPC player;
    [SerializeField] private chunksGenerator cgen;
    [SerializeField] private GameObject terminalCamera;
    [SerializeField] private GameObject[] allLands, allAirports;

    void Start()
    {
        GameObject loadedPlane = Instantiate(planes[PlayerPrefs.GetInt("plane")].gameObject);
        AircraftContentData loadedPlaneData = loadedPlane.GetComponent<AircraftContentData>();
        loadedPlaneData.plane.SetParent(null);
        loadedPlaneData.canvas.SetParent(null);
        loadedPlaneData.services.SetParent(StartAirport);
        player.seat = loadedPlaneData.seat;
        player.noclip = loadedPlaneData.noclip;
        cgen.plane = loadedPlaneData.plane;
        player.j = loadedPlaneData.PlayerJoystick;
        player.imageLock = loadedPlaneData.ImageLock;
        loadedPlaneData.camManager.firstPersonal = player.gameObject;
        loadedPlaneData.camManager.terminal = terminalCamera;
        loadedPlaneData.radar.lands = allLands;
        loadedPlaneData.radar.aps = allAirports;
        terminalCamera.GetComponent<cameraMover>().sidestick = loadedPlaneData.PilotSidestick;
        player.gameObject.SetActive(true);
        Destroy(loadedPlane);
        Destroy(gameObject);
    }
}
