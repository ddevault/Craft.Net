using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public class ClickWindowPacket : Packet
    {
        public byte WindowId;
        public short SlotIndex;
        public bool RightClick;
        public bool MiddleClick;
        public short ActionNumber;
        public bool Shift;
        public Slot ClickedItem;

        public override byte PacketId
        {
            get { return 0x66; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            byte shift, rightClick;
            if (!DataUtility.TryReadByte(buffer, ref offset, out WindowId))
                return -1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out SlotIndex))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out rightClick))
                return -1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out ActionNumber))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out shift))
                return -1;
            if (!Slot.TryReadSlot(buffer, ref offset, out ClickedItem))
                return -1;
            Console.WriteLine("Window click: " + shift + ", " + rightClick);
            Shift = shift == 1;
            RightClick = rightClick == 1;
            if (shift == 2 && rightClick == 3)
                MiddleClick = true;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (MiddleClick)
                return; // No effect in vanilla minecraft
            Window window = null;
            if (WindowId == 0)
                window = client.Entity.Inventory;
            // TODO: Fetch appropriate furnace/crafting bench/etc window
            if (window == null)
                return;
            if (Shift)
            {
                window.MoveToAlternateArea(SlotIndex);
                return;
            }
            if (SlotIndex == -999)
            {
                // TODO: Throw items out of windows
                return;
            }
            var clickedItem = client.Entity.Inventory[SlotIndex];
            var heldItem = client.Entity.ItemInMouse;
            if (heldItem == null)
            {
                if (clickedItem.Empty)
                    return;
                if (RightClick)
                {
                    var heldCount = (byte)(clickedItem.Count / 2 + (clickedItem.Count % 2));
                    var leftCount = (byte)(clickedItem.Count / 2);
                    client.Entity.ItemInMouse = new Slot(clickedItem.Id, heldCount, clickedItem.Metadata);
                    client.Entity.Inventory[SlotIndex].Count = leftCount;
                }
                else
                {
                    client.Entity.ItemInMouse = clickedItem;
                    client.Entity.Inventory[SlotIndex] = new Slot();
                }
            }
            else
            {
                if (RightClick && ((clickedItem.Id == heldItem.Id &&
                    clickedItem.Metadata == heldItem.Metadata) || clickedItem.Empty))
                {
                    if (!clickedItem.Empty && clickedItem.Count < clickedItem.Item.MaximumStack)
                    {
                        client.Entity.Inventory[SlotIndex] = new Slot(heldItem.Id,
                            (byte)(clickedItem.Count + (clickedItem.Empty ? 0 : 1)), heldItem.Metadata);
                        client.Entity.ItemInMouse.Count--;
                    }
                    else
                        client.Entity.Inventory[SlotIndex] = new Slot(heldItem.Id, 1, heldItem.Metadata);
                    }
                    else
                    {
                        if (clickedItem.Empty)
                        {
                            client.Entity.Inventory[SlotIndex] = heldItem;
                            client.Entity.ItemInMouse = new Slot();
                        }
                        else
                        {
                            client.Entity.ItemInMouse = clickedItem;
                            client.Entity.Inventory[SlotIndex] = heldItem;
                        }
                    }
                }
            }

            public override void SendPacket(MinecraftServer server, MinecraftClient client)
            {
                throw new InvalidOperationException();
            }
        }
}