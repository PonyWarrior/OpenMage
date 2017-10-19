using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$CreateShell", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class CreateShellPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(1)]
        public byte SubType { get; set; }

        [PacketIndex(2)]
        public byte Level { get; set; }

        [PacketIndex(3)]
        public byte Rarity { get; set; }

        public static string ReturnHelp()
        {
            return "$Drop TYPE SUBTYPE LEVEL RARITY";
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            // idk wtf this is used for so just leaving this
            return $"CreateShell Command";
        }

        #endregion
    }
}
