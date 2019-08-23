using System;

namespace YueDroidBox.Model.Adb
{
    public class ForwardingInfo
    {
        public string Serial { get; set; }
        public string LocalProtocol { get; set; }
        public int LocalPort { get; set; }
        public string RemoteProtocol { get; set; }
        public int RemotePort { get; set; }

        public override string ToString()
        {
            return $"{this.Serial} {this.LocalProtocol}:{this.LocalPort} {this.RemoteProtocol}:{this.RemotePort}";
        }
    }
}