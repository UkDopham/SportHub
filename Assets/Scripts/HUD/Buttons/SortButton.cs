using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SortButton : Button
{
    [SerializeField]
    private ExercieType _type;
    private List<VideoButton> _exercices = new List<VideoButton>();
    protected override void OnClick()
    {
        Sort();
    }
    protected override void Initialization()
    {
        base.Initialization();
        this._text.text = this._type.ToString();
        this._exercices = FindObjectsOfType<VideoButton>().Where(x => x.name.Contains("Video button")).ToList();
    }
    private void Sort()
    {
        foreach(VideoButton exercice in this._exercices)
        {
            exercice.gameObject.SetActive(exercice.Exercice.Types.Contains(this._type) || this._type == ExercieType.all);
        }
    }
}
