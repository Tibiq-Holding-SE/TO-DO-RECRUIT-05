using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerLite.Data;

namespace UnitTests
{
    public class Startup
    {
        public ServiceProvider ServiceProvider { get;}

        public Startup()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TaskContext>();
            serviceCollection.AddScoped<ITaskRepository, TaskRepository>();
            serviceCollection.AddScoped<IBackgroundRepository, BackgroundRepository>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
