using AuthSystem.Data;
using AuthSystem.DataModel;
using Azure.Core;
using CPMethod.DataModel.DTOs;
using CPMethod.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Text.Json;

namespace CPMethod.WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class CPMController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;

        private readonly ISvgService _svgService;
        private readonly IMemoryCache _memoryCache;


        public CPMController(
            ISvgService svgService,
            UserManager<User> userManager,
            AppDbContext dbContext,
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _svgService = svgService;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("svgs")]
        public ActionResult<SVGsResponse> GetSvgs()
        {
            if (!_memoryCache.TryGetValue("CPMSolution", out CPMSolution? solution))
                return NotFound();

            if (solution is null)
                return NotFound();

            if (solution.GanttResponse is null || 
                solution.CPMResponse is null ||
                solution.GanttResponse.svgs is null ||
                solution.CPMResponse.svg is null)
                return NotFound();

            SVGsResponse response = new SVGsResponse
            {
                alap = solution.GanttResponse.svgs.ALAP,
                asap = solution.GanttResponse.svgs.ASAP,
                diagram = solution.CPMResponse.svg
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("results")]
        public ActionResult<ResultsResponse> GetResults()
        {
            if (!_memoryCache.TryGetValue("CPMSolution", out CPMSolution? solution))
                return NotFound();

            if (solution is null)
                return NotFound();

            if (solution.CPMResponse is null ||
                solution.CPMResponse.nodes is null)
                return NotFound();

            List<Result> results = new List<Result>();

            foreach (Node node in solution.CPMResponse.nodes)
            {
                Result result = new Result
                {
                    task = node.name,
                    earliestStart = node.es,
                    earliestFinish = node.ef,
                    latestStart = node.ls,
                    latestFinish = node.lf,
                    slack = node.r,
                    isCritical = node.r == 0,
                    duration = node.t
                };

                results.Add(result);
            }

            ResultsResponse response = new ResultsResponse
            {
                results = results
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("SolutionRequest")]
        public async Task<ActionResult<CPMSolution>> PostSolutionRequest(CPMInput request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.tasks is null)
                return BadRequest();

            GanttResponse? ganttResponse = await getGantt(request.tasks);

            if (ganttResponse is null)
                return BadRequest();

            CPMResponse? CPMResponse = await getCPM(request.tasks);

            if (CPMResponse is null)
                return BadRequest();

            CPMSolution solution = new CPMSolution
            {
                GanttResponse = ganttResponse,
                CPMResponse = CPMResponse
            };

            _memoryCache.Set("CPMSolution", solution);

            return Ok(solution);
        }

        [AllowAnonymous]
        [HttpGet("svg/{id}")]
        public IActionResult GetSvgById(string id)
        {
            if (_memoryCache.TryGetValue(id, out string? svg))
            {
                if (string.IsNullOrEmpty(svg))
                    return NotFound();

                var bytes = Encoding.UTF8.GetBytes(svg);
                return File(bytes, "image/svg+xml", $"diagram_{id}.svg");
            }

            return NotFound();
        }

        private async Task<CPMResponse?> getCPM(Dictionary<string, CPMTask> tasks)
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://laptop.skaner.dev/")
            };

            HttpContent content = new StringContent(JsonSerializer.Serialize(tasks), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = content
            };

            using HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            CPMResponse? cpmResponse = await response.Content.ReadFromJsonAsync<CPMResponse>();

            if (cpmResponse is null)
                return null;

            cpmResponse.svg = _svgService.SaveSvgToFile(cpmResponse.svg);

            return cpmResponse;
        }

        private async Task<GanttResponse?> getGantt(Dictionary<string, CPMTask> tasks)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.febru.dev/api/")
            };

            GanttRequest ganttRequest = new GanttRequest
            {
                tasks = tasks
            };

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("gantt/", ganttRequest);

            if (!response.IsSuccessStatusCode)
                return null;

            GanttResponse? ganttResponse = await response.Content.ReadFromJsonAsync<GanttResponse>();

            return ganttResponse;
        }
    }
}
