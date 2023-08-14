using Throw;

namespace ConsoleApp1;

public class Rule
{
    public int Id { get; private set; }
    public ForwardProtocol Protocol { get; private set; }
    public ushort SourcePort { get; private set; }
    public ushort DestinationtPort { get; private set; }
    public string DestinationIP { get; private set; }


    public override string? ToString() => $"Id: {Id}, Proto: {Protocol}, Source port: {SourcePort}, Destination: {DestinationIP}:{DestinationtPort}";

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
        var getIdTask = GetIdAsync(str);
        var getProtoTask = GetProtoAsync(str);
        var getSourcePortTask = GetSourcePortAsync(str);
        var getDestinationPortTask = GetDestinationPortAsync(str);
        var getDestinationIpTask = GetDestinationIpAsync(str);


        Task.WaitAll(getIdTask, getProtoTask, getSourcePortTask, getDestinationPortTask, getDestinationIpTask);
        int id = getIdTask.Result;
        var proto = getProtoTask.Result;
        var sourcePort = getSourcePortTask.Result;
        var destinationPort = getDestinationPortTask.Result;
        var destinationIp = getDestinationIpTask.Result;
        return new Rule(id, proto, sourcePort, destinationPort, destinationIp);
    }
    public static List<Rule> ParseList(IEnumerable<string> lines)
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
    private static async Task<int> GetIdAsync(string str)
    {
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
        return await getIdTask;
    }
    private static async Task<ForwardProtocol> GetProtoAsync(string str)
    {
        var getProtoTask = Task.Run(() =>
        {
            bool isTcp = str.Contains("tcp");
            bool isUdp = str.Contains("udp");
            if (isTcp is true) { return ForwardProtocol.TCP; }
            if (isUdp is true) { return ForwardProtocol.UDP; }
            throw new Exception("Протокол не определен.");
        });
        return await getProtoTask;
    }
    private static async Task<ushort> GetSourcePortAsync(string str)
    {
        var getSourcePortTask = Task.Run(() =>
        {
            int colonIndex = str.IndexOf(':');
            var chars = new List<char>();
            for (int i = colonIndex + 1; i<str.Length; i++)
            {
                if (str[i] == ' ') { break; }
                chars.Add(str[i]);
            }
            string d = new string(chars.ToArray());

            ushort port = ushort.Parse(d);
            return port;
        });
        return await getSourcePortTask;
    }
    private static async Task<ushort> GetDestinationPortAsync(string str)
    {
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
        return await getDestinationPortTask;
    }
    private static async Task<string> GetDestinationIpAsync(string str)
    {
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
        return await getDestinationIpTask;
    }
}