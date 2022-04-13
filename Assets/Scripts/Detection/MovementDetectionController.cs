using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BodyTransform
{
    public Vector3 HeadPosition;
    public Vector3 LeftArmPosition;
    public Vector3 RightArmPosition;

    public BodyTransform(Vector3 headPos, Vector3 leftArmPos, Vector3 rightArmPos)
    {
        HeadPosition = headPos;
        LeftArmPosition = leftArmPos;
        RightArmPosition = rightArmPos;
    }

    public Vector3 this[int index]
    {
        get 
        {
            switch (index)
            {
                case 0:
                    return HeadPosition;
                case 1:
                    return LeftArmPosition;
                default:
                    return RightArmPosition;
            }
        }
    }

    public int Count
    {
        get { return 3; }
    }
}



public class MovementDetectionController : MonoBehaviour
{

    [SerializeField]
    float movementDistancePrecision = 1;
    [SerializeField]
    float movementTimePrecision = 0.5f;


    float timeStep = 0.5f; // Le temps entre chaque étape du mouvement
    List<BodyTransform> movementSteps = new List<BodyTransform>();


    BodyTransform playerTransform;
    bool movementOngoing = false;
    int indexStep = 0;
    float time = 0;
    float targetTime = 0f;



    public void InitializeMovement(List<BodyTransform> steps, float intervalStep)
    {
        timeStep = intervalStep;
        movementSteps = steps;
        time = 0f;
    }

    public void StartMovement()
    {
        movementOngoing = true;
        indexStep = 0;

        targetTime = timeStep;
    }

    // Update is called once per frame
    void Update()
    {
        if(movementOngoing)
        {
            time += Time.deltaTime;

            // On check si la position est bonne
            if(DetectStep(movementDistancePrecision, movementSteps[indexStep], playerTransform))
            {
                // Si on est dans les temps
                if(targetTime - movementTimePrecision < time && time < targetTime + movementTimePrecision)
                {
                    // 2tape suivante
                    NextStep();
                }
                else
                {
                    // Trop rapide, (arrêter le mouvement ?)
                }
            }
        }
    }

    private void NextStep()
    {
        indexStep += 1;
        targetTime = time + timeStep;

        if(indexStep >= movementSteps.Count)
        {
            EndMovement();
        }
    }

    public void EndMovement()
    {
        movementOngoing = false;
    }





    public bool DetectStep(float distancePrecision, BodyTransform stepPosition, BodyTransform playerTransform)
    {
        for (int i = 0; i < stepPosition.Count; i++)
        {
            if (!CheckDetection(distancePrecision, stepPosition[i], playerTransform[i]))
                return false;
        }
        return true;
    }

    private bool CheckDetection(float precision, Vector3 position, Vector3 playerPos)
    {
        float distance = (position - playerPos).magnitude;
        return (distance < precision);
    }
}
