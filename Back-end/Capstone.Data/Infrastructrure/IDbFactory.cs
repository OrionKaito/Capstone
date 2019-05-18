using System;

namespace Capstone.Data.Infrastructrure
{
    public interface IDbFactory : IDisposable
    {
        CapstoneEntities Init();
    }
}
