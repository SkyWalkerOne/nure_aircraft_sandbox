using UnityEngine;
using UnityEngine.UI;

public class airlineSelecter : MonoBehaviour
{
    [Header("Airline attributes")]
    public string name;
    public string creator;
    public bool isCustom, isAd, isAvailable;
    public int index;

    [Header("Output displayes")]
    public Text nameText;
    public Text creatorText;
    public GameObject officialSign, customSign, adButton;
    public Button submit;

    public centralAdvertisementManager ad;
    private bool adWatched;

    void Update() {
        if (!adWatched) {
            if (ad.isLoaded()) {
                adButton.GetComponent<Image>().color = Color.green;
                adButton.GetComponent<Button>().interactable = true;
            } 

            if (ad.isFailed()) {
                adButton.GetComponent<Image>().color = Color.red;
                adButton.GetComponent<Button>().interactable = false;
            }

            if (ad.isFinished()) {
                adWatched = true;
                isAvailable = true;
                applyThisAirline();
                adButton.SetActive(false);
            }
        }
    }

    public void selectThisAirline () {
        disableEverything();
        PlayerPrefs.SetInt("airline", index);

        nameText.text = name;
        creatorText.text = "Creator: " + creator;

        officialSign.SetActive(!isCustom);
        customSign.SetActive(isCustom);

        if (isAvailable) {
            applyThisAirline();
        } else if (isAd) {
            adButton.SetActive(true);
        }
    }

    void applyThisAirline () {
        submit.interactable = true;
    }

    void disableEverything () {
        submit.interactable = false;
        adButton.SetActive(false);
    }
}
