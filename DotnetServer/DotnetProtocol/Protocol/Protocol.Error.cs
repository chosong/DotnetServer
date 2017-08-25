using System;
using System.IO;
using ProtoBuf;

namespace DotnetPJ
{
	[ProtoContract]
	public class ErrorRes : ProtocolRes
	{
		public ErrorRes() : base(ProtocolId.Error) {}

		public ErrorRes(Result result) : base(ProtocolId.Error)
		{
			Result = result;
		}
	}
}
