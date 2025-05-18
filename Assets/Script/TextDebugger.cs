using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TextDebugger : MonoBehaviour
{
    void Start()
    {
        List<Text> texts = new List<Text>();
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        foreach (GameObject rootObj in rootObjects)
        {
            Text[] foundTexts = rootObj.GetComponentsInChildren<Text>(true); // 非アクティブも含む
            texts.AddRange(foundTexts);
        }

        foreach (Text t in texts)
        {
            Debug.Log($"[TextDebugger] {t.gameObject.name}: \"{t.text}\"", t.gameObject);
        }
    }
}
