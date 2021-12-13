using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeDataApiIssue.Models;

namespace YouTubeDataApiIssue.Pages
{
    public partial class Index
    {
        private readonly YouTubeService _Service;

        public string SearchTerm { get; set; }

        public List<SearchResult> SearchResults { get; set; }

        public Index()
        {
            _Service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "your-api-key",
                ApplicationName = "your-application-name",
                GZipEnabled = false
            });
        }

        public async Task Search()
        {
            var searchListRequest = _Service.Search.List("snippet");
            searchListRequest.Q = SearchTerm;
            searchListRequest.MaxResults = 5;
            searchListRequest.RelevanceLanguage = "de";
            searchListRequest.RegionCode = "de";
            searchListRequest.Type = "youtube#video";
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;

            var results = await searchListRequest.ExecuteAsync();

            SearchResults = (from r in results.Items
                             select new SearchResult
                             {
                                 Title = r.Snippet.Title,
                                 Teaser = r.Snippet.Description,
                                 TargetUrl = new Uri($"https://www.youtube.com/watch?v={r.Id?.VideoId}"),
                                 ThumbnailUrl = new Uri(r.Snippet.Thumbnails.Medium.Url)
                             }).ToList();
        }
    }
}
