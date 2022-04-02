using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : Button
{
    [SerializeField]
    private string _sceneName;
    protected override void OnClick()
    {
        SceneManager.LoadScene(this._sceneName);
    }
}
