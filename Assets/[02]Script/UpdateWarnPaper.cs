using UnityEngine;

public class UpdateWarnPaper : MonoBehaviour
{
    public GameObject WarnPaper1;
    public GameObject WarnPaper2;
    public GameObject WarnPaper3;

    public void UpdateUiPaperwarn(int i) { 
        if(i == 1) {
            WarnPaper1.SetActive(true);
            WarnPaper2.SetActive(false);
            WarnPaper3.SetActive(false);
        }
        if(i == 2) {
            WarnPaper1.SetActive(true);
            WarnPaper2.SetActive(true);
            WarnPaper3.SetActive(false);
        }
        if(i == 3) {
            WarnPaper1.SetActive(true);
            WarnPaper2.SetActive(true);
            WarnPaper3.SetActive(true);
        }
    }



}
