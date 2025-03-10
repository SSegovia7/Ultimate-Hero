using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Sprite[] images;
    [SerializeField] private float delayBetweenImages = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Play Cutscene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishCutscene()
    {
        SceneManager.LoadScene(2);
    }

}
