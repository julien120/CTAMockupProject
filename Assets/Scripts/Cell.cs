using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Text text;

    public void SetText(int num)
    {
        text.text = num.ToString();
    }
}
