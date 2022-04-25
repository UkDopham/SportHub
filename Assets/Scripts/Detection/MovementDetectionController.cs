using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    TMPro.TextMeshPro textFeedback = null;
    [SerializeField]
    TMPro.TextMeshPro textSeries = null;
    [SerializeField]
    Transform helperLeftArm = null;
    [SerializeField]
    Transform helperRightArm = null;

    [Header("Feedbacks")]
    [SerializeField]
    private TextMeshProUGUI _message;
    [SerializeField]
    private CanvasGroup _back;
    [SerializeField]
    private Image _backImage;
    [SerializeField]
    private AudioSource _audioSourceSound;
    [SerializeField]
    private AudioSource _audioSourceMusic;
    [SerializeField]
    private AudioClip _key;
    [SerializeField]
    private AudioClip _coin;
    [SerializeField]
    private List<CanvasGroup> _stars = new List<CanvasGroup>();
    [SerializeField]
    private CanvasGroup _messageCG;


    MovementDetectionData movementData;
    float timeStep = 0.5f; // Le temps entre chaque étape du mouvement
    List<BodyTransform> movementSteps = new List<BodyTransform>();
    int[] exerciceScore = new int[3];


    BodyTransform playerTransform;


    bool movementOngoing = false;
    int indexStep = 0;
    int repetition = 0;
    float time = 0;
    float targetTime = 0f;

    float startTimer = 0f;
    bool cooldownText = true;

    private void Start()
    {
        playerTransform = new BodyTransform();
        // movementData = GlobalManager.currentExercice.

#if UNITY_EDITOR
        movementData = debugExercice;
#endif
        InitializeMovement(movementData.Movements, movementData.TimeInterval);
        StartMovement();
        //Finished("Bonjour la france", Color.red, 5);
    }

    public void InitializeMovement(List<BodyTransform> steps, float intervalStep)
    {
        timeStep = intervalStep;
        movementSteps = steps;
        time = 0f;

        DrawMovementHelper(steps[0], steps[0], 0);
        textFeedback.text = "Maintenez la position pour commencer.";
    }

    public void StartMovement()
    {
        movementOngoing = true;
        indexStep = 0;

        startTimer = 0;
        targetTime = timeStep;

        exerciceScore = new int[3];
        for (int i = 0; i < exerciceScore.Length; i++)
        {
            exerciceScore[i] = 0;
        }

        textFeedback.text = "C'est parti !";
        textSeries.text = repetition + " / " + movementData.Repetition;
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

            if (time >= targetTime)
            {
                // 2tape suivante
                NextStep();
            }
            // On check si la position est bonne
            /*if (DetectStep(movementDistancePrecision, movementSteps[indexStep], playerTransform))
            {
                // Si on est dans les temps
                if (time >= targetTime)
                {
                    // 2tape suivante
                    NextStep();
                    DrawText("Parfait");
                }
                else if (time <= targetTime - movementTimePrecision)
                {
                    // Trop rapide, (arrêter le mouvement ?)
                    //EndMovement();
                    //DrawText("Trop rapide"); cooldownText = true;
                }
            }
            else if (time > targetTime)
            {
                // Trop lent, (arrêter le mouvement ?)
                //EndMovement();
                NextStep(); 
                DrawText("Trop lent");
            }*/
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
                else if (startTimer > 2) // C'est dégueulasse mais c'est pas grave
                {
                    textFeedback.text = "1";
                }
                else if (startTimer > 1)
                {
                    textFeedback.text = "2";
                }
                else if (startTimer > 0)
                {
                    textFeedback.text = "3";
                }
            }
        }
    }

    /*private void DrawText(string text)
    {
        if (cooldownText)
            return;
        textFeedback.text = text;
    }*/

    private void NextStep()
    {
        int result = AddScore(movementDistancePrecision, movementSteps[indexStep], playerTransform);
        if (result == 0)
            textFeedback.text = "Trop lent";
        else if (result == 1)
            textFeedback.text = "Approximatif";
        else if (result == 2)
            textFeedback.text = "Bien";
        else if (result == 3)
            textFeedback.text = "Parfait";

        indexStep += 1;
        startTimer = time;
        targetTime = time + timeStep;
        cooldownText = false;

        if (indexStep >= movementSteps.Count)
        {
            EndMovement();
        }
    }

    public void EndMovement()
    {
        movementOngoing = false;
        repetition += 1;

        textSeries.text = repetition + " / " + movementData.Repetition;

        if (repetition >= movementData.Repetition)
        {
            float percentage = movementData.Movements.Count * movementData.Repetition;

            Finished("Précision de la tête : " + (((float) exerciceScore[0] / percentage) * 100) + " %\n" +
                "Précision du bras gauche : " + (((float)exerciceScore[1] / percentage) * 100) + " %\n" +
                "Précision du bras droit : " + (((float)exerciceScore[2] / percentage) * 100) + " %\n",
                Color.yellow, 10);
        }
        else
        {
            StartMovement();
        }
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

    private int AddScore(float distancePrecision, BodyTransform stepPosition, BodyTransform playerTransform)
    {
        int score = 0;
        for (int i = 0; i<stepPosition.Count; i++)
        {
            if (CheckDetection(distancePrecision, stepPosition[i], playerTransform[i]))
            {
                score += 1;
                exerciceScore[i] += 1;
            }
        }
        return score;
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

    /// <summary>
    /// Finishing an exercice 
    /// </summary>
    /// <param name="message">Message to show</param>
    /// <param name="color">Color of the background</param>
    /// <param name="delay">Delay before switching to the menu scene</param>
    private void Finished(string message, Color color, float delay)
    {
        this._stars.ForEach(x => x.alpha = 0);
        this._messageCG.alpha = 1;
        this._audioSourceMusic.Stop();
        this._audioSourceSound.PlayOneShot(this._coin);
        this._back.alpha = 0.2f;
        this._backImage.color = color;

        StartCoroutine(LoadScene(delay, message));
    }


    /// <summary>
    /// Loading a scene with a delay
    /// </summary>
    /// <param name="delay">Delay before loading the new scene</param>
    /// <returns></returns>
    private IEnumerator LoadScene(float delay, string message)
    {
        this._message.text = string.Empty;
        for (int i = 0; 
            i < message.Length; 
            i ++)
        {
            this._message.text += message[i];
            this._audioSourceSound.PlayOneShot(this._key);
            yield return new WaitForSeconds(.15f);
        }


        if(GlobalManager.currentExercice != null)
        {
            for (int i = 0;
                i < GlobalManager.currentExercice.Difficulty;
                i++)
            {
                this._stars[i].alpha = 1;
                this._audioSourceSound.PlayOneShot(this._coin);
                yield return new WaitForSeconds(.25f);
            }
        }

        yield return new WaitForSeconds(delay); 

        SceneManager.LoadScene("MenuExercice");

        yield return null;
    }
}
