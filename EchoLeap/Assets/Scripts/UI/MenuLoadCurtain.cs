using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuLoadCurtain : MonoBehaviour
{
    [SerializeField] private Image curtain;
    [SerializeField] private int hideDelay;

    public void SwichPanel(GameObject showPanel)
    {
        StartCoroutine(StartSwichPanel(showPanel));
    }

    public void HidePanelWithDelay(GameObject hidePanel)
    {
        StartCoroutine(StartHidePanelWithDelay(hidePanel, hideDelay));
    }

    private IEnumerator StartSwichPanel(GameObject showPanel)
    {
        Color color = new Color(0, 0, 0, 0);
        curtain.gameObject.SetActive(true);

        bool readyShowing = false;

        while(!readyShowing)
        {
            color.a += 0.01f;
            curtain.color = color;

            if (color.a >= 1)
            {
                showPanel.SetActive(true);

                readyShowing = true;
            }

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1f);

        while (true)
        {
            color.a -= 0.01f;
            curtain.color = color;

            if (color.a <= 0f)
            {
                curtain.gameObject.SetActive(false);

                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator StartHidePanelWithDelay(GameObject hidePanel, int sec)
    {
        yield return new WaitForSeconds(sec);

        hidePanel.SetActive(false);
    }
}
