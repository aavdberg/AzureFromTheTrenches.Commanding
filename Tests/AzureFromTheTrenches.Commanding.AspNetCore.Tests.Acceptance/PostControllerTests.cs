﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using AzureFromTheTrenches.Commanding.AspNetCore.Tests.Acceptance.Infrastructure;
using AzureFromTheTrenches.Commanding.AspNetCore.Tests.Acceptance.Web;
using AzureFromTheTrenches.Commanding.AspNetCore.Tests.Acceptance.Web.Commands.MockData;
using AzureFromTheTrenches.Commanding.AspNetCore.Tests.Acceptance.Web.Commands.Responses;
using Newtonsoft.Json;
using Xbehave;
using Xunit;

namespace AzureFromTheTrenches.Commanding.AspNetCore.Tests.Acceptance
{
    public class PostControllerTests : AbstractControllerTestBase
    {
        [Scenario]
        public void ShouldAddNewPost(string requestUrl, HttpResponseMessage response, Guid postId)
        {
            "Given a request to add a new post"
                .x(() => requestUrl = "/api/post");
            "When the API call is made"
                .x(async () =>
                {
                    const string json = "{ \"Title\": \"A new post\", \"Body\": \"With some new contenmt\" }";
                    response = await HttpClient.PostAsync(requestUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                });
            "Then the response is 200"
                .x(() => Assert.Equal(HttpStatusCode.OK, response.StatusCode));
            "And a GUID for the post is returned"
                .x(async () =>
                {
                    string guidAsString = await response.Content.ReadAsStringAsync();
                    postId = JsonConvert.DeserializeObject<Guid>(guidAsString);
                    Assert.NotEqual(Guid.Empty, postId);                    
                });
            "And the author ID has been set to the current user"
                .x(() =>
                {
                    Post post = Posts.Items.Values.Single(x => x.Id == postId);
                    Assert.Equal(post.AuthorId, Constants.UserId);
                });
        }

        [Scenario]
        public void ShouldGetSpecificPost(string requestUrl, HttpResponseMessage response, Post post)
        {
            "Given a request for a specific post"
                .x(() => requestUrl = $"/api/post/{Constants.PresetPostId}");
            "When the API call is made"
                .x(async () =>
                {
                    response = await HttpClient.GetAsync(requestUrl);
                });
            "Then the response is 200"
                .x(() => Assert.Equal(HttpStatusCode.OK, response.StatusCode));
            "And a post is returned"
                .x(async () =>
                {
                    string json = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<Post>(json);
                    Assert.NotNull(post);
                });
            "And the properties are as expected"
                .x(() =>
                {
                    Assert.NotEqual(Constants.UserId, post.AuthorId);
                    Assert.Equal("A preset post with a random author", post.Title);
                    Assert.Equal("Some text for the post", post.Body);
                });
        }
    }
}
