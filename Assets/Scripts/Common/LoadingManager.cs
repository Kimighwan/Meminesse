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

        //LogoAnim.gameObject.SetActive(false);
        Title.SetActive(true);

        //_asyncOperation = SceneManager.LoadSceneAsync(1);
        _asyncOperation = Manager.Scene.LoadSceneAsync(SceneType.GameScene);

        // 로딩이 안 되는 비정상 행동 예외 처리
        if (_asyncOperation == null)
        {
            yield break;
        }

        // 모든 로딩이 완료되어도 씬을 넘기지 않도록 설정
        // 이유 : 로딩 게이지가 차는 것을 보여주기 위함
        // 빠르게 로딩이 된다면 게이지 차는 쌈뽕한 장면을 못 보여줄 수 있어서 처리함
        _asyncOperation.allowSceneActivation = false;

        LoadingSlider.value = 0.5f;
        LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";
        yield return new WaitForSeconds(0.5f);

        while (!_asyncOperation.isDone) // 로딩이 진행 중일 때
        {
            LoadingSlider.value = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
            LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";

            // 로딩이 완료된다면 로비로 씬전환 후 코루틴 종료
            if (_asyncOperation.progress >= 0.9f)
            {
                // 90% 이상 로딩이 되었다면 이후 로딩이 완료되었을 때 씬을 넘기게 만든다.
                _asyncOperation.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }
    }
}
