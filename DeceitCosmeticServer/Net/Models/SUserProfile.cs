using DeceitCosmeticServer.Net.Interfaces;
using DeceitCosmeticServer.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeceitCosmeticServer.Net.Objects {
    public class SUserProfile : INetworkSerializable {
        public string Username;
        public short EmblemId;
        public uint[] Unlocks = Array.Empty<uint>();
        public List<SUserProfileParam> UserParams = new(0);
        public List<SUserProfileParam> ServerParams = new(0);
        public long CalculateSize() => 
            8 + 8 + 4 +
            2 + Encoding.UTF8.GetByteCount(Username) +
            2 + 4 + 8 + 4 + (Unlocks.Length << 2) +
            8 + 2 + 1 + 8 + 1 + 1 + 8 + 8 + 4 + 2 + 1 + 2 + UserParams.Select((x) => x.CalculateSize()).Sum() + 2 + 1 + 2 + ServerParams.Select((x) => x.CalculateSize()).Sum() + 2; //+2 out of idk where i lost it

        public byte[] Serialize() {
            _writer = new SNetworkWriter(CalculateSize());
            _writer.WriteLong(1);
            _writer.WriteLong(1);
            _writer.WriteInt(0);
            _writer.WriteShort(0);
            _writer.WriteString(Username);
            _writer.WriteShort(EmblemId);
            _writer.WriteInt(0);
            _writer.WriteLong(0);
            _writer.WriteInt(Unlocks.Length);
            for (int i = 0; i < Unlocks.Length; i++) _writer.WriteInt((int)Unlocks[i]);
            _writer.WriteLong(0);
            _writer.WriteShort(0);
            _writer.WriteByte(1);
            _writer.WriteLong(0);
            _writer.WriteByte(0);
            _writer.WriteByte(0xFF);
            _writer.WriteLong(0);
            _writer.WriteLong(0);
            _writer.WriteInt(0);
            _writer.WriteShort(0);
            _writer.WriteByte(0);
            _writer.WriteShort((short)UserParams.Count);
            for (int i = 0; i < UserParams.Count; i++) {
                _writer.WriteSerializable(UserParams[i]);
            }
            _writer.WriteShort(0);
            _writer.WriteByte(0);
            _writer.WriteShort((short)ServerParams.Count);
            for (int i = 0; i < ServerParams.Count; i++) {
                _writer.WriteSerializable(ServerParams[i]);
            }
            return _writer.GetData();
        }
        protected SNetworkWriter _writer = null;
    }
}
