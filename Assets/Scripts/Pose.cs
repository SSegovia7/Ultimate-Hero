using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose : MonoBehaviour
{
    [SerializeField] int currentPose = 0;
    [SerializeField] int maxPose = 100;

    public HealthBar poseBar; 

    // Start is called before the first frame update
    void Start()
    {
        poseBar.SetMaxHealth(maxPose);
        poseBar.SetHealth(currentPose);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreasePose(20);   //MANUALLY INCREASES POSE BY 20
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreasePose(-20); //MANUALLY DECREASES POSE BY 20
        }
        if(currentPose > 100)
        {
            currentPose = 100;
        }
    }

    public void IncreasePose(int value){ //INCREASES POSE IN POSE BAR
        Debug.Log($"Pose Meter increasing by {value}");
        currentPose += value;
        poseBar.SetHealth(currentPose);
    }

    public int GetPoseValue(){
        return currentPose;
    }
}
