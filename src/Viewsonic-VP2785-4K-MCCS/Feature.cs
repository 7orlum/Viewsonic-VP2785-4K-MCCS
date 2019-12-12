using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public abstract class Feature
    {
        public string Name { get; protected set; }
        public byte Code { get; protected set; }
        public TimeSpan Delay { get; protected set; }
        public string Description { get; protected set; }


        protected Feature(string name, byte code, float delaySeconds = 0.5f)
        {
            Name = name;
            Code = code;
            Delay = TimeSpan.FromSeconds(delaySeconds);
        }


        public abstract bool TryParseValue(YamlNode node, out uint value);


        public abstract string ValueName(uint value);


        public abstract string YamlConfigTemplate();
    }
}
