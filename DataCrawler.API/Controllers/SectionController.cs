using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCrawler.Model;
using DataCrawler.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataCrawler.API.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]
    public class SectionController : BaseController
    {
        SectionRepository _SectionRepository;

        public SectionController(SectionRepository sectionRepository)
        {
            _SectionRepository = sectionRepository;
        }

        [HttpGet]
        public async Task<ResultNormal> Init()
        {
            return await _SectionRepository.Init();
        }
    }
}