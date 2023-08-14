using Throw;

namespace ConsoleApp1;

public class Rule
{
    int Id { get; set; }
    ForwardProtocol Protocol { get; set; }
    ushort SourcePort { get; set; }
    ushort DestinationtPort { get; set; }
    string DestinationIP { get; set; }

#error дописать тут
    public override string? ToString() => $"Id: {Id}, ";

    public Rule(int id, ForwardProtocol protocol, ushort sourcePort, ushort destinationtPort, string destinationIP)
    {
        Id = id;
        Protocol = protocol;
        SourcePort = sourcePort;
        DestinationtPort = destinationtPort;
        DestinationIP = destinationIP;
    }
    public static Rule Parse(string str)
    {
        str.Throw(_=> new ArgumentException("str parameter is NullOrEmptyOrWhiteSpace.")).IfNullOrWhiteSpace(_ => _);

        var getIdTask = Task.Run(() =>
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ') { break; }
                chars.Add(str[i]);
            }
            string d = new string(chars.ToArray());

            int id = int.Parse(d);
            return id;
        });

        var getProtoTask = Task.Run(() =>
        {
            bool isTcp = str.Contains("tcp");
            bool isUdp = str.Contains("udp");
            if (isTcp is true) { return ForwardProtocol.TCP; }
            if (isUdp is true) { return ForwardProtocol.UDP; }
            throw new Exception("Протокол не определен.");
        });

        var getSourcePortTask = Task.Run(() =>
        {
            int colonIndex = str.IndexOf(':');
            var chars = new List<char>();
            for (int i = colonIndex + 1; i < str.Length; i++)
            {
                if (str[i] == ' ') { break; }
                chars.Add(str[i]);
            }
            string d = new string(chars.ToArray());

            ushort port = ushort.Parse(d);
            return port;

        });

        var getDestinationPortTask = Task.Run(() =>
        {
            int colonIndex = str.LastIndexOf(':');
            var chars = new List<char>();
            for (int i = colonIndex + 1; i < str.Length; i++)
            {
                if (str[i] == ' ') { break; }
                chars.Add(str[i]);
            }
            string d = new string(chars.ToArray());

            ushort port = ushort.Parse(d);
            return port;

        });

        var getDestinationIpTask = Task.Run(() =>
        {
            int toColonIndex = str.IndexOf("to:");
            var chars = new List<char>();
            for (int i = toColonIndex + 3; i < str.Length; i++)
            {
                if (str[i] == ':') { break; }
                chars.Add(str[i]);
            }
            string ip = new string(chars.ToArray());
            return ip;

        });

        Task.WaitAll(getIdTask, getProtoTask, getSourcePortTask, getDestinationPortTask, getDestinationIpTask);
        int id = getIdTask.Result;
        var proto = getProtoTask.Result;
        var sourcePort = getSourcePortTask.Result;
        var destinationPort = getDestinationPortTask.Result;
        var destinationIp = getDestinationIpTask.Result;
        return new Rule(id, proto, sourcePort, destinationPort, destinationIp);
    }
    public static List<Rule> Parse(IEnumerable<string> lines)
    {
        var list = lines.ToList();
        list.RemoveAt(0);
        list.RemoveAt(0);
        list.RemoveAt(list.Count - 1);


        var rules = new List<Rule>();
        foreach (var line in list)
        {
            rules.Add(Parse(line));
        }
        return rules;
    }
}
