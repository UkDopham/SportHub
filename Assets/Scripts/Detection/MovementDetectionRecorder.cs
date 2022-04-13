using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MovementDetectionRecorder : MonoBehaviour
{
    [SerializeField]
    float timeStepRecord = 0.5f;

    [SerializeField]
    Transform headPosition;
    [SerializeField]
    Transform leftArmPosition;
    [SerializeField]
    Transform rightArmPosition;

    float time = 0;
    bool recording = false;
    List<BodyTransform> movementSteps = new List<BodyTransform>();


    public void StartRecording()
    {
        recording = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(recording)
        {
            time += Time.deltaTime;
            if(time >= timeStepRecord)
            {
                movementSteps.Add(new BodyTransform(headPosition.position, leftArmPosition.position, rightArmPosition.position));
                time = 0;
            }
        }
    }


    public void EndRecording()
    {
        recording = false;
    }
}
