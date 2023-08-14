using Renci.SshNet;
using System.Net;
using Throw;

namespace ConsoleApp1;

public class IpTables
{
    public string Table { get; private set; }
    public string Chain { get; private set; }
    private SshClient Client { get; set; }
    public IpTables(string table, string chain, SshClient client)
    {
        Client = client;
        chain.Throw(s => throw new ArgumentException("\"Chain\" parameter is NullOrEmptyOrWhiteSpace.")).IfNullOrWhiteSpace(_ => _);
        Chain = chain;
        table.Throw(s => throw new ArgumentException("\"Table\" parameter is NullOrEmptyOrWhiteSpace.")).IfNullOrWhiteSpace(_ => _);
        Table = table;
    }
    public string Ping(string hostname)
    {
        Client.Connect();
        var cmd = Client.CreateCommand($"ping -c 5 {hostname} -q");
        var res = cmd.Execute();
        Client.Disconnect();
        return res;
    }

    public void AddForwardingRule(ushort sourcePort, string localIP, ushort localPort, ForwardProtocol proto)
    {

        Client.Connect();
        SshCommand addRuleCommand = null!;
        switch (proto)
        {
            case ForwardProtocol.TCP:
                addRuleCommand = Client.CreateCommand($"iptables -t {Table} -A {Chain} -p tcp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort}");
                break;
            case ForwardProtocol.UDP:
                addRuleCommand = Client.CreateCommand($"iptables -t {Table} -A {Chain} -p udp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort}");
                break;
            case ForwardProtocol.Both:
                addRuleCommand = Client.CreateCommand($"iptables -t {Table} -A {Chain} -p tcp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort} && " +
                    $"iptables -t nat -A {Chain} -p udp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort}");
                break;
        }
        addRuleCommand.Execute();
        Client.Disconnect();

        GetRules();
    }
    public List<Rule> GetRules() 
    {
        Client.Connect();

        var printRulesCommand = Client.CreateCommand($"iptables -L {Chain} -t {Table} --line-numbers -n");
        var rulesArray = printRulesCommand.Execute().Split('\n');

        Client.Disconnect();
        return Rule.ParseList(rulesArray);
    }
    public void DeleteRule(int ruleId)
    {
        Client.Connect();
        SshCommand delRuleCommand = Client.CreateCommand($"iptables -t {Table} -D {Chain} {ruleId}");
        delRuleCommand.Execute();
        delRuleCommand.Error.Contains("Index of deletion too big").Throw(_=> new ArgumentException($"Правило c номером {ruleId} не найдено.")).IfTrue();
        Client.Disconnect();
    }
}

