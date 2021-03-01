namespace InfoTrack.Cdd.Application.Models
{
    public class TemplateModel<TModel>
    {
        public TModel DataModel { get; set; }

        public string ReportTitle { get; set; }

        public TemplateModel(TModel dataModel)
        {
            DataModel = dataModel;
        }
    }
}
