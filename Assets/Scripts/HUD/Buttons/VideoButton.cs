using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoButton : Button
{
    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private float _scaleVideoEnter = 2f;
    [SerializeField]
    private CanvasGroup _blur;
    [SerializeField]
    private Exercice _exercice;

    private List<CanvasGroup> _stars = new List<CanvasGroup>();  
    private RawImage _video;
    private Image _back;
    private VideoPlayer _videoPlayer;
    private Vector2 _baseScale;
    private Vector2 _baseScaleBack;
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private CanvasGroup _metrics;
    private TextMeshProUGUI _difficultyName;
    private CanvasGroup _canvas;

    public CanvasGroup Canvas
    {
        set
        {
            this._canvas = value;
        }
        get
        {
            return this._canvas;
        }
    }
    public Exercice Exercice
    {
        get
        {
            return this._exercice;
        }
    }
    protected override void Initialization()
    {
        base.Initialization();
        this._video = GetComponentInChildren<RawImage>();
        this._videoPlayer = GetComponentInChildren<VideoPlayer>();
        this._back = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Back");
        this._baseScale = this._video.rectTransform.sizeDelta;
        this._baseScaleBack = this._back.rectTransform.sizeDelta;
        this._canvas = GetComponents<CanvasGroup>().FirstOrDefault(x => x.name.Contains("Video button"));
        this._metrics = GetComponentsInChildren<CanvasGroup>().FirstOrDefault(x => x.name == "Metrics");
        this._title = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "Title");
        this._description = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "Description");
        this._stars = GetComponentsInChildren<CanvasGroup>().Where(x => x.name.Contains("Star")).ToList();
        this._difficultyName = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "Difficulty");

        SetDifficulty();
        this._title.text = this._exercice.Name;
        this._description.text = this._exercice.Description;
        this._metrics.alpha = 0f;
    }
    private void SetDifficulty()
    {
        switch(this._exercice.Difficulty)
        {
            case 1:
                this._difficultyName.text = "Beginner";
                break;
            case 2:
                this._difficultyName.text = "Novice";
                break;
            case 3:
                this._difficultyName.text = "Average";
                break;
            case 4:
                this._difficultyName.text = "Elite";
                break;
            case 5:
                this._difficultyName.text = "Pro";
                break;
        }
        this._stars.ForEach(x => x.alpha = 0);
        for (int i = 0;
            i < this._exercice.Difficulty;
            i++)
        {
            this._stars[i].alpha = 1;
        }
    }
    protected override void OnPointClickVirtual()
    {
        //base.OnPointClickVirtual();
        GlobalManager.currentExercice = this._exercice;
        SceneManager.LoadScene(this._sceneName);
    }
    protected override void OnPointerEnterVirtual()
    {
        Scale(this._scaleVideoEnter);
        this._videoPlayer.Play();
        this._blur.alpha = 0.8f;
        this._metrics.alpha = 1f;
    }
    protected override void OnPointerExitVirtual()
    {
        this._video.rectTransform.sizeDelta = this._baseScale;
        this._back.rectTransform.sizeDelta = this._baseScaleBack;
        this._videoPlayer.Stop();
        this._blur.alpha = 0f;
        this._metrics.alpha = 0f;
    }

    private void Scale(float scale)
    {
        this._video.rectTransform.sizeDelta = this._video.rectTransform.sizeDelta * scale;
        this._back.rectTransform.sizeDelta = this._back.rectTransform.sizeDelta * scale;
    }
}
