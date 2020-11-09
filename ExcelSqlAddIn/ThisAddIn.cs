using DatabaseManager.Interfaces;
using DatabaseManager.Models;
using DatabaseManager.Services;

namespace ExcelSqlAddIn
{
    public partial class ThisAddIn
    {
        string _connectionString;
        IUnityContainer unityContainer;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _connectionString = Properties.Settings.Default.ConnectionString;
            IDatabaseSettings databaseSettings = new DatabaseSettings
            {
                ConnectionString = _connectionString
            };
            unityContainer = new UnityContainer();
            unityContainer.RegisterInstance(databaseSettings);
            unityContainer.RegisterType<IDataBaseService, DatabaseService>();
            UnityServiceLocator unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region Von VSTO generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
