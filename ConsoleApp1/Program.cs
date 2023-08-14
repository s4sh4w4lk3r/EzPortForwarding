using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

var conf = new ConfigurationBuilder().AddJsonFile(@"C:\Users\sanchous\Desktop\epf.json").Build();
string hostname = conf.GetSection("hostname").Value!;
string username = conf.GetSection("username").Value!;
string password = conf.GetSection("password").Value!;
string chain = "vserver";




using (var client = new SshClient(hostname, username, password))
{
    /*Class1.AddForwardingRule(client, 12345, "192.168.1.181", 54321, ForwardProtocol.Both);*/
    var ipTables = new IpTables(chain, client);
    var rules = ipTables.PrintRules();
    var lines = rules.Split('\n');
    var a = Rule.Parse(lines);

}