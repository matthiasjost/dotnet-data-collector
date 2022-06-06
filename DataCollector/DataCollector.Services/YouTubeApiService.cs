using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace DataCollector.Services
{
    public class YouTubeApiService
    {
        public YouTubeApiService()
        {


        }
        public void GetVideo()
        {
            try
            {
        
                var youTubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyBAlcvuUyXwLQf_bEX6Sh4Cj - EAcZVNfDg" });

 
                var channelsListRequest = youTubeService.Channels.List("contentDetails");
                channelsListRequest.ForUsername = "binarythistle";

                var channelsListResponse = channelsListRequest.Execute();

                foreach (var channel in channelsListResponse.Items)
                {
                    // of videos uploaded to the authenticated user's channel.

                    var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

                    var nextPageToken = "";

                    while (nextPageToken != null)
                    {
                        var playlistItemsListRequest = youTubeService.PlaylistItems.List("snippet");

                        playlistItemsListRequest.PlaylistId = uploadsListId;
                        playlistItemsListRequest.MaxResults = 50;
                        playlistItemsListRequest.PageToken = nextPageToken;

                        // Retrieve the list of videos uploaded to the authenticated user's channel.
                        var playlistItemsListResponse = playlistItemsListRequest.Execute();
                        foreach (var playlistItem in playlistItemsListResponse.Items)
                        {

            
                            Console.WriteLine($"Thumbnails: '{playlistItem.Snippet.Thumbnails.Medium.Url}'");
                            Console.WriteLine($"PublishedAt: '{playlistItem.Snippet.PublishedAt}'");
                            Console.WriteLine($"Title: '{playlistItem.Snippet.Title}'");

                        }

                        nextPageToken = playlistItemsListResponse.NextPageToken;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
