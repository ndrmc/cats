namespace Cats.Models.Hubs
{
    public interface IWorkflowHub
    {

         int BusinessProcessId { get; set; }

         BusinessProcess BusinessProcess { get; set; }
    }
}