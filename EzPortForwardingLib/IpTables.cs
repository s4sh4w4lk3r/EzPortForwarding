using Renci.SshNet;
using System.Collections;
using System.Data;
using Throw;

namespace EzPortForwardingLib;

public class IpTables : IEnumerable<Rule>
{
    public string Table { get; private set; }
    public string Chain { get; private set; }
    private SshClient Client { get; set; }

    public IpTables(string table, string chain, SshClient client)
    {
        chain.Throw(_ => throw new ArgumentException("Строковый параметр \"Chain\" является Null или содержит пустой символ.")).IfNullOrWhiteSpace(_ => _);
        table.Throw(_ => throw new ArgumentException("Строковый параметр \"Table\" является Null или содержит пустой символ.")).IfNullOrWhiteSpace(_ => _);
        client.ThrowIfNull(_ => throw new ArgumentNullException("\"Client\" является Null."));
        Chain = chain;
        Client = client;
        Table = table;
        EnsureConnection();
    }

    public void AddRule(Rule rule)
    {
        if (CheckRuleExist(rule) is true) { return; }

        ushort sourcePort = rule.SourcePort;
        string destinationIP = rule.DestinationIP;
        ushort destinationPort = rule.DestinationtPort;
        ForwardProtocol proto = rule.Protocol;

        Client.Connect();
        SshCommand addRuleCommand = null!;
        switch (proto)
        {
            case ForwardProtocol.TCP:
                addRuleCommand = Client.CreateCommand($"iptables -t {Table} -A {Chain} -p tcp --dport {sourcePort} -j DNAT --to-destination {destinationIP}:{destinationPort}");
                break;
            case ForwardProtocol.UDP:
                addRuleCommand = Client.CreateCommand($"iptables -t {Table} -A {Chain} -p udp --dport {sourcePort} -j DNAT --to-destination {destinationIP}:{destinationPort}");
                break;
        }
        addRuleCommand.Execute();
        Client.Disconnect();
        if (CheckRuleExist(rule) is false) { throw new Exception($"Правило {rule} не добавилось."); }
    }
    public bool DeleteRule(Rule rule)
    {
        var ruleWithId = CastRule(rule);

        if (ruleWithId is null) { return false; }

        int idToDel = ruleWithId.Id;
        DeleteRuleById(idToDel);

        if (CheckRuleExist(rule) is true) {return false;}
        else {return true;}
    }
    public bool ClearRules()
    {
        int count = GetRuleList().Count;
        for (int i = 0; i < count; i++)
        {
            DeleteRuleById(1);
        }
        if (GetRuleList().Count == 0) return true;
        else return false;
    }

    public async Task AddRuleAsync(Rule rule) => await Task.Run(() => AddRule(rule));
    public async Task<bool> DeleteRuleAsync(Rule rule) => await Task.Run(() => DeleteRule(rule));
    public async Task<bool> ClearRulesAsync() => await Task.Run(ClearRules);

    private void DeleteRuleById(int ruleId)
    {
        Client.Connect();
        SshCommand delRuleCommand = Client.CreateCommand($"iptables -t {Table} -D {Chain} {ruleId}");
        delRuleCommand.Execute();
        delRuleCommand.Error.Contains("Index of deletion too big").Throw(_=> new ArgumentException($"Правило c номером {ruleId} не найдено.")).IfTrue();
        Client.Disconnect();
    }
    private void EnsureConnection()
    {
        try
        {
            Client.Connect();
            Client.Disconnect();
        }
        catch (Exception)
        {
            string username = Client.ConnectionInfo.Username;
            string hostname = Client.ConnectionInfo.Host;
            throw new Exception($"Не удалось установить ssh соединение с {username}@{hostname}.");
        }
    }
    private List<RuleWithId> GetRuleList()
    {
        Client.Connect();

        var printRulesCommand = Client.CreateCommand($"iptables -L {Chain} -t {Table} --line-numbers -n");
        var rulesArray = printRulesCommand.Execute().Split('\n');

        Client.Disconnect();
        return RuleWithId.ParseList(rulesArray);
    }
    private RuleWithId? CastRule(Rule rule)
    {
        var actualList = GetRuleList();

        var ruleToDel = actualList.Where(ruleWithId => ruleWithId.SourcePort == rule.SourcePort &&
        ruleWithId.DestinationIP == rule.DestinationIP && ruleWithId.DestinationtPort == rule.DestinationtPort && ruleWithId.Protocol == rule.Protocol)
            .FirstOrDefault();

        return ruleToDel;
    }
    private bool CheckRuleExist(Rule rule)
    {
        RuleWithId? ruleWithId = CastRule(rule);
        if (ruleWithId is null) return false;
        else return true;
    }

    public IEnumerator<Rule> GetEnumerator() => GetRuleList().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetRuleList().GetEnumerator();
}