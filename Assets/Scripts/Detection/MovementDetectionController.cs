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

    [Header("Debug")]
    [SerializeField]
    MovementDetectionData debugExercice = null;

    [Header("Parameters")]
    [SerializeField]
    float movementDistancePrecision = 1;
    [SerializeField]
    float movementTimePrecision = 0.5f;

    [SerializeField]
    float timeStartup = 3f;


    [Header("Player Tracking")]
    [SerializeField]
    Transform playerHead = null;
    [SerializeField]
    Transform playerLeftArm = null;
    [SerializeField]
    Transform playerRightArm = null;


    [Header("Movement Indicator")]
    [SerializeField]
    Transform helperLeftArm = null;
    [SerializeField]
    Transform helperRightArm = null;


    MovementDetectionData movementData;
    float timeStep = 0.5f; // Le temps entre chaque étape du mouvement
    List<BodyTransform> movementSteps = new List<BodyTransform>();


    BodyTransform playerTransform;


    bool movementOngoing = false;
    int indexStep = 0;
    float time = 0;
    float targetTime = 0f;

    float startTimer = 0f;

    private void Start()
    {
        playerTransform = new BodyTransform();
        // movementData = GlobalManager.currentExercice.

#if UNITY_EDITOR
        movementData = debugExercice;
#endif
        InitializeMovement(movementData.Movements, movementData.TimeInterval);
    }

    public void InitializeMovement(List<BodyTransform> steps, float intervalStep)
    {
        timeStep = intervalStep;
        movementSteps = steps;
        time = 0f;

        DrawMovementHelper(steps[0], steps[0], 0);
    }

    public void StartMovement()
    {
        movementOngoing = true;
        indexStep = 0;

        startTimer = 0;
        targetTime = timeStep;

        Debug.Log("Movement START");
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform.HeadPosition = playerHead.position;
        playerTransform.LeftArmPosition = playerLeftArm.position;
        playerTransform.RightArmPosition = playerRightArm.position;

        if (movementOngoing)
        {
            time += Time.deltaTime;
            DrawMovementHelper(GetPreviousStepPosition(indexStep), movementSteps[indexStep], (time - startTimer) / (targetTime - startTimer));

            // On check si la position est bonne
            if (DetectStep(movementDistancePrecision, movementSteps[indexStep], playerTransform))
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
                    //EndMovement();
                }
            }
            else if (time > targetTime + movementTimePrecision)
            {
                // Trop lent, (arrêter le mouvement ?)
                //EndMovement();
            }
        }
        else
        {
            // Une fois que le joueur est dans la bonne position de départ, on lance l'exo
            if (DetectStep(movementDistancePrecision, movementSteps[0], playerTransform))
            {
                startTimer += Time.deltaTime;
                if(startTimer > timeStartup)
                {
                    StartMovement();
                }
            }
        }
    }

    private void NextStep()
    {
        indexStep += 1;
        startTimer = time;
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


    private void DrawMovementHelper(BodyTransform stepPosition, BodyTransform stepNextPosition, float t)
    {
        helperLeftArm.position = Vector3.Lerp(stepPosition.LeftArmPosition, stepNextPosition.LeftArmPosition, t);
        helperRightArm.position = Vector3.Lerp(stepPosition.RightArmPosition, stepNextPosition.RightArmPosition, t);
    }

    private BodyTransform GetPreviousStepPosition(int index)
    {
        if (index == 0)
            return movementSteps[0];
        return movementSteps[index - 1];
    }


    [ContextMenu("Debug")]
    private void AutoSetDebug()
    {
        playerRightArm.position = movementSteps[indexStep].RightArmPosition;
        playerLeftArm.position = movementSteps[indexStep].LeftArmPosition;
        playerHead.position = movementSteps[indexStep].HeadPosition;
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
