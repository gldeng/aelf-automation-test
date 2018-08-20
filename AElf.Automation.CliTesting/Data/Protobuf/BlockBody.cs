﻿using System.Collections.Generic;
using ProtoBuf;

namespace AElf.Automation.CliTesting.Data.Protobuf
{
    [ProtoContract]
    public class BlockBody
    {
        [ProtoMember(1)]
        public Hash BlockHeader { get; set; }
        
        [ProtoMember(2)]
        public List<Hash> Transactions { get; set; }
    }
}