using CommonServiceLocator;
using DatabaseManager.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelSqlAddIn
{
    public partial class DatabaseSetupForm : Form
    {
        IDataBaseService _databaseService;
        IDatabaseSettings _dataBaseSettings;
        public event EventHandler BusyEvent;
        public DatabaseSetupForm()
        {
            InitializeComponent();
            _databaseService = ServiceLocator.Current.GetInstance<IDataBaseService>();
            _dataBaseSettings = ServiceLocator.Current.GetInstance<IDatabaseSettings>();
        }

        private void OnBusyEvent(BusyEventArgs busyEventArgs)
        {
            BusyEvent?.Invoke(this, busyEventArgs);
        }

        private async void setButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SqlDataSource = sourceTextBox.Text;
            _dataBaseSettings.Source = sourceTextBox.Text;
            Properties.Settings.Default.SqlUserID = idTextBox.Text;
            _dataBaseSettings.Id = idTextBox.Text;
            Properties.Settings.Default.SqlPassword = passwordTextBox.Text;
            _dataBaseSettings.Password = passwordTextBox.Text;
            Properties.Settings.Default.ConnectionString = $"Data Source={_dataBaseSettings.Source};Initial Catalog={_dataBaseSettings.DatabaseName};Persist Security Info=True;User ID={_dataBaseSettings.Id};Password={_dataBaseSettings.Password}";
            _dataBaseSettings.ConnectionString = Properties.Settings.Default.ConnectionString;
            try
            {
                OnBusyEvent(new BusyEventArgs() { IsBusy = true });
                UseWaitCursor = true;
                await _databaseService.CreateDatabase();
                UseWaitCursor = false;
                OnBusyEvent(new BusyEventArgs() { IsBusy = false });
            }
            catch (Exception exception)
            {
                OnBusyEvent(new BusyEventArgs() { IsBusy = false });
                UseWaitCursor = false;
                MessageBox.Show(exception.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Invoke((MethodInvoker)delegate { Close(); });
        }

        private async void dropDatabaseButton_Click(object sender, EventArgs e)
        {
            _dataBaseSettings.Source = Properties.Settings.Default.SqlDataSource;
            _dataBaseSettings.Id = Properties.Settings.Default.SqlUserID;
            _dataBaseSettings.Password = Properties.Settings.Default.SqlPassword;
            try
            {
                await _databaseService.DropDatabase();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }
    }
}
