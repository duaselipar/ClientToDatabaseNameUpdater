namespace ClientToDatabaseUpdater
{
    partial class NameUpdater
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
        private System.Windows.Forms.TextBox txtClientPath;
        private System.Windows.Forms.Button btnFindClient;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnNpcIniToDb;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NameUpdater));
            txtClientPath = new TextBox();
            btnFindClient = new Button();
            txtHost = new TextBox();
            txtPort = new TextBox();
            txtUser = new TextBox();
            txtPassword = new TextBox();
            txtDatabase = new TextBox();
            btnConnect = new Button();
            lblStatus = new Label();
            btnNpcIniToDb = new Button();
            btnMnstrToDb = new Button();
            btnItemToDb = new Button();
            btnMgcToDb = new Button();
            btnMapToDb = new Button();
            groupBox1 = new GroupBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // txtClientPath
            // 
            txtClientPath.Location = new Point(95, 19);
            txtClientPath.Name = "txtClientPath";
            txtClientPath.PlaceholderText = "EO Client Folder Path";
            txtClientPath.Size = new Size(413, 23);
            txtClientPath.TabIndex = 0;
            // 
            // btnFindClient
            // 
            btnFindClient.Location = new Point(514, 19);
            btnFindClient.Name = "btnFindClient";
            btnFindClient.Size = new Size(100, 23);
            btnFindClient.TabIndex = 1;
            btnFindClient.Text = "Find Client";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(107, 58);
            txtHost.Name = "txtHost";
            txtHost.PlaceholderText = "Host";
            txtHost.Size = new Size(115, 23);
            txtHost.TabIndex = 2;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(297, 60);
            txtPort.Name = "txtPort";
            txtPort.PlaceholderText = "Port";
            txtPort.Size = new Size(115, 23);
            txtPort.TabIndex = 3;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(107, 86);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "User";
            txtUser.Size = new Size(115, 23);
            txtUser.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(297, 86);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(115, 23);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtDatabase
            // 
            txtDatabase.Location = new Point(497, 60);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.PlaceholderText = "Database";
            txtDatabase.Size = new Size(115, 23);
            txtDatabase.TabIndex = 6;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(497, 85);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(115, 23);
            btnConnect.TabIndex = 7;
            btnConnect.Text = "Connect";
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 240);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(400, 20);
            lblStatus.TabIndex = 9;
            lblStatus.Text = "Status: Disconnected";
            // 
            // btnNpcIniToDb
            // 
            btnNpcIniToDb.Location = new Point(19, 31);
            btnNpcIniToDb.Name = "btnNpcIniToDb";
            btnNpcIniToDb.Size = new Size(176, 30);
            btnNpcIniToDb.TabIndex = 10;
            btnNpcIniToDb.Text = "NPC.ini to Database";
            // 
            // btnMnstrToDb
            // 
            btnMnstrToDb.Location = new Point(216, 31);
            btnMnstrToDb.Name = "btnMnstrToDb";
            btnMnstrToDb.Size = new Size(176, 30);
            btnMnstrToDb.TabIndex = 11;
            btnMnstrToDb.Text = "Monster.fdb to Database";
            btnMnstrToDb.UseVisualStyleBackColor = true;
            // 
            // btnItemToDb
            // 
            btnItemToDb.Location = new Point(418, 31);
            btnItemToDb.Name = "btnItemToDb";
            btnItemToDb.Size = new Size(176, 30);
            btnItemToDb.TabIndex = 12;
            btnItemToDb.Text = "Itemtype.fdb to Database";
            btnItemToDb.UseVisualStyleBackColor = true;
            // 
            // btnMgcToDb
            // 
            btnMgcToDb.Location = new Point(19, 67);
            btnMgcToDb.Name = "btnMgcToDb";
            btnMgcToDb.Size = new Size(176, 30);
            btnMgcToDb.TabIndex = 13;
            btnMgcToDb.Text = "Magictype.fdb to Database";
            btnMgcToDb.UseVisualStyleBackColor = true;
            // 
            // btnMapToDb
            // 
            btnMapToDb.Location = new Point(216, 67);
            btnMapToDb.Name = "btnMapToDb";
            btnMapToDb.Size = new Size(176, 30);
            btnMapToDb.TabIndex = 14;
            btnMapToDb.Text = "GameMap.Ini to Database";
            btnMapToDb.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnNpcIniToDb);
            groupBox1.Controls.Add(btnMapToDb);
            groupBox1.Controls.Add(btnMnstrToDb);
            groupBox1.Controls.Add(btnMgcToDb);
            groupBox1.Controls.Add(btnItemToDb);
            groupBox1.Location = new Point(12, 126);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(614, 111);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tools";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 60);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 16;
            label1.Text = "Hostname/IP :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(65, 89);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 17;
            label2.Text = "User :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(256, 63);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 18;
            label3.Text = "Port :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(228, 89);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 19;
            label4.Text = "Password :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(430, 63);
            label5.Name = "label5";
            label5.Size = new Size(61, 15);
            label5.TabIndex = 20;
            label5.Text = "Database :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 23);
            label6.Name = "label6";
            label6.Size = new Size(71, 15);
            label6.TabIndex = 21;
            label6.Text = "Client Path :";
            // 
            // NameUpdater
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(642, 269);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Controls.Add(txtClientPath);
            Controls.Add(btnFindClient);
            Controls.Add(txtHost);
            Controls.Add(txtPort);
            Controls.Add(txtUser);
            Controls.Add(txtPassword);
            Controls.Add(txtDatabase);
            Controls.Add(btnConnect);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "NameUpdater";
            Text = "Client to Database Name Updater";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Button btnMnstrToDb;
        private Button btnItemToDb;
        private Button btnMgcToDb;
        private Button btnMapToDb;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}
