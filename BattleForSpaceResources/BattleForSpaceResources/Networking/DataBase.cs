using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.ShipComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BattleForSpaceResources.Networking
{
    public class DataBase
    {
        private XmlReader reader;
        private XmlWriter writer;
        private XmlWriterSettings settings = new XmlWriterSettings();
        public DataBase()
        {

        }
        public string Login(string name, string password, bool isWar)
        {
            if (isWar)
            {
                try
                {
                    reader = XmlReader.Create("world\\players\\" + name + ".xml");
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "Password":
                                    reader.Read();
                                    if (password == reader.Value)
                                    {
                                        reader.Close();
                                        return "Успешно";
                                    }
                                    else
                                    {
                                        reader.Close();
                                        return "Неверный пароль";
                                    }
                            }
                        }
                    }
                    reader.Close();
                    return "Ошибка";
                }
                catch
                {
                    if (reader != null)
                        reader.Close();
                    return "Пользователь с таким имененм не зарегистрирован";
                }
            }
            else
            {
                try
                {
                    reader = XmlReader.Create("world\\players\\" + name + ".xml");
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "Password":
                                    {
                                        reader.Read();
                                    }
                                    break;
                            }
                        }
                    }
                    reader.Close();
                    return "Успешно";
                }
                catch
                {
                    if (reader != null)
                        reader.Close();
                    try
                    {
                        settings.Indent = true;
                        settings.OmitXmlDeclaration = true;
                        Directory.CreateDirectory("world\\players\\");
                        writer = XmlWriter.Create("world\\players\\" + name + ".xml", settings);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("User");
                        {
                            writer.WriteElementString("test", password);
                            writer.WriteElementString("Date", DateTime.Now.ToString());
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();
                        return "Успешно";
                    }
                    catch
                    {
                        if(writer != null)
                        writer.Close();
                        return "Ошибка";
                    }
                }
            }
        }
        public string Register(string name, string password)
        {
            try
            {
                reader = XmlReader.Create("world\\players\\" + name + ".xml");
                reader.Close();
                return "Пользователь с таким именем уже зарегистрирован";
            }
            catch(ArgumentException)
            {
                return "Недопустимые символы";
            }
            catch
            {
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                Directory.CreateDirectory("world\\players\\");
                writer = XmlWriter.Create("world\\players\\" + name + ".xml", settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("User");
                {
                    writer.WriteElementString("Password", password);
                    writer.WriteElementString("Date", DateTime.Now.ToString());
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
                return "Вы успешно зарегистрированы";
            }
        }
        public void SaveUser(string name, Ship s)
        {
            string pass = "";
            string date = "";
            int[] components = {s.engineType, s.reactorType, s.shieldType, s.armorPlates[0].armorType,s.armorPlates[1].armorType,s.armorPlates[2].armorType,s.armorPlates[3].armorType};
            try
            {
                reader = XmlReader.Create("world\\players\\" + name + ".xml");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Password":
                                reader.Read();
                                pass = reader.Value;
                                break;
                            case "Date":
                                reader.Read();
                                date = reader.Value;
                                break;
                        }
                    }
                    if (pass.Length > 0)
                    {
                        break;
                    }
                }
                reader.Close();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                writer = XmlWriter.Create("world\\players\\" + name + ".xml", settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("User");
                {
                    writer.WriteElementString("Password", pass);
                    writer.WriteElementString("Date", date);
                    writer.WriteElementString("Faction", ((byte)s.shipFaction).ToString());
                    writer.WriteStartElement("Ship");
                    {
                        writer.WriteElementString("ShipType", ((byte)s.shipType).ToString());
                        writer.WriteElementString("Engine", components[0].ToString());
                        writer.WriteElementString("Reactor", components[1].ToString());
                        writer.WriteElementString("Shield", components[2].ToString());
                        writer.WriteElementString("ArmorFace", components[3].ToString());
                        writer.WriteElementString("ArmorFront", components[5].ToString());
                        writer.WriteElementString("ArmorLeft", components[6].ToString());
                        writer.WriteElementString("ArmorRight", components[4].ToString());
                        writer.WriteStartElement("GunSlots");
                        {
                            writer.WriteElementString("Count", s.gunSlots.Length.ToString());
                            for (int i = 0; i < s.gunSlots.Length; i++)
                            {
                                if(s.gunSlots[i] != null)
                                writer.WriteElementString("GunType", ((byte)s.gunSlots[i].gunType).ToString());
                            }
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            catch(IOException e)
            {
                ServerCore.LogAdd(""+e);
                if (reader != null)
                    reader.Close();
            }
        }
        public int[] GetUserComponents(string name)
        {
            int[] components = new int[7];
            try
            {
                reader = XmlReader.Create("world\\players\\" + name + ".xml");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Engine":
                                reader.Read();
                                components[0] = int.Parse(reader.Value);
                                break;
                            case "Reactor":
                                reader.Read();
                                components[1] = int.Parse(reader.Value);
                                break;
                            case "Shield":
                                reader.Read();
                                components[2] = int.Parse(reader.Value);
                                break;
                            case "ArmorFace":
                                reader.Read();
                                components[3] = int.Parse(reader.Value);
                                break;
                            case "ArmorFront":
                                reader.Read();
                                components[5] = int.Parse(reader.Value);
                                break;
                            case "ArmorLeft":
                                reader.Read();
                                components[6] = int.Parse(reader.Value);
                                break;
                            case "ArmorRight":
                                reader.Read();
                                components[4] = int.Parse(reader.Value);
                                break;
                        }
                    }
                }
                reader.Close();
                return components;
            }
            catch
            {
                if (reader != null)
                    reader.Close();
                return null;
            }
        }
        public byte[] GetUserHangar(string name)
        {
            byte[] i = new byte[2];
            try
            {
                reader = XmlReader.Create("world\\players\\" + name + ".xml");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "ShipType":
                                reader.Read();
                                i[0] = byte.Parse(reader.Value);
                                break;
                            case "Faction":
                                reader.Read();
                                i[1] = byte.Parse(reader.Value);
                                break;
                        }
                    }
                }
                reader.Close();
                return i;
            }
            catch
            {
                if (reader != null)
                    reader.Close();
                return null;
            }
        }
        public GunSlot[] GetUserGuns(string name, Ship s)
        {
            GunSlot[] gs= null;
            byte gunNum = 0;
            try
            {
                reader = XmlReader.Create("world\\players\\" + name + ".xml");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Count":
                                {
                                    reader.Read();
                                    gs = new GunSlot[int.Parse(reader.Value)];
                                }
                                break;
                            case "GunType":
                                {
                                    reader.Read();
                                    GunType gt = (GunType)byte.Parse(reader.Value);
                                    gs[gunNum] = new GunSlot(gt, s);
                                    gunNum++;
                                }
                                break;
                        }
                    }
                }
                reader.Close();
                return gs;
            }
            catch
            {
                if (reader != null)
                    reader.Close();
                return null;
            }
        }
    }
}
