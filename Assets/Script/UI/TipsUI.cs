using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TipsUI : MonoBehaviour
{
    public GameObject interactTips;
    public void OpenInteractTips() => interactTips.SetActive(true);
    public void CloseInteractTips() => interactTips.SetActive(false);

    public GameObject getTips;
    public void OpenGetTips(string txt)
    {
        getTips.GetComponent<Text>().text = txt;
        getTips.SetActive(true);
        StartCoroutine(CloseGetTips());
    }
    private IEnumerator CloseGetTips()
    {
        yield return new WaitForSeconds(5);
        getTips.GetComponent<Text>().text = "";
        getTips.SetActive(false);
    }
}
