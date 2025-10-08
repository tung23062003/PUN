using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using UnityEngine;
using UnityEngine.UI;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Enums;

namespace Assets.HeroEditor4D.InventorySystem.Scripts
{
    /// <summary>
    /// High-level inventory interface.
    /// </summary>
    public class InventoryBase : ItemWorkspace
    {
        public Transform TabsTrans;
        public Equipment Equipment;
        public ScrollInventory PlayerInventory;
        public ScrollInventory Materials;
        public Button EquipButton;
        public Button RemoveButton;
        public Button CraftButton;
        public Button LearnButton;
        public Button UseButton;
        public Button AssembleButton;
        public AudioClip EquipSound;
        public AudioClip CraftSound;
        public AudioClip UseSound;
        public AudioSource AudioSource;
        public bool InitializeExample;

        // These callbacks can be used outside;
        public Action<Item> OnRefresh;
        public Action<Item> OnEquip;
        public Func<Item, bool> CanEquip = i => true;

        //public List<ItemType> Tabs = new List<ItemType>()
        //{
        //    { ItemType.All},
        //    { ItemType.Helmet},
        //    { ItemType.Armor},
        //    { ItemType.Boosts},
        //    { ItemType.Weapon},
        //    { ItemType.Gloves},
        //    { ItemType.Earrings},
        //    { ItemType.Food},
        //};

        [HideInInspector] public ItemType CurrentTab;

        public void Awake()
        {
            ItemCollection.Active = ItemCollection;
            LoadInventory();

            Equipment.LoadEquipment();
        }

        public void Start()
        {
            if (InitializeExample)
            {
                TestInitialize();
            }

            OnSelectTab(true);
        }

        public void OnSelectTab(bool value)
        {
            if (!value) return;

            var tab = TabsTrans.GetComponentsInChildren<Toggle>().Single(i => i.isOn);

            ItemCollection.Active.Reset();

            ItemType itemType = ItemType.All;
            bool isAll = false;
            switch (tab.name)
            {
                case "All":
                    {
                        isAll = true;
                        break;
                    }
                case "Armor":
                    {
                        itemType = ItemType.Armor;
                        break;
                    }
                case "Helmet":
                    {
                        itemType = ItemType.Helmet;
                        break;
                    }
                case "Gloves":
                    {
                        itemType = ItemType.Gloves;
                        break;
                    }
                case "Boosts":
                    {
                        itemType = ItemType.Boosts;
                        break;
                    }
                case "Weapon":
                    {
                        itemType = ItemType.Weapon;
                        break;
                    }
                case "Earrings":
                    {
                        itemType = ItemType.Earrings;
                        break;
                    }
                case "Food":
                    {
                        itemType = ItemType.Food;
                        break;
                    }
                default:
                    throw new NotImplementedException(tab.name);
            }

            CurrentTab = itemType;

            var items = new List<ItemParams>();

            if(!isAll)
                items = ItemCollection.Active.Items.FindAll(i => i.Type == itemType).ToList();
            else
                items = ItemCollection.Active.Items.ToList();

            for (int i = items.Count - 1; i >= 0; i--)
            {
                var itemIndex = items[i];
                if (itemIndex.Id != null && Equipment.Items.Exists(item => item.Id == itemIndex.Id))
                {
                    items.RemoveAt(i);
                }
            }


            var inventory = items.Select(i => new Item(i.Id)).ToList(); // inventory.Clear();
            Initialize(ref inventory, Equipment.Items, 6, null);
            PlayerInventory.ScrollRect.verticalNormalizedPosition = 1;
        }


        /// <summary>
        /// Initialize owned items (just for example).
        /// </summary>
        public void TestInitialize()
        {
            var inventory = ItemCollection.Active.Items.Select(i => new Item(i.Id)).ToList(); // inventory.Clear();
			var equipped = new List<Item>();

            Initialize(ref inventory, equipped, 6, null);
		}

        public void Initialize(ref List<Item> inventory, List<Item> equipped, int bagSize, Action onRefresh)
        {
            RegisterCallbacks();
            PlayerInventory.Initialize(ref inventory);
            Equipment.SetBagSize(bagSize);
            Equipment.Initialize(ref equipped);
            Equipment.OnRefresh = onRefresh;

            if (!Equipment.SelectAny() && !PlayerInventory.SelectAny())
            {
                ItemInfo.Reset();
            }
        }

        public void RegisterCallbacks()
        {
            InventoryItem.OnLeftClick = SelectItem;
            InventoryItem.OnRightClick = InventoryItem.OnDoubleClick = QuickAction;
        }

        private void QuickAction(Item item)
        {
            SelectItem(item);

            if (Equipment.Items.Contains(item))
            {
                Remove();
            }
            else if (CanEquipSelectedItem())
            {
                Equip();
            }
        }

        public void SelectItem(Item item, RectTransform rectTransform = null)
        {
            SelectedItem = item;
            ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price, rectTransform);
            Refresh();
        }

        public void Equip()
        {
            if (!CanEquip(SelectedItem)) return;

            //var equipped = SelectedItem.IsFirearm
            //    ? Equipment.Items.Where(i => i.IsFirearm).ToList()
            //    : Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type && !i.IsFirearm).ToList();

            var equipped = Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type).ToList();

            if (equipped.Any())
            {
                AutoRemove(equipped, Equipment.Slots.Count(i => i.Supports(SelectedItem)));
            }

            //if (SelectedItem.IsTwoHanded) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            //if (SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());

            //if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            //if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
            //if (SelectedItem.IsTwoHanded || SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsFirearm).ToList());

            MoveItem(SelectedItem, PlayerInventory, Equipment);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
            ItemInfo.Reset();
            Equipment.SaveEquipment();
            OnEquip?.Invoke(SelectedItem);
        }

        public void Remove()
        {
            if(SelectedItem.Params.Type == CurrentTab || CurrentTab == ItemType.All)
            {
                MoveItem(SelectedItem, Equipment, PlayerInventory);
                SelectItem(SelectedItem);
            }
            else
            {
                Equipment.Items.Remove(SelectedItem);
                Equipment.Refresh(SelectedItem);
            }
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
            ItemInfo.Reset();
        }

        public void Craft()
        {
            var materials = MaterialList;

            if (CanCraft(materials))
            {
                materials.ForEach(i => PlayerInventory.Items.Single(j => j.Hash == i.Hash).Count -= i.Count);
                PlayerInventory.Items.RemoveAll(i => i.Count == 0);

                var itemId = SelectedItem.Params.FindProperty(PropertyId.Craft).Value;
                var existed = PlayerInventory.Items.SingleOrDefault(i => i.Id == itemId && i.Modifier == null);

                if (existed == null)
                {
                    PlayerInventory.Items.Add(new Item(itemId));
                }
                else
                {
                    existed.Count++;
                }

                PlayerInventory.Refresh(SelectedItem);
                CraftButton.interactable = CanCraft(materials);
                AudioSource.PlayOneShot(CraftSound, SfxVolume);
            }
            else
            {
                Debug.Log("No materials.");
            }
        }

        public void Learn()
        {
            // Implement your logic here!
        }

        public void Use()
        {
            Use(UseSound);
        }

        public void Use(AudioClip sound)
        {
            if (SelectedItem.Count == 1)
            {
                PlayerInventory.Items.Remove(SelectedItem);
                SelectedItem = PlayerInventory.Items.FirstOrDefault();

                if (SelectedItem == null)
                {
                    PlayerInventory.Refresh(null);
                    SelectedItem = Equipment.Items.FirstOrDefault();

                    if (SelectedItem != null)
                    {
                        Equipment.Refresh(SelectedItem);
                    }
                }
                else
                {
                    PlayerInventory.Refresh(SelectedItem);
                }
            }
            else
            {
                SelectedItem.Count--;
                PlayerInventory.Refresh(SelectedItem);
            }

            Equipment.OnRefresh?.Invoke();

            if (sound != null)
            {
                AudioSource.PlayOneShot(sound, SfxVolume);
            }
        }

        public Item Assemble()
        {
            if (SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(PropertyId.Fragments).ValueInt)
            {
                SelectedItem.Count -= SelectedItem.Params.FindProperty(PropertyId.Fragments).ValueInt;

                var crafted = new Item(SelectedItem.Params.FindProperty(PropertyId.Craft).Value);
                var existed = PlayerInventory.Items.SingleOrDefault(i => i.Hash == crafted.Hash);

                if (existed == null)
                {
                    PlayerInventory.Items.Add(crafted);
                }
                else
                {
                    existed.Count++;
                }

                if (SelectedItem.Count == 0)
                {
                    PlayerInventory.Items.Remove(SelectedItem);
                    SelectedItem = crafted;
                }

                PlayerInventory.Refresh(SelectedItem);

                return crafted;
            }

            return null;
        }

        public override void Refresh()
        {
            if (SelectedItem == null)
            {
                ItemInfo.Reset();
                EquipButton.SetActive(false);
                RemoveButton.SetActive(false);
            }
            else
            {
                var equipped = Equipment.Items.Contains(SelectedItem);

                EquipButton.SetActive(!equipped && CanEquipSelectedItem());
                RemoveButton.SetActive(equipped);
            }

            UseButton.SetActive(SelectedItem != null && CanUse());
            AssembleButton.SetActive(SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(PropertyId.Fragments).ValueInt);

            var receipt = SelectedItem != null && SelectedItem.Params.Type == ItemType.Recipe;

            if (CraftButton != null) CraftButton.SetActive(false);
            if (LearnButton != null) LearnButton.SetActive(false);

            if (receipt)
            {
                if (LearnButton == null)
                {
                    var materialSelected = !PlayerInventory.Items.Contains(SelectedItem) && !Equipment.Items.Contains(SelectedItem);

                    CraftButton.SetActive(true);
                    Materials.SetActive(materialSelected);
                    Equipment.Scheme.SetActive(!materialSelected);

                    var materials = MaterialList;

                    Materials.Initialize(ref materials);
                }
                else
                {
                    LearnButton.SetActive(true);
                }
            }

            OnRefresh?.Invoke(SelectedItem);
        }

        private List<Item> MaterialList => SelectedItem.Params.FindProperty(PropertyId.Materials).Value.Split(',').Select(i => i.Split(':')).Select(i => new Item(i[0], int.Parse(i[1]))).ToList();

        private bool CanEquipSelectedItem()
        {
            return PlayerInventory.Items.Contains(SelectedItem) && Equipment.Slots.Any(i => i.Supports(SelectedItem));
        }

        private bool CanUse()
        {
            switch (SelectedItem.Params.Type)
            {
                case ItemType.Container:
                case ItemType.Boosts:
                case ItemType.Coupon:
                    return true;
                default:
                    return false;
            }
        }

        private bool CanCraft(List<Item> materials)
        {
            return materials.All(i => PlayerInventory.Items.Any(j => j.Hash == i.Hash && j.Count >= i.Count));
        }

        /// <summary>
        /// Automatically removes items if target slot is busy.
        /// </summary>
        private void AutoRemove(List<Item> items, int max = 1)
        {
            long sum = 0;

            foreach (var p in items)
            {
                sum += p.Count;
            }

            if (sum == max)
            {
                MoveItemSilent(items.LastOrDefault(i => i.Id != SelectedItem.Id) ?? items.Last(), Equipment, PlayerInventory);
            }
        }

        [ContextMenu("SaveInventory")]
        public void SaveInventory()
        {
            SaveSystem.Save(ItemCollection.Active.Items, "player_item");
        }

        [ContextMenu("LoadInventory")]
        public void LoadInventory()
        {
            ItemCollection.Active.Items = SaveSystem.Load<List<ItemParams>>("player_item") ?? new List<ItemParams>();
        }
    }
}