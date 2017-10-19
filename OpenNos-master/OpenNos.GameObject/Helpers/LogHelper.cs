﻿using System;
using OpenNos.Core;
using OpenNos.Data;
using OpenNos.DAL;
using OpenNos.Domain;

namespace OpenNos.GameObject.Helpers
{
    public class LogHelper : Singleton<LogHelper>
    {
        public void InsertCommandLog(long characterId, PacketDefinition commandPacket, string ipAddress)
        {
            string withoutHeaderpacket = string.Empty;
            string[] packet = commandPacket.OriginalContent.Split(' ');
            for (int i = 1; i < packet.Length; i++)
            {
                withoutHeaderpacket += $" {packet[i]}";
            }
            LogCommandsDTO command = new LogCommandsDTO
            {
                CharacterId = characterId,
                Command = commandPacket.OriginalHeader,
                Data = withoutHeaderpacket,
                IpAddress = ipAddress,
                Timestamp = DateTime.Now
            };
            DAOFactory.LogCommandsDAO.InsertOrUpdate(ref command);
        }

        public void InsertChatLog(ChatType type, long characterId, string message, string ipAddress)
        {
            LogChatDTO log = new LogChatDTO
            {
                CharacterId = characterId,
                ChatMessage = message,
                IpAddress = ipAddress,
                ChatType = (byte) type,
                Timestamp = DateTime.Now
            };
            DAOFactory.LogChatDAO.InsertOrUpdate(ref log);
        }
    }
}