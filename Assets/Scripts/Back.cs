
using UnityEngine;

public class Back : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject leaderboard;

    public void OnBack()
    {
        characterSelectScreen.SetActive(false);
        mainMenu.SetActive(true);
        leaderboard.SetActive(false);
    }
}
