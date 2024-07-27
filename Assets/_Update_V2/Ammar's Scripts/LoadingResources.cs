using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingResources : MonoBehaviour
{
    public Image loadingBar;
    public int counter;
    public TextMeshProUGUI resourceLoadingText;

    private firebaseDB firebaseDB;

    private Coroutine _Co;
    // Start is called before the first frame update
    void Start()
    {
        firebaseDB = FindObjectOfType<firebaseDB>();

        _Co = StartCoroutine(getResourcesDownload());
    }

    private IEnumerator getResourcesDownload()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if(firebaseDB.resourceCounter < 5)
            {
                loadingBar.fillAmount = (firebaseDB.resourceCounter / 5f);
                resourceLoadingText.text = ((firebaseDB.resourceCounter / 5f) * 100f).ToString("f0") + "%";
            }
            else if(firebaseDB.resourceCounter == 5)
            {
                yield return new WaitForSeconds(2f);
                gameObject.SetActive(false);
            }
        }
    }
}
