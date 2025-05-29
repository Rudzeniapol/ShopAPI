using System;

namespace ClientsService.Application.Services
{
    public class GuidGeneratorSingleton
    {
        private static GuidGeneratorSingleton _instance;
        private static readonly object _lock = new object();

        public static GuidGeneratorSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GuidGeneratorSingleton();
                        }
                    }
                }
                return _instance;
            }
        }

        private GuidGeneratorSingleton() { }

        public Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}
