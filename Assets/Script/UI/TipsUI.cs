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
    
    public GameObject commonTips;
    public void OpenCommonTips(string txt)
    {
        commonTips.GetComponent<Text>().text = txt;
        commonTips.SetActive(true);
        StartCoroutine(CloseCommonTips());
    }
    private IEnumerator CloseCommonTips()
    {
        yield return new WaitForSeconds(1);
        commonTips.GetComponent<Text>().text = "";
        commonTips.SetActive(false);
    }
}
