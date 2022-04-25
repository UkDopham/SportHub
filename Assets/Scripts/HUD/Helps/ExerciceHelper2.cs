using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ExerciceHelper2 : MonoBehaviour
{
    private TextMeshPro _text;
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        this._text = GetComponent<TextMeshPro>();// (x => x.name == "Text");
        this._text.text = GlobalManager.currentExercice != null ? $"Exercice : {GlobalManager.currentExercice.Name}" : "Not found";
    }
}
