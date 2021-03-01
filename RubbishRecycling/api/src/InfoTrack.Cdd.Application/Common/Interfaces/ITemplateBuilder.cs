using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    public interface ITemplateBuilder
    {
        Task<string> Build<TemplateModel>(TemplateModel model, string templateFile);
    }
}
