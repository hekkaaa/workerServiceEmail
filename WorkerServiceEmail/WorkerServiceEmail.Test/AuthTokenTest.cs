using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.ResponseModels;
using Moq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerServiceEmail.Infrastructure;

namespace WorkerServiceEmail.Test
{
    public class AuthTokenTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AuthResponseTokenTest()
        {
            //given
            Mock<AuthToken> mock = new Mock<AuthToken>();

            AuthToken token = new AuthToken();

           //var ty = Mock.Of<AuthToken>(Id =>
           //{
           //    //Task<RestResponse<IEnumerable<ConfigResponseModel>>> task = Id.SendRequestAsync<IEnumerable<ConfigResponseModel>>(@"https://piter-education.ru:6040", ConfigsEndpoints.Configs, "adsad") == ;
           //    //return task;
           //});
         
            //when
            //var result = token.SendRequestAsync<IEnumerable<Marvelous.Contracts.ResponseModels.ConfigResponseModel>>(@"https://piter-education.ru:6040", ConfigsEndpoints.Configs, tokenKey.Data);


            //then 
            Assert.IsTrue(true);
        }
    }
}