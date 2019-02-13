using Jellyfin.Plugin.DateMagician.Shared;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.DateMagician.Tasks
{
    public class DateMagicianScheduledTask : IScheduledTask
    {
        private readonly ILibraryManager libraryManager;
        private readonly IProviderManager providerManager;
        private readonly ILogger logger;
        public DateMagicianScheduledTask(ILogger logger, ILibraryManager libraryManager, IProviderManager providerManager)
        {
            this.logger = logger;
            this.libraryManager = libraryManager;
            this.providerManager = providerManager;
        }

        public string Name => "Update AddedDate";

        public string Key => "DateMagician";

        public string Description => "DateMagician sets the TV-Shows added-Date in your Library to the original aired date.";

        public string Category => "DateMagician";

        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
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

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return new[] { 
            
                // Every so often
                new TaskTriggerInfo { Type = TaskTriggerInfo.TriggerDaily, TimeOfDayTicks = 0 }
            };
        }
    }
}
