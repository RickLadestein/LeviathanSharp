using Leviathan.Util.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leviathan.ECS.Systems
{
    public class DelayedExecutionSystem : ECSystem
    {
        public override string FriendlyName => "DelayedExecutionSystem";

        public override SystemPriority Priority => SystemPriority.POST_RENDER;

        private Queue<Job> jobs;
        private Mutex job_mutex;

        public DelayedExecutionSystem() : base()
        {
            jobs = new Queue<Job>();
            job_mutex = new Mutex();
        }

        public void AddJob(Job job)
        {
            this.jobs.Enqueue(job);
        }

        protected override void SystemFunc()
        {
            bool mutex_aquired = false;
            Job job = null;
            try
            {
                job_mutex.WaitOne();
                mutex_aquired = true;
                while(jobs.Count != 0)
                {
                    job = jobs.Dequeue();
                    if(job == null) { continue; }
                    job.job.Invoke(job.jobResultHandler);
                }
            }
            catch(Exception exception)
            {
                job.jobResultHandler.JobFailed(exception.Message);
            }
            finally
            {
                if (mutex_aquired)
                {
                    job_mutex.ReleaseMutex();
                }
            }
        }
    }
}
