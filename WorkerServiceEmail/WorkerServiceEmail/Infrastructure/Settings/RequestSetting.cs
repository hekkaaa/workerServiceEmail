using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.ResponseModels;
using NLog;
using RestSharp;
using WorkerServiceEmail.Infrastructure.Logging;

namespace WorkerServiceEmail.Infrastructure
{
    public static class RequestSetting
    {
        public static RestResponse<IEnumerable<ConfigResponseModel>>? _settingApp = null;
        //public static Microsoft.Extensions.Logging.ILogger Logger1 = (Microsoft.Extensions.Logging.ILogger)LogManager.GetCurrentClassLogger();

        public static RestResponse<IEnumerable<ConfigResponseModel>> RequestSettingfromServer()
        {
            AuthToken token = new AuthToken();
            RestResponse<string> tokenKey = ReqestTokenfromServer(token);
            if (token is null)
            {
                //_runner.CriticalAction("Token key from server is NULL!");
                throw new ArgumentException("Token key null");
            }

            var result = token.SendRequestAsync<IEnumerable<Marvelous.Contracts.ResponseModels.ConfigResponseModel>>(@"https://piter-education.ru:6040", ConfigsEndpoints.Configs, tokenKey.Data);

            if (result.Result.Data?.Count() == 0) throw new ArgumentNullException("Response with settin from server is NULL");

            _settingApp = result.Result;
            return result.Result;
        }

        public static string ReturnValueByKey(string keys)
        {
            try
            {
                return _settingApp.Data.FirstOrDefault(x => x.Key == keys).Value.ToString();
            }
            catch(NullReferenceException ex)
            {
                return string.Empty;
            }
           
        }
        private static RestResponse<string> ReqestTokenfromServer(AuthToken token)
        {
            var tokenString = token.SendRequestAsync<string>(@"https://piter-education.ru:6042", $"{AuthEndpoints.ApiAuth}{AuthEndpoints.TokenForMicroservice}");
            return tokenString.Result;
        }
    }
}
