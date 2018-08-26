namespace SP_Server
{
    partial class Frm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listviewLog = new System.Windows.Forms.ListView();
            this.colDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFunc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSend = new System.Windows.Forms.Button();
            this.CB_AutoScroll = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 361);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(457, 25);
            this.textBox1.TabIndex = 2;
            // 
            // listviewLog
            // 
            this.listviewLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listviewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDesc,
            this.colFunc,
            this.colFile,
            this.colDate});
            this.listviewLog.FullRowSelect = true;
            this.listviewLog.GridLines = true;
            this.listviewLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listviewLog.Location = new System.Drawing.Point(12, 12);
            this.listviewLog.MultiSelect = false;
            this.listviewLog.Name = "listviewLog";
            this.listviewLog.Size = new System.Drawing.Size(760, 331);
            this.listviewLog.TabIndex = 4;
            this.listviewLog.UseCompatibleStateImageBehavior = false;
            this.listviewLog.View = System.Windows.Forms.View.Details;
            // 
            // colDesc
            // 
            this.colDesc.Text = "DESC";
            // 
            // colFunc
            // 
            this.colFunc.Text = "Function";
            // 
            // colFile
            // 
            this.colFile.Text = "File";
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(566, 361);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 25);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.OnBtnSend);
            // 
            // CB_AutoScroll
            // 
            this.CB_AutoScroll.AutoSize = true;
            this.CB_AutoScroll.Checked = true;
            this.CB_AutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_AutoScroll.Location = new System.Drawing.Point(12, 366);
            this.CB_AutoScroll.Name = "CB_AutoScroll";
            this.CB_AutoScroll.Size = new System.Drawing.Size(85, 16);
            this.CB_AutoScroll.TabIndex = 7;
            this.CB_AutoScroll.Text = "Auto Scroll";
            this.CB_AutoScroll.UseVisualStyleBackColor = true;
            this.CB_AutoScroll.CheckedChanged += new System.EventHandler(this.CB_AutoScroll_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(672, 361);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 25);
            this.button1.TabIndex = 8;
            this.button1.Text = "초기화";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnBtnDataInit);
            // 
            // Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 402);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CB_AutoScroll);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.listviewLog);
            this.Controls.Add(this.textBox1);
            this.MaximumSize = new System.Drawing.Size(800, 440);
            this.MinimumSize = new System.Drawing.Size(800, 440);
            this.Name = "Frm";
            this.Text = "SP_SERVER";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listviewLog;        
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.CheckBox CB_AutoScroll;
        public System.Windows.Forms.ColumnHeader colDesc;
        public System.Windows.Forms.ColumnHeader colFunc;
        public System.Windows.Forms.ColumnHeader colFile;
        public System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.Button button1;
    }
}

