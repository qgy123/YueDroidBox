using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SharpAdbClient;
using YueDroidBox.Model.Adb;

namespace YueDroidBox.Core.Adb.Receiver
{
    public class PortForwardingReceiver : MultiLineReceiver
    {
        private const string TAG = nameof(PortForwardingReceiver);
        private const string INFO = @"([a-zA-Z0-9_-]+)\s(.*?):(\d+)\s(.*?):(\d+)";
        public ForwardingInfo ForwardingInfo { get; private set; }

        private const RegexOptions REOPTIONS = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

        protected override void ProcessNewLines(IEnumerable<string> lines)
        {
            ForwardingInfo = new ForwardingInfo();

            foreach (var line in lines)
            {
                var match = line.Match(INFO, REOPTIONS);
                if (match.Success)
                {
                    this.ForwardingInfo.Serial = match.Groups[1].Value;

                    this.ForwardingInfo.LocalProtocol = match.Groups[2].Value;

                    try
                    {
                        this.ForwardingInfo.LocalPort = int.Parse(match.Groups[3].Value);
                    }
                    catch (FormatException)
                    {
                        Log.Warn(TAG, $"Failed to parse {match.Groups[3].Value} as an integer");
                    }

                    this.ForwardingInfo.RemoteProtocol = match.Groups[4].Value;

                    try
                    {
                        this.ForwardingInfo.RemotePort = int.Parse(match.Groups[5].Value);
                    }
                    catch (FormatException)
                    {
                        Log.Warn(TAG, $"Failed to parse {match.Groups[5].Value} as an integer");
                    }
                }
            }
        }
    }
}