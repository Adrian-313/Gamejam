using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        GameManager.Instance.SelectScene(0);
    }
}
