namespace Cats.Models.Hubs
{
    public interface IWorkflow
    {

         int BusinessProcessId { get; set; }

         BusinessProcess BusinessProcess { get; set; }
    }
}