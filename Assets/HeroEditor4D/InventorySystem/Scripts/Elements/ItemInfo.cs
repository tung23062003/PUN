using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.InventorySystem.Scripts.Elements
{
    /// <summary>
    /// Represents item when it was selected. Displays icon, name, price and properties.
    /// </summary>
    public class ItemInfo : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject Panel;
        public GameObject Selection;
        public GameObject Buttons;
        public Text Name;
        public Text Labels;
        public Text Values;
        public Text Price;
        public Image Icon;
        public Image Background;

        [Header("Tooltip Settings")]
        public Vector2 offset = new Vector2(15, -15);
        public RectTransform canvasRect;  // assign Canvas RectTransform trong inspector

        public Item Item { get; protected set; }

        protected static readonly List<PropertyId> Sorting = new List<PropertyId>
        {
            PropertyId.Damage,
            PropertyId.StaminaMax,
            PropertyId.Blocking,
            PropertyId.Resistance
        };

        public void OnEnable()
        {
            if (Item == null)
            {
                Reset();
            }
        }

        public void Reset()
        {
            Panel.SetActive(false);
            Selection.SetActive(false);
            Buttons.SetActive(false);

            if (Name) Name.text = null;
            if (Labels) Labels.text = null;
            if (Values) Values.text = null;
            if (Price) Price.text = null;
        }

        public virtual void Initialize(Item item, int price, RectTransform itemRect, bool trader = false)
        {
            Item = item;

            if (item == null)
            {
                Reset();
                return;
            }

            Selection.SetActive(true);
            Buttons.SetActive(true);

            Name.text = item.Params.GetLocalizedName(Application.systemLanguage.ToString());
            Icon.sprite = ItemCollection.Active.FindIcon(item.Params.IconId);
            Background.sprite = ItemCollection.Active.GetBackground(item);

            UpdatePrice(item, price, trader);

            var main = new List<object> { item.Params.Type };

            if (item.Params.Class != ItemClass.Undefined) main.Add(item.Params.Class);

            foreach (var t in item.Params.Tags)
            {
                main.Add(t);
            }

            var dict = new Dictionary<string, object> { { "Type", string.Join(" / ", main) } };

            if (item.Params.Level >= 0) dict.Add("Level", item.Params.Level);

            if (item.Modifier != null)
            {
                dict.Add("Modifier", $"{item.Modifier.Id} [{item.Modifier.Level}]");
            }

            var props = item.Params.Properties.ToList()
                .OrderBy(i => { var index = Sorting.IndexOf(i.Id); return index == -1 ? 999 : index; }).ToList();

            foreach (var p in props)
            {
                switch (p.Id)
                {
                    case PropertyId.Damage:
                        dict.Add($"{p.Id}", $"{p.Min}-{p.Max}");
                        break;
                    case PropertyId.CriticalChance:
                    case PropertyId.CriticalDamage:
                        dict.Add($"{p.Id}", $"+{p.Value}%");
                        break;
                    case PropertyId.ChargeTimings:
                        dict.Add($"{p.Id}", $"{p.Value.Split(',').Length}");
                        break;
                    default:
                        dict.Add($"{p.Id}", $"{p.Value}");
                        break;
                }
            }

            dict.Add("Weight", $"{item.Params.Weight / 10f:0.##} kg");

            if (Price && item.Params.Type != ItemType.Currency)
            {
                dict.Add("Price", $"{item.Params.Price} gold");
            }

            Labels.text = string.Join("\n", dict.Keys);
            Values.text = string.Join("\n", dict.Values);

            Panel.SetActive(true);

            // 🔹 Điều chỉnh vị trí Panel nếu có itemRect (nút item được click)
            if (itemRect != null)
            {
                AdjustPanelPosition(itemRect);
            }
        }

        private void AdjustPanelPosition(RectTransform itemRect)
        {
            var panelRect = GetComponent<RectTransform>();

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, itemRect.position);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPoint);

            Vector2 anchoredPos = localPoint + offset;
            panelRect.anchoredPosition = anchoredPos;

            Vector3[] panelCorners = new Vector3[4];
            Vector3[] canvasCorners = new Vector3[4];
            panelRect.GetWorldCorners(panelCorners);
            canvasRect.GetWorldCorners(canvasCorners);

            Vector2 adjustment = Vector2.zero;

            if (panelCorners[2].x > canvasCorners[2].x)
                adjustment.x -= panelCorners[2].x - canvasCorners[2].x;

            if (panelCorners[0].x < canvasCorners[0].x)
                adjustment.x += canvasCorners[0].x - panelCorners[0].x;

            if (panelCorners[2].y > canvasCorners[2].y)
                adjustment.y -= panelCorners[2].y - canvasCorners[2].y;

            if (panelCorners[0].y < canvasCorners[0].y)
                adjustment.y += canvasCorners[0].y - panelCorners[0].y;

            panelRect.anchoredPosition += adjustment;
        }

        public virtual void UpdatePrice(Item item, int price, bool trader)
        {
            if (!Price) return;

            if (item.Params.Type == ItemType.Currency)
            {
                Price.text = null;
            }
            else
            {
                Price.text = trader ? $"Buy price: {price}G" : $"Sell price: {price}G";
            }
        }
    }
}
