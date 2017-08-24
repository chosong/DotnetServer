namespace DotnetProtocol
{
	public static class Version
	{
		public static readonly string Ver = "1.9.0.3";

		public static ushort[] Vs { get; private set; }
		public static ushort V1 { get { return Vs[0]; } }
		public static ushort V2 { get { return Vs[1]; } }
		public static ushort V3 { get { return Vs[2]; } }
		public static ushort V4 { get { return Vs[3]; } }

		static Version()
		{
			string[] tokens = Ver.Split('.');

			Vs = new ushort[tokens.Length];
			for (int i = 0; i < tokens.Length; i++)
			{
				Vs[i] = ushort.Parse(tokens[i]);
			}
		}
	}
}
