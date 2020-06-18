using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BattleForSpaceResources
{
    public class Settings
    {
        public static bool isRenderBodys = false, isUseSystemCursor = true, isDebug = false;
        public static float mouseSpeed = 1.2F, soundVolume = 1f, gravity = 0.002F, renderDistance = 2500;
        public static string language = "ru", ip = "127.0.0.1";
        public static int port = 25565;
        public static XmlReader reader;
        public static XmlWriter writer;
        public static XmlWriterSettings settings = new XmlWriterSettings();
        public static void LoadSettings(Core c)
        {
            try
            {
                reader = XmlReader.Create("settings.xml");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Language":
                                reader.Read();
                                language = reader.Value;
                                break;
                            case "MouseSpeed":
                                reader.Read();
                                mouseSpeed = float.Parse(reader.Value);
                                break;
                            case "IsUseSystemCursor":
                                reader.Read();
                                c.IsMouseVisible = isUseSystemCursor = bool.Parse(reader.Value);
                                break;
                            case "SoundVolume":
                                reader.Read();
                                soundVolume = float.Parse(reader.Value);
                                break;
                            case "Ip":
                                reader.Read();
                                ip = reader.Value;
                                break;
                            case "Port":
                                reader.Read();
                                port = int.Parse(reader.Value);
                                break;
                            case "Debug":
                                reader.Read();
                                c.Window.AllowUserResizing = isDebug = bool.Parse(reader.Value);
                                break;
                        }
                    }
                }
                reader.Close();
            }
            catch
            {
                if(reader != null)
                reader.Close();
            }
        }
        public static void SaveSettings()
        {
            Core.console.AddDebugString("Saving settings...");
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            writer = XmlWriter.Create("settings.xml", settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Settings");
            {
                writer.WriteElementString("Language", language);
                writer.WriteElementString("MouseSpeed", mouseSpeed.ToString());
                writer.WriteElementString("IsUseSystemCursor", isUseSystemCursor.ToString());
                writer.WriteElementString("SoundVolume", soundVolume.ToString());
                writer.WriteElementString("Ip", ip.ToString());
                writer.WriteElementString("Port", port.ToString());
                writer.WriteElementString("Debug", isDebug.ToString());
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            Core.console.AddDebugString("Settings saved!");
        }
        public static void SaveLoginInfo(string name, string pass)
        {
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            writer = XmlWriter.Create("loginInfo.xml", settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("UserInfo");
            {
                writer.WriteElementString("Login", name);
                writer.WriteElementString("Password", pass);
                writer.WriteElementString("Ip", "127.0.0.1");
                writer.WriteElementString("Port", "25565");
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        public static void SaveLoginInfo(string name, string ip, string port)
        {
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            writer = XmlWriter.Create("loginInfo.xml", settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("UserInfo");
            {
                writer.WriteElementString("Login", name);
                writer.WriteElementString("Password", ".");
                writer.WriteElementString("Ip", ip);
                writer.WriteElementString("Port", port);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        public static string[] LoadLoginInfo()
        {
            string[] s = new string[4];
            try
            {
                reader = XmlReader.Create("loginInfo.xml");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Login":
                                reader.Read();
                                s[0] = reader.Value;
                                break;
                            case "Password":
                                reader.Read();
                                s[1] = reader.Value;
                                break;
                            case "Ip":
                                reader.Read();
                                s[2] = reader.Value;
                                break;
                            case "Port":
                                reader.Read();
                                s[3] = reader.Value;
                                break;
                        }
                    }
                }
                reader.Close();
                return s;
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
