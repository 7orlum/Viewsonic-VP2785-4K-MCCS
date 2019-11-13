using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DelaySecondsAttribute : Attribute
    {
        public TimeSpan Delay { get; private set; }


        public DelaySecondsAttribute(float delay)
        {
            Delay = TimeSpan.FromSeconds(delay);
        }
    }
}
