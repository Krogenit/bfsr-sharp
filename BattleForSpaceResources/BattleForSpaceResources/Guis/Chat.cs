using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BattleForSpaceResources.Guis
{
    public class Chat
    {
        public GameObject guiChat;
        private int chatSliderPos;
        public List<string> chatStrings = new List<string>();
        public List<string> chatRefreshedStrings = new List<string>(), chatRenderStrigs = new List<string>();
        private Microsoft.Xna.Framework.Input.Keys[] lastKeys;
        private int timerKey, timerSlash, lenghtMax = 100, timerActive;
        private string stringInBox, refreshedString;
        public Rectangle rect = new Rectangle();
        private Core core = Core.GetCore();
        public Chat()
        {
            guiChat = new GameObject(Textures.guiChat, Vector2.Zero);
            guiChat.color = Vector4.Zero;
        }
        public void UpdateStrings()
        {
            chatRefreshedStrings = new List<string>();
            for (int i = 0; i < chatStrings.Count; i++)
            {
                RefreshString(chatStrings[i]);
            }
            chatRenderStrigs = new List<string>();
            if (chatRefreshedStrings.Count > 9)
            {
                int lenght = chatRefreshedStrings.Count - (chatSliderPos + 9);
                for (int i = lenght; i < chatRefreshedStrings.Count - chatSliderPos; i++)
                {
                    chatRenderStrigs.Add(chatRefreshedStrings[i]);
                }
            }
            else
            {
                chatRenderStrigs = chatRefreshedStrings;
            }
        }
        private void UpdateInputString(string s, int k)
        {
            if (s != null && Fonts.mediumFont.MeasureString(s).X > Textures.guiChat.Width / 2 + 20)
            {
                int rerNum = refreshedString.Length - k;
                refreshedString = null;
                for (int i = s.Length - rerNum; i < s.Length; i++)
                {
                    refreshedString += s[i];
                }
                if (Fonts.mediumFont.MeasureString(refreshedString).X > Textures.guiChat.Width / 2 + 20)
                {
                    UpdateInputString(refreshedString, k + 1);
                }
            }
            else
                refreshedString = s;
        }
        public void Update(Vector2 rightCurrentGuiPos)
        {
            guiChat.Position = rightCurrentGuiPos + new Vector2(core.cam.screenCenter.X + core.screenWidth / 2 - Textures.guiChat.Width * 0.58f / 2, core.cam.screenCenter.Y - core.screenHeight / 2 + Textures.guiChat.Height * 0.58f / 2);
            guiChat.color = GuiInGame.guiColor;
            guiChat.Size = 0.6f;
            if (--timerSlash <= 0)
                timerSlash = 60;
            if (core.isChat)
            {
                if (timerActive > 0)
                {
                    timerActive--;
                    stringInBox = "";
                    refreshedString = "";
                }
                if (core.inputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    if (stringInBox != null && stringInBox.Length > 0)
                    {
                        ClientPacketSender.SendMessage(stringInBox);
                        stringInBox = "";
                        refreshedString = "";
                    }
                }
                else
                {
                    Microsoft.Xna.Framework.Input.Keys[] keys = core.inputManager.GetPressedKeys();
                    if (lastKeys != null)
                        foreach (Microsoft.Xna.Framework.Input.Keys currentKey in keys)
                        {
                            if (currentKey != Microsoft.Xna.Framework.Input.Keys.None)
                            {
                                if (lastKeys.Contains(currentKey))
                                {
                                    if ((timerKey++ > 15))
                                    {
                                        HandleKey(currentKey);
                                        timerKey = 15;
                                    }
                                }
                                else if (!lastKeys.Contains(currentKey))
                                {
                                    HandleKey(currentKey);
                                }
                            }
                        }
                    lastKeys = keys;
                }
                int scroll = (int)(core.inputManager.GetMouseScroll() * 10);
                chatSliderPos += scroll;
                if (chatSliderPos > (chatRefreshedStrings.Count > 9 ? (chatRefreshedStrings.Count - 9) : 0))
                    chatSliderPos = (chatRefreshedStrings.Count > 9 ? (chatRefreshedStrings.Count - 9) : 0);
                else if (chatSliderPos < 0)
                    chatSliderPos = 0;
                if (scroll != 0)
                    UpdateStrings();
            }
            else
            {
                timerActive = 5;
            }
            rect = new Rectangle((int)(core.guiInGame.chat.guiChat.Position.X - core.guiInGame.chat.guiChat.Text.Width * 0.6f / 2),
    (int)(core.guiInGame.chat.guiChat.Position.Y - core.guiInGame.chat.guiChat.Text.Height * 0.6f / 2),
    (int)(core.guiInGame.chat.guiChat.Text.Width * 0.6f),
    (int)(core.guiInGame.chat.guiChat.Text.Height * 0.6f));
        }
        private void RefreshString(string s)
        {
            if (Fonts.mediumFont.MeasureString(s).X > Textures.guiChat.Width / 2 + 10)
            {
                string refreshed = "";
                for (int j = 0; j < s.Length; j++)
                {
                    if (Fonts.mediumFont.MeasureString(refreshed + s[j]).X > Textures.guiChat.Width / 2 + 10)
                    {

                    }
                    else
                        refreshed += s[j];
                }
                if (chatRefreshedStrings.Count < 50)
                {
                    chatRefreshedStrings.Add(refreshed);
                    int refreshedSize = refreshed.Length;
                    refreshed = "";
                    for (int j = refreshedSize; j < s.Length; j++)
                    {
                        refreshed += s[j];
                    }
                    RefreshString(refreshed);
                }
                else
                {
                    chatStrings.RemoveAt(0);
                    UpdateStrings();
                }
            }
            else if (chatRefreshedStrings.Count < 50)
            {
                chatRefreshedStrings.Add(s);
            }
            else
            {
                chatStrings.RemoveAt(0);
                UpdateStrings();
            }
        }
        private void HandleKey(Microsoft.Xna.Framework.Input.Keys currentKey)
        {
            string keyString = currentKey.ToString();
            if (currentKey == Microsoft.Xna.Framework.Input.Keys.Space && stringInBox.Length < lenghtMax)
                stringInBox += " ";
            else if ((currentKey == Microsoft.Xna.Framework.Input.Keys.Back || currentKey == Microsoft.Xna.Framework.Input.Keys.Delete) && stringInBox.Length > 0)
                stringInBox = stringInBox.Remove(stringInBox.Length - 1);
            else if (stringInBox == null || stringInBox.Length < lenghtMax)
                stringInBox += KeyInput(currentKey);
            timerKey = 0;
            UpdateInputString(stringInBox, 0);
        }
        private string KeyInput(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (InputLanguage.CurrentInputLanguage.Culture.ToString().Equals("en-US"))
            {
                if (core.inputManager.IsCapsLocked())
                {
                    switch (key)
                    {
                        case Microsoft.Xna.Framework.Input.Keys.D0:
                            return "0";
                        case Microsoft.Xna.Framework.Input.Keys.D1:
                            return "1";
                        case Microsoft.Xna.Framework.Input.Keys.D2:
                            return "2";
                        case Microsoft.Xna.Framework.Input.Keys.D3:
                            return "3";
                        case Microsoft.Xna.Framework.Input.Keys.D4:
                            return "4";
                        case Microsoft.Xna.Framework.Input.Keys.D5:
                            return "5";
                        case Microsoft.Xna.Framework.Input.Keys.D6:
                            return "6";
                        case Microsoft.Xna.Framework.Input.Keys.D7:
                            return "7";
                        case Microsoft.Xna.Framework.Input.Keys.D8:
                            return "8";
                        case Microsoft.Xna.Framework.Input.Keys.D9:
                            return "9";
                        case Microsoft.Xna.Framework.Input.Keys.OemPlus:
                            return "=";
                        case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            return ".";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuestion:
                            return "/";
                        case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            return ",";
                        case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                            return "-";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuotes:
                            return "'";
                        case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                            return "`";
                        case Microsoft.Xna.Framework.Input.Keys.OemSemicolon:
                            return ";";
                        case Microsoft.Xna.Framework.Input.Keys.A:
                            return "A";
                        case Microsoft.Xna.Framework.Input.Keys.B:
                            return "B";
                        case Microsoft.Xna.Framework.Input.Keys.C:
                            return "C";
                        case Microsoft.Xna.Framework.Input.Keys.D:
                            return "D";
                        case Microsoft.Xna.Framework.Input.Keys.E:
                            return "E";
                        case Microsoft.Xna.Framework.Input.Keys.F:
                            return "F";
                        case Microsoft.Xna.Framework.Input.Keys.G:
                            return "G";
                        case Microsoft.Xna.Framework.Input.Keys.H:
                            return "H";
                        case Microsoft.Xna.Framework.Input.Keys.I:
                            return "I";
                        case Microsoft.Xna.Framework.Input.Keys.J:
                            return "J";
                        case Microsoft.Xna.Framework.Input.Keys.K:
                            return "K";
                        case Microsoft.Xna.Framework.Input.Keys.L:
                            return "L";
                        case Microsoft.Xna.Framework.Input.Keys.M:
                            return "M";
                        case Microsoft.Xna.Framework.Input.Keys.N:
                            return "N";
                        case Microsoft.Xna.Framework.Input.Keys.O:
                            return "O";
                        case Microsoft.Xna.Framework.Input.Keys.P:
                            return "P";
                        case Microsoft.Xna.Framework.Input.Keys.Q:
                            return "Q";
                        case Microsoft.Xna.Framework.Input.Keys.R:
                            return "R";
                        case Microsoft.Xna.Framework.Input.Keys.S:
                            return "S";
                        case Microsoft.Xna.Framework.Input.Keys.T:
                            return "T";
                        case Microsoft.Xna.Framework.Input.Keys.U:
                            return "U";
                        case Microsoft.Xna.Framework.Input.Keys.V:
                            return "V";
                        case Microsoft.Xna.Framework.Input.Keys.W:
                            return "W";
                        case Microsoft.Xna.Framework.Input.Keys.X:
                            return "X";
                        case Microsoft.Xna.Framework.Input.Keys.Y:
                            return "Y";
                        case Microsoft.Xna.Framework.Input.Keys.Z:
                            return "Z";
                        case Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets:
                            return "{";
                        case Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets:
                            return "}";
                        default:
                            return "";
                    }
                }
                if (core.inputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                {
                    switch (key)
                    {
                        case Microsoft.Xna.Framework.Input.Keys.D0:
                            return ")";
                        case Microsoft.Xna.Framework.Input.Keys.D1:
                            return "!";
                        case Microsoft.Xna.Framework.Input.Keys.D2:
                            return "@";
                        case Microsoft.Xna.Framework.Input.Keys.D3:
                            return "#";
                        case Microsoft.Xna.Framework.Input.Keys.D4:
                            return "$";
                        case Microsoft.Xna.Framework.Input.Keys.D5:
                            return "%";
                        case Microsoft.Xna.Framework.Input.Keys.D6:
                            return "^";
                        case Microsoft.Xna.Framework.Input.Keys.D7:
                            return "&";
                        case Microsoft.Xna.Framework.Input.Keys.D8:
                            return "*";
                        case Microsoft.Xna.Framework.Input.Keys.D9:
                            return "(";
                        case Microsoft.Xna.Framework.Input.Keys.OemPlus:
                            return "+";
                        case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            return ">";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuestion:
                            return "?";
                        case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            return "<";
                        case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                            return "_";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuotes:
                            return "'";
                        case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                            return "~";
                        case Microsoft.Xna.Framework.Input.Keys.OemSemicolon:
                            return ":";
                        case Microsoft.Xna.Framework.Input.Keys.A:
                            return "A";
                        case Microsoft.Xna.Framework.Input.Keys.B:
                            return "B";
                        case Microsoft.Xna.Framework.Input.Keys.C:
                            return "C";
                        case Microsoft.Xna.Framework.Input.Keys.D:
                            return "D";
                        case Microsoft.Xna.Framework.Input.Keys.E:
                            return "E";
                        case Microsoft.Xna.Framework.Input.Keys.F:
                            return "F";
                        case Microsoft.Xna.Framework.Input.Keys.G:
                            return "G";
                        case Microsoft.Xna.Framework.Input.Keys.H:
                            return "H";
                        case Microsoft.Xna.Framework.Input.Keys.I:
                            return "I";
                        case Microsoft.Xna.Framework.Input.Keys.J:
                            return "J";
                        case Microsoft.Xna.Framework.Input.Keys.K:
                            return "K";
                        case Microsoft.Xna.Framework.Input.Keys.L:
                            return "L";
                        case Microsoft.Xna.Framework.Input.Keys.M:
                            return "M";
                        case Microsoft.Xna.Framework.Input.Keys.N:
                            return "N";
                        case Microsoft.Xna.Framework.Input.Keys.O:
                            return "O";
                        case Microsoft.Xna.Framework.Input.Keys.P:
                            return "P";
                        case Microsoft.Xna.Framework.Input.Keys.Q:
                            return "Q";
                        case Microsoft.Xna.Framework.Input.Keys.R:
                            return "R";
                        case Microsoft.Xna.Framework.Input.Keys.S:
                            return "S";
                        case Microsoft.Xna.Framework.Input.Keys.T:
                            return "T";
                        case Microsoft.Xna.Framework.Input.Keys.U:
                            return "U";
                        case Microsoft.Xna.Framework.Input.Keys.V:
                            return "V";
                        case Microsoft.Xna.Framework.Input.Keys.W:
                            return "W";
                        case Microsoft.Xna.Framework.Input.Keys.X:
                            return "X";
                        case Microsoft.Xna.Framework.Input.Keys.Y:
                            return "Y";
                        case Microsoft.Xna.Framework.Input.Keys.Z:
                            return "Z";
                        case Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets:
                            return "{";
                        case Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets:
                            return "}";
                        default:
                            return "";
                    }
                }
                else
                {
                    switch (key)
                    {
                        case Microsoft.Xna.Framework.Input.Keys.D0:
                            return "0";
                        case Microsoft.Xna.Framework.Input.Keys.D1:
                            return "1";
                        case Microsoft.Xna.Framework.Input.Keys.D2:
                            return "2";
                        case Microsoft.Xna.Framework.Input.Keys.D3:
                            return "3";
                        case Microsoft.Xna.Framework.Input.Keys.D4:
                            return "4";
                        case Microsoft.Xna.Framework.Input.Keys.D5:
                            return "5";
                        case Microsoft.Xna.Framework.Input.Keys.D6:
                            return "6";
                        case Microsoft.Xna.Framework.Input.Keys.D7:
                            return "7";
                        case Microsoft.Xna.Framework.Input.Keys.D8:
                            return "8";
                        case Microsoft.Xna.Framework.Input.Keys.D9:
                            return "9";
                        case Microsoft.Xna.Framework.Input.Keys.OemPlus:
                            return "=";
                        case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            return ".";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuestion:
                            return "/";
                        case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            return ",";
                        case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                            return "-";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuotes:
                            return "'";
                        case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                            return "`";
                        case Microsoft.Xna.Framework.Input.Keys.OemSemicolon:
                            return ";";
                        case Microsoft.Xna.Framework.Input.Keys.A:
                            return "a";
                        case Microsoft.Xna.Framework.Input.Keys.B:
                            return "b";
                        case Microsoft.Xna.Framework.Input.Keys.C:
                            return "c";
                        case Microsoft.Xna.Framework.Input.Keys.D:
                            return "d";
                        case Microsoft.Xna.Framework.Input.Keys.E:
                            return "e";
                        case Microsoft.Xna.Framework.Input.Keys.F:
                            return "f";
                        case Microsoft.Xna.Framework.Input.Keys.G:
                            return "g";
                        case Microsoft.Xna.Framework.Input.Keys.H:
                            return "h";
                        case Microsoft.Xna.Framework.Input.Keys.I:
                            return "i";
                        case Microsoft.Xna.Framework.Input.Keys.J:
                            return "j";
                        case Microsoft.Xna.Framework.Input.Keys.K:
                            return "k";
                        case Microsoft.Xna.Framework.Input.Keys.L:
                            return "l";
                        case Microsoft.Xna.Framework.Input.Keys.M:
                            return "m";
                        case Microsoft.Xna.Framework.Input.Keys.N:
                            return "n";
                        case Microsoft.Xna.Framework.Input.Keys.O:
                            return "o";
                        case Microsoft.Xna.Framework.Input.Keys.P:
                            return "p";
                        case Microsoft.Xna.Framework.Input.Keys.Q:
                            return "q";
                        case Microsoft.Xna.Framework.Input.Keys.R:
                            return "r";
                        case Microsoft.Xna.Framework.Input.Keys.S:
                            return "s";
                        case Microsoft.Xna.Framework.Input.Keys.T:
                            return "t";
                        case Microsoft.Xna.Framework.Input.Keys.U:
                            return "u";
                        case Microsoft.Xna.Framework.Input.Keys.V:
                            return "v";
                        case Microsoft.Xna.Framework.Input.Keys.W:
                            return "w";
                        case Microsoft.Xna.Framework.Input.Keys.X:
                            return "x";
                        case Microsoft.Xna.Framework.Input.Keys.Y:
                            return "y";
                        case Microsoft.Xna.Framework.Input.Keys.Z:
                            return "z";
                        case Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets:
                            return "[";
                        case Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets:
                            return "]";
                        default:
                            return "";
                    }
                }
            }
            else if (InputLanguage.CurrentInputLanguage.Culture.ToString().Equals("ru-RU"))
            {
                if (core.inputManager.IsCapsLocked())
                {
                    switch (key)
                    {
                        case Microsoft.Xna.Framework.Input.Keys.D0:
                            return "0";
                        case Microsoft.Xna.Framework.Input.Keys.D1:
                            return "1";
                        case Microsoft.Xna.Framework.Input.Keys.D2:
                            return "2";
                        case Microsoft.Xna.Framework.Input.Keys.D3:
                            return "3";
                        case Microsoft.Xna.Framework.Input.Keys.D4:
                            return "4";
                        case Microsoft.Xna.Framework.Input.Keys.D5:
                            return "5";
                        case Microsoft.Xna.Framework.Input.Keys.D6:
                            return "6";
                        case Microsoft.Xna.Framework.Input.Keys.D7:
                            return "7";
                        case Microsoft.Xna.Framework.Input.Keys.D8:
                            return "8";
                        case Microsoft.Xna.Framework.Input.Keys.D9:
                            return "9";
                        case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            return "Ю";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuestion:
                            return ",";
                        case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            return "Б";
                        case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                            return "_";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuotes:
                            return "Э";
                        case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                            return "Ё";
                        case Microsoft.Xna.Framework.Input.Keys.OemSemicolon:
                            return "Ж";
                        case Microsoft.Xna.Framework.Input.Keys.A:
                            return "Ф";
                        case Microsoft.Xna.Framework.Input.Keys.B:
                            return "И";
                        case Microsoft.Xna.Framework.Input.Keys.C:
                            return "С";
                        case Microsoft.Xna.Framework.Input.Keys.D:
                            return "В";
                        case Microsoft.Xna.Framework.Input.Keys.E:
                            return "У";
                        case Microsoft.Xna.Framework.Input.Keys.F:
                            return "А";
                        case Microsoft.Xna.Framework.Input.Keys.G:
                            return "П";
                        case Microsoft.Xna.Framework.Input.Keys.H:
                            return "Р";
                        case Microsoft.Xna.Framework.Input.Keys.I:
                            return "Ш";
                        case Microsoft.Xna.Framework.Input.Keys.J:
                            return "О";
                        case Microsoft.Xna.Framework.Input.Keys.K:
                            return "Л";
                        case Microsoft.Xna.Framework.Input.Keys.L:
                            return "Д";
                        case Microsoft.Xna.Framework.Input.Keys.M:
                            return "Ь";
                        case Microsoft.Xna.Framework.Input.Keys.N:
                            return "Т";
                        case Microsoft.Xna.Framework.Input.Keys.O:
                            return "Щ";
                        case Microsoft.Xna.Framework.Input.Keys.P:
                            return "З";
                        case Microsoft.Xna.Framework.Input.Keys.Q:
                            return "Й";
                        case Microsoft.Xna.Framework.Input.Keys.R:
                            return "К";
                        case Microsoft.Xna.Framework.Input.Keys.S:
                            return "Ы";
                        case Microsoft.Xna.Framework.Input.Keys.T:
                            return "Е";
                        case Microsoft.Xna.Framework.Input.Keys.U:
                            return "Г";
                        case Microsoft.Xna.Framework.Input.Keys.V:
                            return "М";
                        case Microsoft.Xna.Framework.Input.Keys.W:
                            return "Ц";
                        case Microsoft.Xna.Framework.Input.Keys.X:
                            return "Ч";
                        case Microsoft.Xna.Framework.Input.Keys.Y:
                            return "Н";
                        case Microsoft.Xna.Framework.Input.Keys.Z:
                            return "Я";
                        case Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets:
                            return "Х";
                        case Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets:
                            return "Ъ";
                        default:
                            return "";
                    }
                }
                if (core.inputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                {
                    switch (key)
                    {
                        case Microsoft.Xna.Framework.Input.Keys.D0:
                            return ")";
                        case Microsoft.Xna.Framework.Input.Keys.D1:
                            return "!";
                        case Microsoft.Xna.Framework.Input.Keys.D2:
                            return "@";
                        case Microsoft.Xna.Framework.Input.Keys.D3:
                            return "№";
                        case Microsoft.Xna.Framework.Input.Keys.D4:
                            return ";";
                        case Microsoft.Xna.Framework.Input.Keys.D5:
                            return "%";
                        case Microsoft.Xna.Framework.Input.Keys.D6:
                            return ":";
                        case Microsoft.Xna.Framework.Input.Keys.D7:
                            return "?";
                        case Microsoft.Xna.Framework.Input.Keys.D8:
                            return "*";
                        case Microsoft.Xna.Framework.Input.Keys.D9:
                            return "(";
                        case Microsoft.Xna.Framework.Input.Keys.OemPlus:
                            return "+";
                        case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            return "Ю";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuestion:
                            return ",";
                        case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            return "Б";
                        case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                            return "_";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuotes:
                            return "Э";
                        case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                            return "Ё";
                        case Microsoft.Xna.Framework.Input.Keys.OemSemicolon:
                            return "Ж";
                        case Microsoft.Xna.Framework.Input.Keys.A:
                            return "Ф";
                        case Microsoft.Xna.Framework.Input.Keys.B:
                            return "И";
                        case Microsoft.Xna.Framework.Input.Keys.C:
                            return "С";
                        case Microsoft.Xna.Framework.Input.Keys.D:
                            return "В";
                        case Microsoft.Xna.Framework.Input.Keys.E:
                            return "У";
                        case Microsoft.Xna.Framework.Input.Keys.F:
                            return "А";
                        case Microsoft.Xna.Framework.Input.Keys.G:
                            return "П";
                        case Microsoft.Xna.Framework.Input.Keys.H:
                            return "Р";
                        case Microsoft.Xna.Framework.Input.Keys.I:
                            return "Ш";
                        case Microsoft.Xna.Framework.Input.Keys.J:
                            return "О";
                        case Microsoft.Xna.Framework.Input.Keys.K:
                            return "Л";
                        case Microsoft.Xna.Framework.Input.Keys.L:
                            return "Д";
                        case Microsoft.Xna.Framework.Input.Keys.M:
                            return "Ь";
                        case Microsoft.Xna.Framework.Input.Keys.N:
                            return "Т";
                        case Microsoft.Xna.Framework.Input.Keys.O:
                            return "Щ";
                        case Microsoft.Xna.Framework.Input.Keys.P:
                            return "З";
                        case Microsoft.Xna.Framework.Input.Keys.Q:
                            return "Й";
                        case Microsoft.Xna.Framework.Input.Keys.R:
                            return "К";
                        case Microsoft.Xna.Framework.Input.Keys.S:
                            return "Ы";
                        case Microsoft.Xna.Framework.Input.Keys.T:
                            return "Е";
                        case Microsoft.Xna.Framework.Input.Keys.U:
                            return "Г";
                        case Microsoft.Xna.Framework.Input.Keys.V:
                            return "М";
                        case Microsoft.Xna.Framework.Input.Keys.W:
                            return "Ц";
                        case Microsoft.Xna.Framework.Input.Keys.X:
                            return "Ч";
                        case Microsoft.Xna.Framework.Input.Keys.Y:
                            return "Н";
                        case Microsoft.Xna.Framework.Input.Keys.Z:
                            return "Я";
                        case Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets:
                            return "Х";
                        case Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets:
                            return "Ъ";
                        default:
                            return "";
                    }
                }
                else
                {
                    switch (key)
                    {
                        case Microsoft.Xna.Framework.Input.Keys.D0:
                            return "0";
                        case Microsoft.Xna.Framework.Input.Keys.D1:
                            return "1";
                        case Microsoft.Xna.Framework.Input.Keys.D2:
                            return "2";
                        case Microsoft.Xna.Framework.Input.Keys.D3:
                            return "3";
                        case Microsoft.Xna.Framework.Input.Keys.D4:
                            return "4";
                        case Microsoft.Xna.Framework.Input.Keys.D5:
                            return "5";
                        case Microsoft.Xna.Framework.Input.Keys.D6:
                            return "6";
                        case Microsoft.Xna.Framework.Input.Keys.D7:
                            return "7";
                        case Microsoft.Xna.Framework.Input.Keys.D8:
                            return "8";
                        case Microsoft.Xna.Framework.Input.Keys.D9:
                            return "9";
                        case Microsoft.Xna.Framework.Input.Keys.OemPlus:
                            return "=";
                        case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                            return "ю";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuestion:
                            return ".";
                        case Microsoft.Xna.Framework.Input.Keys.OemComma:
                            return "б";
                        case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                            return "-";
                        case Microsoft.Xna.Framework.Input.Keys.OemQuotes:
                            return "э";
                        case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                            return "ё";
                        case Microsoft.Xna.Framework.Input.Keys.OemSemicolon:
                            return "ж";
                        case Microsoft.Xna.Framework.Input.Keys.A:
                            return "ф";
                        case Microsoft.Xna.Framework.Input.Keys.B:
                            return "и";
                        case Microsoft.Xna.Framework.Input.Keys.C:
                            return "с";
                        case Microsoft.Xna.Framework.Input.Keys.D:
                            return "в";
                        case Microsoft.Xna.Framework.Input.Keys.E:
                            return "у";
                        case Microsoft.Xna.Framework.Input.Keys.F:
                            return "а";
                        case Microsoft.Xna.Framework.Input.Keys.G:
                            return "п";
                        case Microsoft.Xna.Framework.Input.Keys.H:
                            return "р";
                        case Microsoft.Xna.Framework.Input.Keys.I:
                            return "ш";
                        case Microsoft.Xna.Framework.Input.Keys.J:
                            return "о";
                        case Microsoft.Xna.Framework.Input.Keys.K:
                            return "л";
                        case Microsoft.Xna.Framework.Input.Keys.L:
                            return "д";
                        case Microsoft.Xna.Framework.Input.Keys.M:
                            return "ь";
                        case Microsoft.Xna.Framework.Input.Keys.N:
                            return "т";
                        case Microsoft.Xna.Framework.Input.Keys.O:
                            return "щ";
                        case Microsoft.Xna.Framework.Input.Keys.P:
                            return "з";
                        case Microsoft.Xna.Framework.Input.Keys.Q:
                            return "й";
                        case Microsoft.Xna.Framework.Input.Keys.R:
                            return "к";
                        case Microsoft.Xna.Framework.Input.Keys.S:
                            return "ы";
                        case Microsoft.Xna.Framework.Input.Keys.T:
                            return "е";
                        case Microsoft.Xna.Framework.Input.Keys.U:
                            return "г";
                        case Microsoft.Xna.Framework.Input.Keys.V:
                            return "м";
                        case Microsoft.Xna.Framework.Input.Keys.W:
                            return "ц";
                        case Microsoft.Xna.Framework.Input.Keys.X:
                            return "ч";
                        case Microsoft.Xna.Framework.Input.Keys.Y:
                            return "н";
                        case Microsoft.Xna.Framework.Input.Keys.Z:
                            return "я";
                        case Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets:
                            return "х";
                        case Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets:
                            return "ъ";
                        default:
                            return "";
                    }
                }
            }
            else
            {
                return "";
            }
        }
        public void Render(SpriteBatch spriteBatch)
        {
            guiChat.Render(spriteBatch);
            if (refreshedString != null && refreshedString.Length > 0 && timerActive == 0)
                spriteBatch.DrawString(Fonts.mediumFont, refreshedString, guiChat.Position + new Vector2(-Textures.guiChat.Width * 0.6f / 2 + 22, Textures.guiChat.Height * 0.6f / 2 - 38), Color.White);
            if (timerSlash > 30 && core.isChat && refreshedString != null)
            {
                spriteBatch.DrawString(Fonts.mediumFont, "|", guiChat.Position + new Vector2(-Textures.guiChat.Width * 0.6f / 2 + 22 + Fonts.mediumFont.MeasureString(refreshedString).X, Textures.guiChat.Height * 0.6f / 2 - 38), new Color(GuiInGame.guiColor));
            }
            else if (timerSlash > 30 && core.isChat)
            {
                spriteBatch.DrawString(Fonts.mediumFont, "|", guiChat.Position + new Vector2(-Textures.guiChat.Width * 0.6f / 2 + 22, Textures.guiChat.Height * 0.6f / 2 - 38), new Color(GuiInGame.guiColor));
            }
            for (int i = 0; i < chatRenderStrigs.Count; i++)
            {
                spriteBatch.DrawString(Fonts.mediumFont, chatRenderStrigs[i], guiChat.Position + new Vector2(-Textures.guiChat.Width * 0.6f / 2 + 22, -Textures.guiChat.Height * 0.6f / 2 + 150 + ((chatRenderStrigs.Count - i) * -15)), Color.White);
            }
            GameObject.DrawRectangle(new Color(GuiInGame.guiColor), guiChat.Position + new Vector2(Textures.guiChat.Width * 0.6f / 2 - 28, -Textures.guiChat.Height * 0.6f / 2 - 5 + (chatRefreshedStrings.Count > 9 ? chatRefreshedStrings.Count * 2 : 20) - chatSliderPos * 2), guiChat.Position + new Vector2(Textures.guiChat.Width * 0.6f / 2 - 12, Textures.guiChat.Height * 0.6f / 2 - 45 - chatSliderPos * 2), spriteBatch, 2);
        }
    }
}
