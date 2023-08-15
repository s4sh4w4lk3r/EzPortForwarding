using Throw;

namespace EzPortForwardingLib;

public class RuleWithId : Rule
{
    public int Id { get; private set; }
    public RuleWithId(int id, ForwardProtocol protocol, ushort sourcePort, ushort destinationtPort, string destinationIP) : base(protocol, sourcePort, destinationtPort, destinationIP)
    {
        Id = id;
    }
    public override string? ToString() => $"Id: {Id}, Proto: {Protocol}, Source port: {SourcePort}, Destination: {DestinationIP}:{DestinationtPort}";

    public static RuleWithId Parse(string str)
    {
        str.Throw(_ => new ArgumentException("Параметр str содежрит пустой символ или NULL.")).IfNullOrWhiteSpace(_ => _);
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
        return new RuleWithId(id, proto, sourcePort, destinationPort, destinationIp);
    }
    public static List<RuleWithId> ParseList(IEnumerable<string> lines)
    {
        var list = lines.ToList();
        list.RemoveAt(0);
        list.RemoveAt(0);
        list.RemoveAt(list.Count - 1);


        var rules = new List<RuleWithId>();
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
            string idStr = new string(chars.ToArray());

            int.TryParse(idStr, out int id).Throw(_ => new FormatException($"Строку {idStr} не получилось преобразовать в int id.")).IfFalse();
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
            for (int i = colonIndex + 1; i < str.Length; i++)
            {
                if (str[i] == ' ') { break; }
                chars.Add(str[i]);
            }
            string portStr = new string(chars.ToArray());

            ushort.TryParse(portStr, out ushort port).Throw(_ => new FormatException($"Строку {portStr} не получилось преобразовать в порт.")).IfFalse();
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
            string portStr = new string(chars.ToArray());

            ushort.TryParse(portStr, out ushort port).Throw(_ => new FormatException($"Строку {portStr} не получилось преобразовать в порт.")).IfFalse();
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
