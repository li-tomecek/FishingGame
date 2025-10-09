using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObject;
    [SerializeField] TextMeshProUGUI _resultsTxt, _fishCaughtTxt, _scoreTxt;
    [SerializeField] Button _mainMenuBtn;
    [SerializeField] Vector3 _entryTranslation = new Vector3(-15, 0, 0);
    Vector3 _resultsStart, _fishStart, _scoreStart;

    [SerializeField] float _menuDelayTime = 24f;

    void Start()
    {
        _mainMenuBtn.gameObject.SetActive(false);

        _resultsStart = _resultsTxt.transform.position;
        _resultsTxt.transform.Translate(-_entryTranslation);

        _fishStart = _fishCaughtTxt.transform.position;
        _fishCaughtTxt.transform.Translate(-_entryTranslation);

        _scoreStart = _scoreTxt.transform.position;
        _scoreTxt.transform.Translate(-_entryTranslation);

        _menuObject.SetActive(false);
    }

    public void Update()
    {
        _menuDelayTime -= Time.deltaTime;
        if (!_menuObject.activeInHierarchy && _menuDelayTime <= 0)
        {
            StartResultsSequence();
        }
    }

    public void StartResultsSequence()
    {
        _menuObject.SetActive(true);

        Sequence seq = DOTween.Sequence().Pause();

        seq.Append(_resultsTxt.transform.DOMove(_resultsStart, 0.5f));
        seq.AppendInterval(0.1f);
        seq.Append(_fishCaughtTxt.transform.DOMove(_fishStart, 0.5f));
        seq.AppendInterval(0.1f);
        seq.Append(_scoreTxt.transform.DOMove(_scoreStart, 0.5f));
        seq.AppendInterval(0.1f);

        seq.OnComplete(() => _mainMenuBtn.gameObject.SetActive(true));

        seq.Play();
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}
