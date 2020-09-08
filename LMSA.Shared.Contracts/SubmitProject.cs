using System;

namespace LMSA.Shared.Contracts
{
    public interface ISubmitProject
    {
        int Id { get; }
        string Title { get; }
        string Description { get; }
    }
}
