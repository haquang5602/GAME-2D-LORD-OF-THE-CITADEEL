using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarTimer : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    private float duration = 5f;

    public void SetDuration(float time)
    {
        duration = time;
    }

    public void StartProgress()
    {
        if (progressImage != null)
        {
            StartCoroutine(UpdateProgress());
        }
    }

    private IEnumerator UpdateProgress()
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            progressImage.fillAmount = 1f - (time / duration);
            yield return null;
        }

        progressImage.fillAmount = 0f;
    }
}
