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
        public Transform Tabs;
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

        public void Awake()
        {
            ItemCollection.Active = ItemCollection;
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

            Action<Item> equipAction;
            int equippedIndex;
            var tab = Tabs.GetComponentsInChildren<Toggle>().Single(i => i.isOn);

            ItemCollection.Active.Reset();

            //List<ItemSprite> SortByCollection(List<ItemSprite> collection)
            //{
            //    return collection.OrderBy(i => CollectionSorting.Contains(i.Collection) ? CollectionSorting.IndexOf(i.Collection) : 999).ThenBy(i => i.Id).ToList();
            //}
            ItemType itemType = ItemType.Undefined;
            switch (tab.name)
            {
                case "All":
                    {

                        break;
                    }
                case "Armor":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Armor);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Armor);
                        //equippedIndex = Character.Front.Armor == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Armor.SingleOrDefault(j => j.name == "FrontBody")));
                        itemType = ItemType.Armor;
                        break;
                    }
                case "Helmet":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Armor);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i, ".Armor.", ".Helmet.")).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Helmet);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Helmet));
                        itemType = ItemType.Helmet;
                        break;
                    }
                case "Vest":
                case "Bracers":
                case "Leggings":
                    {
                        string part;

                        switch (tab.name)
                        {
                            case "Vest": part = "FrontBody"; break;
                            case "Bracers": part = "FrontArmL"; break;
                            case "Leggings": part = "FrontLegL"; break;
                            default: throw new NotSupportedException(tab.name);
                        }

                        //var sprites = SortByCollection(SpriteCollection.Armor);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i, ".Armor.", $".{tab.name}.")).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, tab.name.ToEnum<EquipmentPart>());
                        //equippedIndex = Character.Front.Armor == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Armor.SingleOrDefault(j => j.name == part)));
                        break;
                    }
                case "Shield":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Shield);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Shield);
                        //equippedIndex = Character.Front.Shield == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Shield));
                        break;
                    }
                case "Back":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Back);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Back);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Back));
                        break;
                    }
                case "Wings":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Wings);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Wings);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Wings));
                        break;
                    }
                case "Melee1H":
                    {
                        //var sprites = SortByCollection(SpriteCollection.MeleeWeapon1H);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.MeleeWeapon1H);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                        break;
                    }
                case "Melee2H":
                    {
                        //var sprites = SortByCollection(SpriteCollection.MeleeWeapon2H);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.MeleeWeapon2H);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                        break;
                    }
                case "MeleePaired":
                    {
                        //var sprites = SortByCollection(SpriteCollection.MeleeWeapon1H);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.SecondaryMelee1H);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.SecondaryWeapon));
                        break;
                    }
                case "Bow":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Bow);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Bow);
                        //equippedIndex = Character.Front.CompositeWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.CompositeWeapon));
                        break;
                    }
                case "Crossbow":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Crossbow);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Crossbow);
                        //equippedIndex = Character.Front.CompositeWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.CompositeWeapon));
                        break;
                    }
                case "Firearm1H":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Firearm1H);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Firearm1H);
                        //equippedIndex = Character.Front.SecondaryWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                        break;
                    }
                case "Firearm2H":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Firearm2H);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Firearm2H);
                        //equippedIndex = Character.Front.PrimaryWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                        break;
                    }
                case "SecondaryFirearm1H":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Firearm1H);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.SecondaryFirearm1H);
                        //equippedIndex = Character.Front.SecondaryWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.SecondaryWeapon));
                        break;
                    }
                case "Body":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Body);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Body);
                        //equippedIndex = Character.Front.Body == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Body));
                        break;
                    }
                case "Head":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Body);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Head);
                        //equippedIndex = Character.Front.Head == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Head));
                        break;
                    }
                case "Ears":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Ears);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Ears);
                        //equippedIndex = Character.Front.Ears == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Ears));
                        break;
                    }
                case "Eyebrows":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Eyebrows);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Eyebrows);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Expressions[0].Eyebrows));
                        break;
                    }
                case "Eyes":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Eyes);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Eyes);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Expressions[0].Eyes));
                        break;
                    }
                case "Hair":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Hair);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Hair);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Hair));
                        break;
                    }
                case "Beard":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Beard);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Beard);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Beard));
                        break;
                    }
                case "Mouth":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Mouth);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Mouth);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Expressions[0].Mouth));
                        break;
                    }
                case "Makeup":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Makeup);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.SetBody(item.Sprite, BodyPart.Makeup);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Makeup));
                        break;
                    }
                case "Mask":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Mask);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Mask, item.Sprite != null && item.Sprite.Tags.Contains("Paint") ? null : Color.white);
                        //equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Mask));
                        break;
                    }
                case "Earrings":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Earrings);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Earrings);
                        //equippedIndex = Character.Front.Earrings == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Earrings));
                        break;
                    }
                case "Supplies":
                    {
                        //var sprites = SortByCollection(SpriteCollection.Supplies);

                        //ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                        //equipAction = item => { if (item.Id != null) Debug.LogWarning("Supplies are present as icons only and are not displayed on a character. Can be used for inventory."); };
                        //equippedIndex = -1;
                        break;
                    }
                default:
                    throw new NotImplementedException(tab.name);
            }

            //var items = ItemCollection.Active.Items.Select(i => new Item(i.Id)).ToList();
            //var emptyItem = new Item(null);

            //ItemCollection.Active.Items.Add(CreateFakeItemParams(emptyItem, null));
            //items.Insert(0, emptyItem);

            //var iconIds = IconCollection.Icons.Select(j => j.Id).ToList();

            

            //InventoryItem.OnLeftClick = item =>
            //{
            //    equipAction?.Invoke(item);
            //    EquipCallback?.Invoke(item);
            //    ItemName.text = item == emptyItem ? emptyItem.Id : item.Params.SpriteId;
            //    SetPaintButton(tab.name, item);
            //};
            //PlayerInventory.Initialize(ref items, items[equippedIndex + 1], reset: true);

            //var equipped = items.Count > equippedIndex + 1 ? items[equippedIndex + 1] : null;
            var items = ItemCollection.Active.Items.FindAll(i => i.Type == itemType).ToList();

            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Id != null && Equipment.Items.Exists(item => item.Id == items[i].Id))
                {
                    items.RemoveAt(i);

                    //if (equippedIndex == i) equippedIndex = -1;
                    //else if (equippedIndex > i) equippedIndex--;

                    //i--;
                }
            }


            var inventory = items.Select(i => new Item(i.Id)).ToList(); // inventory.Clear();
            //var equipped = new List<Item>();
            Initialize(ref inventory, Equipment.Items, 6, null);
            PlayerInventory.ScrollRect.verticalNormalizedPosition = 1;

            //SetPaintButton(tab.name, equipped);
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

        public void SelectItem(Item item)
        {
            SelectedItem = item;
            ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price);
            Refresh();
        }

        public void Equip()
        {
            if (!CanEquip(SelectedItem)) return;

            var equipped = SelectedItem.IsFirearm
                ? Equipment.Items.Where(i => i.IsFirearm).ToList()
                : Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type && !i.IsFirearm).ToList();

            if (equipped.Any())
            {
                AutoRemove(equipped, Equipment.Slots.Count(i => i.Supports(SelectedItem)));
            }

            if (SelectedItem.IsTwoHanded) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());

            if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
            if (SelectedItem.IsTwoHanded || SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsFirearm).ToList());

            MoveItem(SelectedItem, PlayerInventory, Equipment);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
            OnEquip?.Invoke(SelectedItem);
        }

        public void Remove()
        {
            MoveItem(SelectedItem, Equipment, PlayerInventory);
            SelectItem(SelectedItem);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
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
                case ItemType.Booster:
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
    }
}