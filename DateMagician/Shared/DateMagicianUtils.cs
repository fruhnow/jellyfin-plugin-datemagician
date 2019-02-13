using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using System.Collections.Generic;
using System.Linq;

namespace Jellyfin.Plugin.DateMagician.Shared
{
    public class DateMagicianUtils
    {
        internal static readonly string logPrefix = "DateMagician: ";

        public static void SetAddedDateToAiredDate(BaseItem item)
        {
            if (item.PremiereDate.HasValue)
            {
                item.DateCreated = item.PremiereDate.Value;
            }
        }

        public static IEnumerable<Episode> GetAllEpisodes(ILibraryManager libraryManager)
        {
            return libraryManager.GetItemList(new MediaBrowser.Controller.Entities.InternalItemsQuery()
            {
                IncludeItemTypes = new[] { typeof(Episode).Name },
                Recursive = true
            }).Cast<Episode>().ToList();
        }
    }
}
