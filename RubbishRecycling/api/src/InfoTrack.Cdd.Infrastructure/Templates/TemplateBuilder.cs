using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using RazorLight;

namespace InfoTrack.Cdd.Infrastructure.Templates
{
    public class TemplateBuilder : ITemplateBuilder
    {
        private readonly ILogger<TemplateBuilder> _logger;

        public TemplateBuilder(ILogger<TemplateBuilder> logger)
        {
            _logger = logger;
        }

        public async Task<string> Build<TemplateModel>(TemplateModel model, string templateFile)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(templateFile))
            {
                throw new ArgumentNullException(nameof(templateFile));
            }

            try
            {
                // TODO consider taking advantage of template caching
                var engine = new RazorLightEngineBuilder()
                    .UseFileSystemProject($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Templates")
                    .UseMemoryCachingProvider()
                    .Build();

                return await engine.CompileRenderAsync<object>($"{templateFile}.cshtml", model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to generate {TemplateName} template", templateFile);
                throw;
            }
        }
    }
}
