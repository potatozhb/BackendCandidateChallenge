using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using QuizService.Model;
using Xunit;

namespace QuizService.Tests;

public class QuizzesControllerTest
{
    const string QuizApiEndPoint = "/api/quizzes/";
    const string QuestionApiEndPoint = "/questions/";
    const string AnswerApiEndPoint = "/answers/";

    [Fact]
    public async Task PostNewQuizAndTakeTheQuiz()
    {
        var quiz = new QuizCreateModel("Test quiz");
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(quiz));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var quizrsp = await client.PostAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}"),
                content);
            Assert.Equal(HttpStatusCode.Created, quizrsp.StatusCode);
            Assert.NotNull(quizrsp.Headers.Location);

            var expectedans = new List<int>();
            for (int i = 1; i < 3; i++)
            {
                var q = new QuestionCreateModel("question" + i);
                var jsonq = new StringContent(JsonConvert.SerializeObject(q));
                jsonq.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               
                var question = await client.PostAsync(new Uri(testHost.BaseAddress, $"{quizrsp.Headers.Location}{QuestionApiEndPoint}"),
                    jsonq);
                Assert.Equal(HttpStatusCode.Created, question.StatusCode);
                Assert.NotNull(question.Headers.Location);

                var answers = new List<int>();
                for(int j=1;j<4;j++)
                {
                    var ans = new AnswerCreateModel("answer" + i + j);
                    var jsonans = new StringContent(JsonConvert.SerializeObject(ans));
                    jsonans.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var answer = await client.PostAsync(new Uri(testHost.BaseAddress, $"{question.Headers.Location}{AnswerApiEndPoint}"),
                        jsonans);
                    Assert.Equal(HttpStatusCode.Created, answer.StatusCode);
                    Assert.NotNull(answer.Headers.Location);
                    var arr = answer.Headers.Location.OriginalString.Split('/');
                    answers.Add(int.Parse(arr[arr.Length - 1]));
                }

                var qupdate = new QuestionUpdateModel()
                {
                    CorrectAnswerId = answers[0],
                    Text = q.Text,
                };
                expectedans.Add(answers[0]);

                var jsonqu = new StringContent(JsonConvert.SerializeObject(qupdate));
                jsonqu.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var arrq = question.Headers.Location.OriginalString.Split('/');
                int qid = int.Parse(arrq[arrq.Length - 1]);
                var qu = await client.PutAsync(new Uri(testHost.BaseAddress, $"{quizrsp.Headers.Location}{QuestionApiEndPoint}{qid}"),
                    jsonqu);
                Assert.Equal(HttpStatusCode.OK, qu.StatusCode);
            }

            var arrqu = quizrsp.Headers.Location.OriginalString.Split('/');
            int quid = int.Parse(arrqu[arrqu.Length - 1]);
            var quizfull = await client.GetAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quid}"));
            quizfull.EnsureSuccessStatusCode();
            var jsonString = await quizfull.Content.ReadAsStringAsync();
            var quizDecode = JsonConvert.DeserializeObject<QuizResponseModel>(jsonString);

            Assert.Equal(HttpStatusCode.OK, quizfull.StatusCode);
            int points = 0;
            var allquestions = quizDecode.Questions.ToArray();
            for(int i=0;i< allquestions.Length; i++)
            {
                if (allquestions[i].CorrectAnswerId == expectedans[i])
                    points++;
            }
            Assert.Equal(points, 2);
        }
    }


    [Fact]
    public async Task PostNewQuizAddsQuiz()
    {
        var quiz = new QuizCreateModel("Test title");
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(quiz));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}"),
                content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }
    }

    [Fact]
    public async Task AQuizExistGetReturnsQuiz()
    {
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            const long quizId = 1;
            var response = await client.GetAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quizId}"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            var quiz = JsonConvert.DeserializeObject<QuizResponseModel>(await response.Content.ReadAsStringAsync());
            Assert.Equal(quizId, quiz.Id);
            Assert.Equal("My first quiz", quiz.Title);
        }
    }

    [Fact]
    public async Task AQuizDoesNotExistGetFails()
    {
        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            const long quizId = 999;
            var response = await client.GetAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}{quizId}"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [Fact]
        
    public async Task AQuizDoesNotExists_WhenPostingAQuestion_ReturnsNotFound()
    {
        const string QuizApiEndPoint = "/api/quizzes/999/questions";

        using (var testHost = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>()))
        {
            var client = testHost.CreateClient();
            const long quizId = 999;
            var question = new QuestionCreateModel("The answer to everything is what?");
            var content = new StringContent(JsonConvert.SerializeObject(question));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(new Uri(testHost.BaseAddress, $"{QuizApiEndPoint}"),content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}