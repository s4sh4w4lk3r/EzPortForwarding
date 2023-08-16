namespace EzPortForwardingGui
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            groupBox1 = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(218, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(397, 349);
            listBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 193);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "SSH";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Green;
            label2.Location = new Point(6, 139);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 7;
            // 
            // label1
            // 
            label1.ForeColor = Color.Lime;
            label1.Location = new Point(6, 135);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(6, 109);
            button1.Name = "button1";
            button1.Size = new Size(188, 23);
            button1.TabIndex = 3;
            button1.Text = "Подключиться";
            button1.UseVisualStyleBackColor = true;
            button1.Click += ConnectButton;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(6, 80);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(188, 23);
            textBox3.TabIndex = 2;
            textBox3.Text = "Password";
            textBox3.UseSystemPasswordChar = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(6, 51);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(188, 23);
            textBox2.TabIndex = 1;
            textBox2.Text = "Username";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(6, 22);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(188, 23);
            textBox1.TabIndex = 0;
            textBox1.Text = "Hostname";
            // 
            // button2
            // 
            button2.Location = new Point(12, 211);
            button2.Name = "button2";
            button2.Size = new Size(194, 23);
            button2.TabIndex = 2;
            button2.Text = "Удалить правило";
            button2.UseVisualStyleBackColor = true;
            button2.Click += DeleteRuleAsync;
            // 
            // button3
            // 
            button3.Location = new Point(12, 240);
            button3.Name = "button3";
            button3.Size = new Size(194, 23);
            button3.TabIndex = 3;
            button3.Text = "Добавить правило";
            button3.UseVisualStyleBackColor = true;
            button3.Click += AddRuleAsync;
            // 
            // button4
            // 
            button4.Location = new Point(12, 269);
            button4.Name = "button4";
            button4.Size = new Size(194, 23);
            button4.TabIndex = 4;
            button4.Text = "Изменить правило";
            button4.UseVisualStyleBackColor = true;
            button4.Click += EditRuleAsync;
            // 
            // button5
            // 
            button5.Location = new Point(12, 298);
            button5.Name = "button5";
            button5.Size = new Size(194, 23);
            button5.TabIndex = 5;
            button5.Text = "Очистить правила";
            button5.UseVisualStyleBackColor = true;
            button5.Click += ClearRulesAsync;
            // 
            // button6
            // 
            button6.Location = new Point(12, 327);
            button6.Name = "button6";
            button6.Size = new Size(194, 23);
            button6.TabIndex = 6;
            button6.Text = "Обновить правила";
            button6.UseVisualStyleBackColor = true;
            button6.Click += UpdateRules;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(627, 373);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(groupBox1);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "EzPortForwardingGui";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private GroupBox groupBox1;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Label label1;
        private Label label2;
        private Button button6;
    }
}