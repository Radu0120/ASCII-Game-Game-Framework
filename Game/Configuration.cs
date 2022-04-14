using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using ASCMandatory1;

namespace Game
{
    public class Configuration
    {
        const string ConfigFilePath = @"Config.xml";
        public static void ReadConfiguration()
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(ConfigFilePath);
            XmlNode root = configDoc.DocumentElement;

            KeyConverter converter = new KeyConverter();

            XmlNode MoveUp = root.SelectSingleNode("Movement/MoveUp");
            Keybinds.MoveUp = (Key)converter.ConvertFromString(MoveUp.InnerText.Trim());

            XmlNode MoveDown = root.SelectSingleNode("Movement/MoveDown");
            Keybinds.MoveDown = (Key)converter.ConvertFromString(MoveDown.InnerText.Trim());

            XmlNode MoveLeft = root.SelectSingleNode("Movement/MoveLeft");
            Keybinds.MoveLeft = (Key)converter.ConvertFromString(MoveLeft.InnerText.Trim());

            XmlNode MoveRight = root.SelectSingleNode("Movement/MoveRight");
            Keybinds.MoveRight = (Key)converter.ConvertFromString(MoveRight.InnerText.Trim());

            XmlNode PickUpItem = root.SelectSingleNode("Inventory/PickUpItem");
            Keybinds.PickUpItem = (Key)converter.ConvertFromString(PickUpItem.InnerText.Trim());

            XmlNode DropItem = root.SelectSingleNode("Inventory/DropItem");
            Keybinds.DropItem = (Key)converter.ConvertFromString(DropItem.InnerText.Trim());

            XmlNode UseItem = root.SelectSingleNode("Inventory/UseItem");
            Keybinds.UseItem = (Key)converter.ConvertFromString(UseItem.InnerText.Trim());

            XmlNode SwapItemLeft = root.SelectSingleNode("Inventory/SwapItemLeft");
            Keybinds.SwapItemLeft = (Key)converter.ConvertFromString(SwapItemLeft.InnerText.Trim());

            XmlNode SwapItemRight = root.SelectSingleNode("Inventory/SwapItemRight");
            Keybinds.SwapItemRight = (Key)converter.ConvertFromString(SwapItemRight.InnerText.Trim());

            XmlNode Exit = root.SelectSingleNode("Misc/Exit");
            Keybinds.Exit = (Key)converter.ConvertFromString(Exit.InnerText.Trim());

            XmlNode MenuOption1 = root.SelectSingleNode("Designer/MenuOption1");
            Keybinds.MenuOption1 = (Key)converter.ConvertFromString(MenuOption1.InnerText.Trim());

            XmlNode MenuOption2 = root.SelectSingleNode("Designer/MenuOption2");
            Keybinds.MenuOption2 = (Key)converter.ConvertFromString(MenuOption2.InnerText.Trim());

            XmlNode MenuOption3 = root.SelectSingleNode("Designer/MenuOption3");
            Keybinds.MenuOption3 = (Key)converter.ConvertFromString(MenuOption3.InnerText.Trim());

            XmlNode MenuOption4 = root.SelectSingleNode("Designer/MenuOption4");
            Keybinds.MenuOption4 = (Key)converter.ConvertFromString(MenuOption4.InnerText.Trim());

            XmlNode Build = root.SelectSingleNode("Designer/Build");
            Keybinds.Build = (Key)converter.ConvertFromString(Build.InnerText.Trim());

            XmlNode Delete = root.SelectSingleNode("Designer/Delete");
            Keybinds.Delete = (Key)converter.ConvertFromString(Delete.InnerText.Trim());

            XmlNode BuildMapUp = root.SelectSingleNode("Designer/BuildMapUp");
            Keybinds.BuildMapUp = (Key)converter.ConvertFromString(BuildMapUp.InnerText.Trim());

            XmlNode BuildMapDown = root.SelectSingleNode("Designer/BuildMapDown");
            Keybinds.BuildMapDown = (Key)converter.ConvertFromString(BuildMapDown.InnerText.Trim());

            XmlNode BuildMapLeft = root.SelectSingleNode("Designer/BuildMapLeft");
            Keybinds.BuildMapLeft = (Key)converter.ConvertFromString(BuildMapLeft.InnerText.Trim());

            XmlNode BuildMapRight = root.SelectSingleNode("Designer/BuildMapRight");
            Keybinds.BuildMapRight = (Key)converter.ConvertFromString(BuildMapRight.InnerText.Trim());
        }
    }
}
