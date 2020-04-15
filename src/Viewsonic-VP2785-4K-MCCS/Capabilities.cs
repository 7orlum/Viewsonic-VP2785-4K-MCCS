using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class Capabilities
    {
        public class VCPCode
        {
            public bool Error { get; set; }
            public NativeMethods.MC_VCP_CODE_TYPE Type { get; set; }
            public string Access { get; set; }
            public uint CurrentValue { get; set; }
            public uint MaximumValue { get; set; }
            public List<uint> PossibleValues { get; set; }
            public string Description { get; set; }
        }


        public string ProtocolClass { get; private set; }
        public string DysplayType { get; private set; }
        public List<byte> Commands { get; private set; } = new List<byte>();
        public Dictionary<byte, VCPCode> VCPCodes { get; private set; } = new Dictionary<byte, VCPCode>();
        public string DysplayModel { get; private set; }
        public string SupportedMCCSVersion { get; private set; }


        public static Capabilities Parse(string capabilitiesString)
        {
            var result = new Capabilities();

            var match = new Regex(@"
				\(
					(
						(?<parameter>[^()]+)
						\(
						(?<value>(?>[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!)))
		                \)
	                )*
	            \)
	            ",
                RegexOptions.IgnorePatternWhitespace).Match(capabilitiesString);

            if (!match.Success)
                return result;

            for (var i = 0; i < match.Groups["parameter"].Captures.Count; i++)
                switch (match.Groups["parameter"].Captures[i].Value)
                {
                    case "prot":
                        result.ProtocolClass = match.Groups["value"].Captures[i].Value;
                        break;
                    case "type":
                        result.DysplayType = match.Groups["value"].Captures[i].Value;
                        break;
                    case "model":
                        result.DysplayModel = match.Groups["value"].Captures[i].Value;
                        break;
                    case "mccs_ver":
                        result.SupportedMCCSVersion = match.Groups["value"].Captures[i].Value;
                        break;
                    case "cmds":
                        result.Commands = ParseCommands(match.Groups["value"].Captures[i].Value);
                        break;
                    case "vcp":
                        result.VCPCodes = ParseVCPCodes(match.Groups["value"].Captures[i].Value);
                        break;
                }

            return result;
        }


        private static List<byte> ParseCommands(string commandsString)
        {
            var result = new List<byte>();

            var match = new Regex(@"((?<code>[0123456789ABCDEF]{2,2})\s*)*").Match(commandsString);

            if (match.Success)
                for (var i = 0; i < match.Groups["code"].Captures.Count; i++)
                    result.Add(byte.Parse(match.Groups["code"].Captures[i].Value, NumberStyles.HexNumber));

            return result;
        }


        private static Dictionary<byte, VCPCode> ParseVCPCodes(string vcpCodesString)
        {
            var result = new Dictionary<byte, VCPCode>();

            var match = new Regex(@"((?<code>[0-9a-fA-F]{2})(?<values>(\(.*?\))?)\s*)*").Match(vcpCodesString);

            if (match.Success)
                for (var i = 0; i < match.Groups["code"].Captures.Count; i++)
                {
                    var vcpCode = new VCPCode();
                    vcpCode.PossibleValues = ParseVCPCodeValues(match.Groups["values"].Captures[i].Value);

                    result.Add(byte.Parse(match.Groups["code"].Captures[i].Value, NumberStyles.HexNumber), vcpCode);
                }

            return result;
        }


        private static List<uint> ParseVCPCodeValues(string vcpCodeValuesString)
        {
            var result = new List<uint>();

            var match = new Regex(@"\(((?<code>[0123456789ABCDEF]{2,2})\s*)*\)").Match(vcpCodeValuesString);

            if (match.Success)
                for (var i = 0; i < match.Groups["code"].Captures.Count; i++)
                    result.Add(uint.Parse(match.Groups["code"].Captures[i].Value, NumberStyles.HexNumber));

            return result;
        }
    }
}
