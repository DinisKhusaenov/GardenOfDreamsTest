using UnityEngine;

public class DefeatPanel : MonoBehaviour
{
    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
