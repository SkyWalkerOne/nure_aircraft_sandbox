using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public GameObject[] airlines, itemsToCloseAfterLoading, itemsToRemoveIfUnappropriate, planeKits;
    [SerializeField] private string[] planes;
    [SerializeField] private Text progressText, planeNameText;
    [SerializeField] private Toggle skipper;
    
    private int _selectedAirLine, _selectedPlane;

    void Start () {
        _selectedAirLine = PlayerPrefs.GetInt("airline");
        selectAirline(_selectedAirLine);
        _selectedPlane = PlayerPrefs.GetInt("plane");
        planeNameText.text = planes[_selectedPlane];
        CheckForDeletableElements();
    }

    void selectAirline (int index) {
        foreach(GameObject o in airlines) o.SetActive(false);
        airlines[index].SetActive(true);
        airlines[index].GetComponent<airlineSelecter>().selectThisAirline();
    }

    public void nextAirline() {
        _selectedAirLine++;
        if (_selectedAirLine == airlines.Length) _selectedAirLine = 0;
        selectAirline(_selectedAirLine);
    }

    public void previousAirline() {
        _selectedAirLine--;
        if (_selectedAirLine == -1) _selectedAirLine = airlines.Length - 1;
        selectAirline(_selectedAirLine);
    }
    
    public void nextPlane()
    {
        _selectedPlane++;
        if (_selectedPlane == planes.Length) _selectedPlane = 0;
        planeNameText.text = planes[_selectedPlane];
        PlayerPrefs.SetInt("plane", _selectedPlane);

        CheckForDeletableElements();
    }
    
    public void previousPlane()
    {
        _selectedPlane--;
        if (_selectedPlane == -1) _selectedPlane = planes.Length - 1;
        planeNameText.text = planes[_selectedPlane];
        PlayerPrefs.SetInt("plane", _selectedPlane);

        CheckForDeletableElements();
    }

    public bool SelectedMultiplayerablePlane()
    {
        return _selectedPlane != 1;
    }

    private void CheckForDeletableElements()
    {
        foreach (GameObject obj in itemsToRemoveIfUnappropriate)
        {
            obj.SetActive(_selectedPlane != 1);
        }
        foreach (GameObject obj in planeKits)
        {
            obj.SetActive(false);
        }
        planeKits[_selectedPlane].SetActive(true);
    }

    public int getSelected() {
        return _selectedAirLine;
    }

    public void loadMain() {
        foreach(GameObject o in itemsToCloseAfterLoading) o.SetActive(false);
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene(){
        AsyncOperation async = SceneManager.LoadSceneAsync((skipper.isOn) ? "SampleScene" : "terminal");
        async.allowSceneActivation = false;
        while(async.progress <= 0.89f){
            progressText.text = "loading (" + Mathf.Round(async.progress) * 100 + "%)";
            yield return null;
        }
        async.allowSceneActivation = true;
    }
}
