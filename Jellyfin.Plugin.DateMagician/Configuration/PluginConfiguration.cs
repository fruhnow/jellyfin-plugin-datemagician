using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.DateMagician.Configuration
{
    /// <summary>
    /// Class PluginConfiguration
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        public bool isEnabled { get; set; }

        public PluginConfiguration()
        {
            isEnabled = true;
        }

    }

}