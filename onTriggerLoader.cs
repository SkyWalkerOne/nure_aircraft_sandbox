using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class onTriggerLoader : MonoBehaviour
{
    public Text progressText;
    public string sceneName, triggerName;

    public void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == triggerName) {
            StartCoroutine(loadScene());
        }
    }

    IEnumerator loadScene(){
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        while(async.progress <= 0.89f){
            progressText.text = "loading (" + Mathf.Round(async.progress) * 100 + "%)";
            yield return null;
        }
        async.allowSceneActivation = true;
    }
}
