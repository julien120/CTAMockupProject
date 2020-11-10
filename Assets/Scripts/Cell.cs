using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Text text;
    
    public void SetText(int num)
    {
        text.text = num.ToString();
        // TODO: 色が濃くなるロジックはもう少し考えた方がいいかも
        image.color = Color.HSVToRGB( 25 / 255.0f, num * 2 / 255.0f, 100 / 255.0f);
    }
}
