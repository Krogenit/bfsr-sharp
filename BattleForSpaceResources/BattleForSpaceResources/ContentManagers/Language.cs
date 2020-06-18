using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources
{
    public enum StringName
    {
        Back, Connect, Multiplayer, Singleplayer, Settings, Quit, Ip, Port, Login, Register, Password, Ok, BattleForRes, Human, Engi, Saimon,
        SelectFaction, HumanDescr, CivDescr, EnemyDescr, Language, SoundVolume, MouseSpeed, Yes, No, UseSystemCursor, RenderBodys, Debug, Mouse, Sound,
        SaveOptionsBeforeExit, Save, BackToGame, InMainMenu, YouHasBeenDestroyedBy, Respawn, Player, Shield, Hull, Energy, Crew, Cargo, Page, Money, Crystals, 
        Metal, Experience, SciencePoints, Statistics, Info
    }
    public static class Language
    {
        public static string GetString(StringName sn)
        {
            if (Settings.language.Equals("ru"))
            {
                switch (sn)
                {
                    case StringName.Back:
                        return "Назад";
                    case StringName.Connect:
                        return "Подключиться";
                    case StringName.Multiplayer:
                        return "Сетевая игра";
                    case StringName.Singleplayer:
                        return "Одиночная игра";
                    case StringName.Settings:
                        return "Настройки";
                    case StringName.Quit:
                        return "Выход";
                    case StringName.Ip:
                        return "IP Адресс";
                    case StringName.Port:
                        return "Порт";
                    case StringName.Login:
                        return "Ник";
                    case StringName.Register:
                        return "Регистрация";
                    case StringName.Password:
                        return "Пароль";
                    case StringName.Ok:
                        return "Ок";
                    case StringName.BattleForRes:
                        return "Война за ресурсы";
                    case StringName.Human:
                        return "Люди";
                    case StringName.Engi:
                        return "Энжи";
                    case StringName.Saimon:
                        return "Саймоны";
                    case StringName.SelectFaction:
                        return "Выберите фракцию";
                    case StringName.HumanDescr:
                        return "Прочная броня\nОружие эффективно против брони";
                    case StringName.CivDescr:
                        return "Развитые реакторы\nОружие эффективно против корпуса";
                    case StringName.EnemyDescr:
                        return "Мощные щиты\nОружие эффективно против щитов";
                    case StringName.Language:
                        return "Язык";
                    case StringName.SoundVolume:
                        return "Громкость звуков";
                    case StringName.UseSystemCursor:
                        return "Системный курсор";
                    case StringName.Yes:
                        return "Да";
                    case StringName.No:
                        return "Нет";
                    case StringName.MouseSpeed:
                        return "Скорость мыши";
                    case StringName.Debug:
                        return "Отладка";
                    case StringName.RenderBodys:
                        return "Тела объектов";
                    case StringName.Mouse:
                        return "Мышь";
                    case StringName.Sound:
                        return "Звук";
                    case StringName.SaveOptionsBeforeExit:
                        return "Сохранить настройки перед выходом?";
                    case StringName.Save:
                        return "Сохранить";
                    case StringName.BackToGame:
                        return "Вернуться в игру";
                    case StringName.InMainMenu:
                        return "В главное меню";
                    case StringName.YouHasBeenDestroyedBy:
                        return "Вас уничтожил";
                    case StringName.Respawn:
                        return "Возродиться";
                    case StringName.Player:
                        return "игроком";
                    case StringName.Cargo:
                        return "Груз";
                    case StringName.Crew:
                        return "Экипаж";
                    case StringName.Energy:
                        return "Энергия";
                    case StringName.Hull:
                        return "Корпус";
                    case StringName.Shield:
                        return "Щит";
                    case StringName.Page:
                        return "Строка";
                    case StringName.Metal:
                        return "Металл";
                    case StringName.Money:
                        return "Кредиты";
                    case StringName.SciencePoints:
                        return "Очки науки";
                    case StringName.Experience:
                        return "Опыт";
                    case StringName.Crystals:
                        return "Кристаллы";
                    case StringName.Statistics:
                        return "Статистика";
                    case StringName.Info:
                        return "Информация";
                    default:
                        return "Пусто";
                }
            }
            else
            {
                return sn.ToString();
            }
        }
    }
}
