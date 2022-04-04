using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ExerciceHelper : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        this._text = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "Text");
        this._text.text = GlobalManager.currentExercice != null ? $"Current Exercice {GlobalManager.currentExercice.Name}" : "Not found";
    }
}
