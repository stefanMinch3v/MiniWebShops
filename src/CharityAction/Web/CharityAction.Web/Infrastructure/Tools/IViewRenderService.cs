namespace CharityAction.Web.Infrastructure.Tools
{
    using System.Threading.Tasks;

    internal interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
