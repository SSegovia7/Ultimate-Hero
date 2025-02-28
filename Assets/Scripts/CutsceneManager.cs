using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] images;
    [SerializeField] private float delayBetweenImages = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RunCutscene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RunCutscene()
    {
        foreach (Sprite image in images)
        {
            backgroundImage.sprite = image;
            yield return new WaitForSeconds(delayBetweenImages);
        }
        SceneManager.LoadScene(2); // load into gameplay scene
        yield return null;
    }
}
