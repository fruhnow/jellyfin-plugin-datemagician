using Jellyfin.Plugin.DateMagician.Shared;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.DateMagician.Tasks
{
    public class DateMagicianLibraryPostScanTask : ILibraryPostScanTask
    {
        private readonly ILibraryManager libraryManager;
        private readonly IProviderManager providerManager;
        private readonly ILogger logger;
        public DateMagicianLibraryPostScanTask(ILogger logger, ILibraryManager libraryManager, IProviderManager providerManager)
        {
            this.logger = logger;
            this.libraryManager = libraryManager;
            this.providerManager = providerManager;
        }

        public async Task Run(IProgress<double> progress, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting DateMagician magic.");
            var result = DateMagicianUtils.GetAllEpisodes(libraryManager);
            int numComplete = 0;
            foreach (Episode item in result)
            {
                logger.LogDebug("{0}Episode: {1}", DateMagicianUtils.logPrefix, item.Name);
                DateMagicianUtils.SetAddedDateToAiredDate(item);
                providerManager.SaveMetadata(item, ItemUpdateType.MetadataEdit);
                logger.LogDebug("{0} Created Date of Episode: {1} set to {3}", DateMagicianUtils.logPrefix, item.Name, item.PremiereDate);
                numComplete++;
                double percent = numComplete;
                percent /= result.Count();
                progress.Report(100 * percent);
            }
            logger.LogInformation("Ending DateMagician magic.");
        }
    }
}
