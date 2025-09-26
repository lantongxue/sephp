using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Share.Services.Interfaces
{
    public interface IConfigService<T> where T : new()
    {
        T Settings { get; }
        event Action<T>? OnChanged;

        void Load();
        void Save();
    }

}
