namespace ExcelSqlAddIn
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">"true", wenn verwaltete Ressourcen gelöscht werden sollen, andernfalls "false".</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für Designerunterstützung -
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.SqlAddin = this.Factory.CreateRibbonGroup();
            this.databaseSetupButton = this.Factory.CreateRibbonButton();
            this.sqTransferButton = this.Factory.CreateRibbonButton();
            this.dropTableButton = this.Factory.CreateRibbonButton();
            this.rowCorrectionDropDown = this.Factory.CreateRibbonDropDown();
            this.rowCounterLlabel = this.Factory.CreateRibbonLabel();
            this.tab1.SuspendLayout();
            this.SqlAddin.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.SqlAddin);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // SqlAddin
            // 
            this.SqlAddin.Items.Add(this.databaseSetupButton);
            this.SqlAddin.Items.Add(this.sqTransferButton);
            this.SqlAddin.Items.Add(this.dropTableButton);
            this.SqlAddin.Items.Add(this.rowCorrectionDropDown);
            this.SqlAddin.Items.Add(this.rowCounterLlabel);
            this.SqlAddin.Label = "SqlAddin";
            this.SqlAddin.Name = "SqlAddin";
            // 
            // databaseSetupButton
            // 
            this.databaseSetupButton.Label = "DatabaseSetup";
            this.databaseSetupButton.Name = "databaseSetupButton";
            this.databaseSetupButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.databaseSetupButton_Click);
            // 
            // sqTransferButton
            // 
            this.sqTransferButton.Label = "SqlTransfer";
            this.sqTransferButton.Name = "sqTransferButton";
            this.sqTransferButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.sqlTransferButton_Click);
            // 
            // dropTableButton
            // 
            this.dropTableButton.Label = "DropTable";
            this.dropTableButton.Name = "dropTableButton";
            this.dropTableButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropTableButton_Click);
            // 
            // rowCorrectionDropDown
            // 
            this.rowCorrectionDropDown.Label = "1st Data Row";
            this.rowCorrectionDropDown.Name = "rowCorrectionDropDown";
            this.rowCorrectionDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rowCorrectionDropDown_SelectionChanged);
            // 
            // rowCounterLlabel
            // 
            this.rowCounterLlabel.Label = "0";
            this.rowCounterLlabel.Name = "rowCounterLlabel";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.SqlAddin.ResumeLayout(false);
            this.SqlAddin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup SqlAddin;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton sqTransferButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton databaseSetupButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown rowCorrectionDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton dropTableButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel rowCounterLlabel;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
