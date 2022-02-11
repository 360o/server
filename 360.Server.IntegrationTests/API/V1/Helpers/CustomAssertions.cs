using _360o.Server.Api.V1.Errors.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Helpers
{
    internal static class CustomAssertions
    {
        public static void AssertSerializeToSameJson(object expected, object actual)
        {
            var serializedExpected = JsonConvert.SerializeObject(expected);
            var serializedActual = JsonConvert.SerializeObject(actual);
            Assert.AreEqual(serializedExpected, serializedActual);
        }

        public static async Task AssertNotFoundWithProblemDetailsAsync(HttpResponseMessage response, string detailMessage)
        {
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.NotFound.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.Status.Value);
            Assert.AreEqual(detailMessage, result.Detail);
        }

        public static async Task AssertBadRequestWithProblemDetailsAsync(HttpResponseMessage response, string containsDetailMessage)
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains(containsDetailMessage));
        }

        public static async Task AssertBadRequestAsync(HttpResponseMessage response, ISet<string> containsDetailMessages)
        {
            foreach (var message in containsDetailMessages)
            {
                await AssertBadRequestWithProblemDetailsAsync(response, message);
            }
        }
    }
}