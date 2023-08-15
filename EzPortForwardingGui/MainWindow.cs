using EzPortForwardingLib;
using Renci.SshNet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EzPortForwardingGui;

public partial class MainWindow : Form
{
    private IpTables ipTables = null!;
    private const string TABLE = "nat";
    private const string CHAIN = "vserver";

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        textBox1.Text = Properties.Settings.Default.Hostname;
        textBox2.Text = Properties.Settings.Default.Username;
        textBox3.Text = Properties.Settings.Default.Password;
        ConnectSsh();
    }
    private void UpdateListBox()
    {
        listBox1.DataSource = ipTables.ToList();
    }

    private void ConnectButton(object sender, EventArgs e)
    {
        ConnectSsh(saveRequired: true);
    }
    private void ConnectSsh(bool saveRequired = false)
    {
        string hostname = textBox1.Text;
        string username = textBox2.Text;
        string password = textBox3.Text;
        var sshClient = new SshClient(hostname, username, password);
        try
        {
            ipTables = new IpTables(TABLE, CHAIN, sshClient);
            label2.Text = $"Connected to\n{hostname}";
        }
        catch (Exception)
        {
            MessageBox.Show("Не удалось подключиться.");
            Close();
        }
        if (saveRequired is false) { return; }
        Properties.Settings.Default.Hostname = hostname;
        Properties.Settings.Default.Username = username;
        Properties.Settings.Default.Password = password;
        Properties.Settings.Default.Save();
    }
}