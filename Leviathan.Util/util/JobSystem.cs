using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leviathan.Util.util
{
    public class JobSystem : IJobRequestListener, IDisposable
    {
        public List<JobHandler> threads { get; private set; }
        public Queue<Job> jobs;
        private Mutex job_mutex;


        public JobSystem(uint _thread_count)
        {
            job_mutex = new Mutex();
            jobs = new Queue<Job>();
            if(_thread_count == 0)
            {
                throw new ArgumentException("The amount of threads in a jobsystem cannot be zero: idiot");
            }
            threads = new List<JobHandler>((int)_thread_count);
            SpawnThreads(_thread_count);
        }

        public void AddJob(Job job)
        {
            this.jobs.Enqueue(job);
        }

        public Job RequestJob()
        {
            bool handle_aquired = false;
            try
            {
                job_mutex.WaitOne();
                handle_aquired = true;
                if(jobs.Count == 0)
                {
                    return null;
                } else
                {
                    return jobs.Dequeue();
                }
            } finally
            {
                if (handle_aquired)
                {
                    job_mutex.ReleaseMutex();
                }
            }
        }

        private void SpawnThreads(uint count)
        {
            for(uint i = 0; i < count; i++)
            {
                JobHandler handler = new JobHandler(this);
                threads.Add(handler);
            }
        }

        public void StopThreads()
        {
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Stop();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Dispose();
            }
        }
    }

    public class JobHandler : IDisposable
    {
        public Guid Id { get; private set; }
        private IJobRequestListener listener;
        private Thread t_handle;
        public bool stoprequested;

        public JobHandler(IJobRequestListener _listener)
        {
            this.Id = Guid.NewGuid();
            this.listener = _listener;
            this.t_handle = new Thread(new ThreadStart(() => { Run(); }));
            t_handle.Start();
        }

        public void Stop()
        {
            stoprequested = true;
        }

        protected void Run()
        {
            while(!stoprequested)
            {
                Job job = listener.RequestJob();
                try
                {
                    if (job == null)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        job.job.Invoke(job.jobResultHandler);
                    }
                } catch(Exception ex)
                {
                    if(job!= null)
                    {
                        job.jobResultHandler.JobFailed.Invoke(ex.Message); 
                    }
                }
                
            }
        }

        public void Dispose()
        {
            this.Stop();
            this.t_handle.Join();
        }
    }

    public class Job
    {
        public Guid TaskID { get; private set; }
        public Action<JobResultHandler> job;
        public JobResultHandler jobResultHandler;

        public Job(Action<JobResultHandler> _job, JobResultHandler _jobResultListener)
        {
            TaskID = Guid.NewGuid();
            job = _job;
            jobResultHandler = _jobResultListener;
        }
    }

    public class JobResultHandler
    {
        public Action JobSucces { get; private set; }
        public Action<string> JobFailed { get; private set; }

        public JobResultHandler(Action jobSucces, Action<string> jobFailed)
        {
            JobSucces = jobSucces;
            JobFailed = jobFailed;
        }
    }

    public interface IJobRequestListener
    {
        public Job RequestJob();
    }
}
