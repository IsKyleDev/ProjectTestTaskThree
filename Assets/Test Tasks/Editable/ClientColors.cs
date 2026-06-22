using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Editable
{
    public class ClientColors : MonoBehaviour
    {
        [SerializeField] private GameObject colorPrefab;
        [SerializeField] private RectTransform parentRect;
        [SerializeField] private Button btnRequestColor;
        [SerializeField] private List<Color> listOfColors;

        private void Start()
        {
            btnRequestColor.onClick.AddListener(HandleOnBtnRequestColor);
        }

        private void HandleOnBtnRequestColor()
        {
            ClientPacketsHandler.SendColorRequest();

        }

        private void ClearColors()
        {
            foreach (Transform child in parentRect)
            {
                Destroy(child.gameObject);
            }
        }

        public void SetColors(List<Color> colors)
        {
            listOfColors = colors;

            ClearColors();

            foreach (Color color in colors)
            {
                GameObject colorObject = Instantiate(colorPrefab, parentRect);
                colorObject.SetActive(true);
                Image image = colorObject.GetComponent<Image>();
                image.color = color;
                image.enabled = true;
            }
        }
    }
}
