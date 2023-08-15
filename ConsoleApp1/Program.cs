using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;

var conf = new ConfigurationBuilder().AddJsonFile(@"C:\Users\sanchous\Desktop\epf.json").Build();
string hostname = conf.GetSection("hostname").Value!;
string username = conf.GetSection("username").Value!;
string password = conf.GetSection("password").Value!;
string chain = "vserver";
string table = "nat";




using (var client = new SshClient(hostname, username, password))
{
    var ipTables = new IpTables(table, chain, client);
    Rule rule1 = new Rule(ForwardProtocol.TCP, 11111, 43121, "192.168.1.183");
    Rule rule2 = new Rule(ForwardProtocol.TCP, 22222, 4321, "192.168.1.181");
    Rule rule3 = new Rule(ForwardProtocol.UDP, 33333, 43231, "192.168.1.180");
    Rule rule4 = new Rule(ForwardProtocol.TCP, 44444, 41321, "192.168.1.182");
    ipTables.AddRule(rule1);
    ipTables.AddRule(rule2);
    ipTables.AddRule(rule3);
    ipTables.AddRule(rule4);
    ipTables.DeleteRule(rule2);
    ipTables.DeleteRule(rule4);

}