using System.Text.RegularExpressions;
using Throw;

namespace EzPortForwardingLib;

public class Rule
{
    public ForwardProtocol Protocol { get; private set; }
    public ushort SourcePort { get; private set; }
    public ushort DestinationtPort { get; private set; }
    public string DestinationIP { get; private set; }

    public override string? ToString() => $"Proto: {Protocol}, Source id: {SourcePort}, Destination: {DestinationIP}:{DestinationtPort}";
    
    public Rule(ForwardProtocol protocol, ushort sourcePort, ushort destinationtPort, string destinationIP)
    {
        var regexIP = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        regexIP.IsMatch(destinationIP).Throw(_ => new ArgumentException($"IP-адрес {destinationIP} имеет неверный формат.")).IfFalse();

        Protocol = protocol;
        SourcePort = sourcePort;
        DestinationtPort = destinationtPort;
        DestinationIP = destinationIP;
    }
    
}