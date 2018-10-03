namespace Ecard.Models
{
    public interface ICommand
    {
        int Execute(User user);
        int Validate();
    } 
}