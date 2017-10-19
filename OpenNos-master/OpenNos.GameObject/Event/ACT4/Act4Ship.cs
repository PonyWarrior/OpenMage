﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace OpenNos.GameObject.Event
{
    public class Act4ShipTask
    {
        #region Methods

        public static void Run(FactionType faction)
        {
            MapInstance map = faction == FactionType.Angel ? ServerManager.Instance.Act4ShipAngel : ServerManager.Instance.Act4ShipDemon;
            OpenShip();
            Observable.Timer(TimeSpan.FromMinutes(1)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_MINUTES"), 4), 0));
            });
            Observable.Timer(TimeSpan.FromMinutes(2)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_MINUTES"), 3), 0));
            });
            Observable.Timer(TimeSpan.FromMinutes(3)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_MINUTES"), 2), 0));
            });
            Observable.Timer(TimeSpan.FromMinutes(4)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("SHIP_MINUTE"), 0));
                LockShip();
            });
            Observable.Timer(TimeSpan.FromMinutes(4) + TimeSpan.FromSeconds(30)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_SECONDS"), 30), 0));
            });
            Observable.Timer(TimeSpan.FromMinutes(4) + TimeSpan.FromSeconds(50)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_SECONDS"), 10), 0));
            });
            Observable.Timer(TimeSpan.FromMinutes(4) + TimeSpan.FromSeconds(55)).Subscribe(o =>
            {
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("SHIP_SETOFF"), 0));
                List<ClientSession> sessions = map.Sessions.Where(s => s?.Character != null).ToList();
                TeleportPlayers(sessions);
            });
        }

        private static void TeleportPlayers(IEnumerable<ClientSession> sessions)
        {
            foreach (ClientSession s in sessions)
            {
                switch (s.Character.Faction)
                {
                    case FactionType.Neutral:
                        ServerManager.Instance.ChangeMap(s.Character.CharacterId, 145, 51, 41);
                        s.SendPacket(UserInterfaceHelper.Instance.GenerateInfo("NEED_FACTION_ACT4"));
                        return;
                    case FactionType.Angel:
                        s.Character.MapId = 130;
                        s.Character.MapX = (short)(12 + ServerManager.Instance.RandomNumber(-2, 3));
                        s.Character.MapY = (short)(40 + ServerManager.Instance.RandomNumber(-2, 3));
                        break;
                    case FactionType.Demon:
                        s.Character.MapId = 131;
                        s.Character.MapX = (short)(12 + ServerManager.Instance.RandomNumber(-2, 3));
                        s.Character.MapY = (short)(40 + ServerManager.Instance.RandomNumber(-2, 3));
                        break;
                }
                //todo: get act4 channel dynamically
                if (!s.Character.ConnectAct4())
                {
                    ServerManager.Instance.ChangeMap(s.Character.CharacterId, 145, 51, 41);
                }
            }
        }

        private static void LockShip()
        {
            EventHelper.Instance.RunEvent(new EventContainer(ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(145)), EventActionType.NPCSEFFECTCHANGESTATE, true));
        }

        private static void OpenShip()
        {
            EventHelper.Instance.RunEvent(new EventContainer(ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(145)), EventActionType.NPCSEFFECTCHANGESTATE, false));
        }

        #endregion
    }
}