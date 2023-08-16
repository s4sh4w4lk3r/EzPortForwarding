using EzPortForwardingLib;
using System.Text.RegularExpressions;

namespace EzPortForwardingGui
{
    public partial class RuleEditor : Form
    {
        public Rule? RuleToEdit { get; set; }

        public RuleEditor(Rule rule = null!)
        {
            InitializeComponent();
            RuleToEdit = rule;
        }

        private void SaveButton(object sender, EventArgs e)
        {
            bool sportOk = ushort.TryParse(textBox1.Text, out ushort sport);
            bool dportOk = ushort.TryParse(textBox3.Text, out ushort dport);

            string ip = textBox2.Text;
            var ipRegex = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            bool ipOk = ipRegex.IsMatch(ip);

            if ((ipOk && sportOk && dportOk) is false)
            {
                MessageBox.Show("Введены неправильные данные.");
                return;
            }

            if (comboBox1.SelectedItem.ToString() == "TCP")
            {
                RuleToEdit = new Rule(ForwardProtocol.TCP, sport, dport, ip);
            }
            if (comboBox1.SelectedItem.ToString() == "UDP")
            {
                RuleToEdit = new Rule(ForwardProtocol.UDP, sport, dport, ip);
            }
            Hide();
        }

        private void RuleEditor_Load(object sender, EventArgs e)
        {
            if (RuleToEdit is not null)
            {
                textBox1.Text = RuleToEdit.SourcePort.ToString();
                textBox2.Text = RuleToEdit.DestinationIP;
                textBox3.Text = RuleToEdit.DestinationtPort.ToString();
                comboBox1.SelectedItem = RuleToEdit.Protocol.ToString();
            }
        }
    }
}
