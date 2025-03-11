using UnityEngine;
using TMPro;
using System;

public class WindowGameResources : MonoBehaviour
{

    [SerializeField] private TMP_Text textField;

    private void Awake()
    {
        GameResources.onResourceAmountChanged += delegate (object sender, EventArgs e)
        {
            UpdateTextObject();
        };
    }

    private void UpdateTextObject()
    {
        textField.text = 
            $"Wood: {GameResources.GetResourceAmount(ResourceType.Wood)}\n" +
            $"Stone: {GameResources.GetResourceAmount(ResourceType.Stone)}";
    }
}
