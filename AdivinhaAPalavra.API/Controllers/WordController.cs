using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdivinhaAPalavra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        Models.Match _match = new Models.Match();

        public string Get()
        {
            return _match.createMatch();
        }

        [Route("word/{token}")]
        public string GetWord(string token)
        {
            return _match.getWord(token);
        }

        [Route("word/check/{word}")]
        public bool CheckWord(string word)
        {
            return _match.checkWord(word);
        }

        [Route("finish")]
        [HttpDelete]
        public bool Finish(string token)
        {
            return _match.finish(token);
        }
    }
}
