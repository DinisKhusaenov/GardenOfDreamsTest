using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Inventory _inventory;

    public bool IsInventoryActive { get; private set; }

    private void Awake()
    {
        IsInventoryActive = true;

        OnInventoryClicked();
    }

    private void OnEnable()
    {
        _inventoryButton.onClick.AddListener(OnInventoryClicked);
    }

    private void OnDisable()
    {
        _inventoryButton.onClick.RemoveListener(OnInventoryClicked);
    }

    private void OnInventoryClicked()
    {
        if (IsInventoryActive == false)
        {
            _inventory.gameObject.SetActive(true);
            IsInventoryActive = true;
        }
        else
        {
            _inventory.gameObject.SetActive(false);
            IsInventoryActive = false;
        }

    }
}
