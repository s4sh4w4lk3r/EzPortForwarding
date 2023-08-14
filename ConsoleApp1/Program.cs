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
string table = "nat";




using (var client = new SshClient(hostname, username, password))
{
    var ipTables = new IpTables(table, chain, client);
    /*ipTables.AddForwardingRule(33333, "192.168.1.192", 2222, ForwardProtocol.Both);*/
    var rules = ipTables.GetRules();
    ipTables.DeleteRule(1);
    ipTables.DeleteRule(1);
}