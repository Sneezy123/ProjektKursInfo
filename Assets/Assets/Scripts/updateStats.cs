using TMPro;
using UnityEngine;

public class updateStats : MonoBehaviour
{
    
    public void updateCompletedPuzzles()
    {
        int completedPuzzlesCount = runtimeManager.alreadyCompletedPuzzlesList.Count;
        string completedPuzzles = string.Format("{0} / 15", completedPuzzlesCount);
        TextMeshProUGUI text = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        text.SetText(completedPuzzles);
    }

    void Update()
    {
        updateCompletedPuzzles();
    }
}