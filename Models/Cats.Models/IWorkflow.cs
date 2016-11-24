namespace Cats.Models
{
    public interface IWorkflow
    {

         int BusinessProcessId { get; set; }

         BusinessProcess BusinessProcess { get; set; }
    }
}