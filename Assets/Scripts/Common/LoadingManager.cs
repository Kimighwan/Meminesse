using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    // 로고
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    public GameObject Title;
    public Slider LoadingSlider;                // Loadingbar - LoadingProgressBar
    public TextMeshProUGUI LoadingProgressTxt; 

    private AsyncOperation _asyncOperation;     // 비동기적으로 로딩 중인 씬

    private void Awake()
    {
        LogoAnim.gameObject.SetActive(true);
        Title.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LoadGameCo());
    }

    private IEnumerator LoadGameCo()
    {
        LogoAnim.Play();
        yield return new WaitForSeconds(LogoAnim.clip.length);

        Title.SetActive(true);

        _asyncOperation = SceneLoader.Instance.LoadSceneAsync(SceneType.GameScene);

        if (_asyncOperation == null)
        {
            yield break;
        }

        _asyncOperation.allowSceneActivation = false;

        LoadingSlider.value = 0.5f;
        LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";
        yield return new WaitForSeconds(0.5f);

        while (!_asyncOperation.isDone)
        {
            LoadingSlider.value = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
            LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";

            if (_asyncOperation.progress >= 0.9f)
            {
                _asyncOperation.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }
    }
}
