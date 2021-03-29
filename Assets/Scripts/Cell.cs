using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    private string colorString = "#E86993";

    public void SetText(int num)
    {
        text.text = num.ToString();
        gameObject.SetActive(num != 0);
        // TODO: 色が濃くなるロジックを考える
        Color color = default(Color);
        ColorUtility.TryParseHtmlString(colorString, out color);
        image.color = color;



    }


}
