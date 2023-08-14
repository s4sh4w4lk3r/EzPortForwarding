using Renci.SshNet;
using System.Net;
using Throw;

namespace ConsoleApp1;

public class Class1
{
    public string Chain { get; private set; }
    private SshClient Client { get; set; }
    public Class1(string chain, SshClient client)
    {
        Client = client;
        chain.Throw(s => throw new ArgumentException("Chain parameter is NullOrEmptyOrWhiteSpace.")).IfNullOrWhiteSpace(_ => _);
        Chain = chain;
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
                addRuleCommand = Client.CreateCommand($"iptables -t nat -A {Chain} -p tcp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort}");
                break;
            case ForwardProtocol.UDP:
                addRuleCommand = Client.CreateCommand($"iptables -t nat -A {Chain} -p udp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort}");
                break;
            case ForwardProtocol.Both:
                addRuleCommand = Client.CreateCommand($"iptables -t nat -A {Chain} -p tcp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort} && " +
                    $"iptables -t nat -A {Chain} -p udp --dport {sourcePort} -j DNAT --to-destination {localIP}:{localPort}");
                break;
        }
        addRuleCommand.Execute();
        Client.Disconnect();

        PrintRules();
    }
    public void PrintRules() 
    {
        Client.Connect();

        var printRulesCommand = Client.CreateCommand($"iptables  -L {Chain} -t nat --line-numbers -n");
        string rules = printRulesCommand.Execute();

        Client.Disconnect();
        Console.WriteLine(rules);
    }
}

public enum ForwardProtocol { TCP, UDP, Both }