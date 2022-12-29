using NarwianskiZakatek.Models;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Repositories
{
    public interface IDescriptionsService
    {
        Description GetByTitle(string title);
        void Update(DescriptionViewModel viewModel);
    }
}