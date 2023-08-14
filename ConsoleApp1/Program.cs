using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System.Net.NetworkInformation;


var conf = new ConfigurationBuilder().AddJsonFile(@"C:\Users\sanchous\Desktop\epf.json").Build();
string hostname = conf.GetSection("hostname").Value!;
string username = conf.GetSection("username").Value!;
string password = conf.GetSection("password").Value!;
string chain = "vserver";




using (var client = new SshClient(hostname, username, password))
{
    /*Class1.AddForwardingRule(client, 12345, "192.168.1.181", 54321, ForwardProtocol.Both);*/
    var p = new Class1(chain, client);
    p.PrintRules();
}