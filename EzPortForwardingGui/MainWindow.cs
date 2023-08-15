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
        try
        {
            textBox1.Text = Properties.Settings.Default.Hostname;
            textBox2.Text = Properties.Settings.Default.Username;
            textBox3.Text = Properties.Settings.Default.Password;
            ConnectSsh();
            _ = UpdateListBoxAsync();
        }
        catch (Exception)
        {
            return;
        }
    }
    private async Task UpdateListBoxAsync()
    {
        listBox1.DataSource = await Task.Run(() => ipTables.ToList());
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

    private async void DeleteRuleAsync(object sender, EventArgs e)
    {
        var rule = listBox1.SelectedItem as RuleWithId;
        if (rule is null) { return; }
        await ipTables.DeleteRuleAsync(rule);
        await Task.Delay(TimeSpan.FromSeconds(3));
        await UpdateListBoxAsync();
    }

    private async void UpdateRules(object sender, EventArgs e)
    {
        await UpdateListBoxAsync();
    }

    private async void ClearRulesAsync(object sender, EventArgs e)
    {
        await ipTables.ClearRulesAsync();
        await Task.Delay(TimeSpan.FromSeconds(3));
        await UpdateListBoxAsync();
    }

    private async void AddRuleAsync(object sender, EventArgs e)
    {
        var ruleEdtiorForm = new RuleEditor();
        ruleEdtiorForm.ShowDialog();
        var rule = ruleEdtiorForm.RuleToEdit;
        if (rule is null) { return; }
        ruleEdtiorForm.Dispose();
        await ipTables.AddRuleAsync(rule);
        await Task.Delay(TimeSpan.FromSeconds(3));
        await UpdateListBoxAsync();
    }

    private async void EditRuleAsync(object sender, EventArgs e)
    {
        var oldRule = listBox1.SelectedItem as RuleWithId;
        if (oldRule is null) { return; }
        var ruleEdtiorForm = new RuleEditor();
        ruleEdtiorForm.ShowDialog();
        var newRule = ruleEdtiorForm.RuleToEdit;
        if (newRule is null) { return; }
        ruleEdtiorForm.Dispose();
        await ipTables.DeleteRuleAsync(oldRule!).ContinueWith(_=>ipTables.AddRuleAsync(newRule));
        await Task.Delay(TimeSpan.FromSeconds(3));
        await UpdateListBoxAsync();
#warning не рабоает обновление
    }
}