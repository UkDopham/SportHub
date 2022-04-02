using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Exercice")]
public class Exercice : ScriptableObject
{
    [SerializeField]
    private string _name;
    [SerializeField]
    [Range(1,5)]
    private int _difficulty = 1;
    [SerializeField]
    private List<ExercieType> _types = new List<ExercieType>();
    [SerializeField]
    [TextArea]
    private string _description;

    public string Name
    {
        get
        {
            return this._name;
        }
    }
    public int Difficulty
    {
        get
        {
            return this._difficulty;
        }
    }
    public List<ExercieType> Types
    {
        get
        {
            return this._types;
        }
    }
    public string Description
    {
        get
        {
            return this._description;
        }
    }
}

