using System.Diagnostics;
using System.Security.Principal;

namespace EnhancePoE.App.Helpers
{
    /// <summary>
    /// Represents the admin request helper.
    /// </summary>
    public static class AdminRequestHelper
    {
        #region Methods

        /// <summary>
        /// Requests the admin.
        /// </summary>
        /// <returns>If request admin are requested.</returns>
        public static bool RequestAdmin()
        {
            if (IsAdministrator())
            {
                return false;
            }

            // Restart program and run as admin
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            var startInfo = new ProcessStartInfo(exeName)
            {
                Verb = "runas"
            };
            Process.Start(startInfo);
            System.Windows.Application.Current.Shutdown();

            return true;
        }

        /// <summary>
        /// Determines whether this instance is administrator.
        /// </summary>
        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        #endregion
    }
}