using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour
{
   private VisualElement[] hearts;

   [SerializeField] private Sprite emptyHeartSprite;
   [SerializeField] private Sprite filledHeartSprite;

   private const string containerName = "Container";
   
   public void Init(int maxHeartCount)
   {
      VisualElement container = GetComponent<UIDocument>().rootVisualElement.Q(containerName);
      for (int i = 0; i < maxHeartCount; i++)
      {
         container.Add(containerHeartElement(maxHeartCount));
      }
   }

   private VisualElement containerHeartElement(int heartCount)
   {
      VisualElement heart = new VisualElement
      {
         style =
         {
            width = Length.Percent(100/heartCount),
            marginBottom = 1,
            marginLeft = 1,
            marginRight = 1,
            marginTop = 1,
            backgroundImage = new StyleBackground(filledHeartSprite)
         }
      };
      return heart;
   }
}
