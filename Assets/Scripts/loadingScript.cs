using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("loading");
    }

    IEnumerator loading()
    {
            yield return new WaitForSeconds(11f);
            SceneManager.LoadScene(2);
    }
}
