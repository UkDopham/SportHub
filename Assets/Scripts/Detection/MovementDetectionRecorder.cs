using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MovementDetectionRecorder : MonoBehaviour
{
    [SerializeField]
    MovementDetectionData movementData;
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


    private void Start()
    {
        if (movementData.isLock)
            this.enabled = false;
    }

    public void StartRecording()
    {
        recording = true;
        Debug.Log("Allo");
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

            if (Input.GetButtonDown("Jump"))
            {
                EndRecording();
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartRecording();
            }
        }
    }


    public void EndRecording()
    {
        recording = false;
        movementData.TimeInterval = timeStepRecord;
        movementData.Movements = movementSteps;
        Debug.Log("Allo");
    }
}
