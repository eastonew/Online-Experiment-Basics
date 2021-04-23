using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainEnvironment.Core.Services
{

    public class LogService : ILogService
    {
        #region Enums
        [Flags]
        public enum LogMode
        {
            Local,
            External
        }
        #endregion

        public static LogService Instance;

        #region Event Handlers
        public delegate void DisposeCompletedEventHandler();
        public event DisposeCompletedEventHandler OnDisposeCompleted;

        public delegate void LogsAvailableEventHandler(ILogModel log);
        public event LogsAvailableEventHandler OnLogsAvailable;
        #endregion

        #region Private Properties
        private string ApiHost;
        private LogMode CurrentLogMode;
        private Queue<ILogModel> _logQueue = new Queue<ILogModel>();
        private List<Exception> _failures = new List<Exception>();
        private static object _flagLock = new object();
        private static object _queueLock = new object();
        private static object _listLock = new object();

        private bool _Disposing;
        private bool Dispose
        {
            get
            {
                bool dispose = false;
                lock (_flagLock)
                {
                    dispose = _Disposing;
                }
                return dispose;
            }
            set
            {
                lock (_flagLock)
                {
                    _Disposing = value;
                }
            }
        }
        #endregion

        #region Constructor
        private LogService()
        {
            Thread backgroundTask = new Thread(async () =>
            {
                while (!Dispose)
                {
                    int count = 0;
                    List<ILogModel> logs = new List<ILogModel>();
                    lock (_queueLock)
                    {
                        count = _logQueue.Count;
                        while (count > 0)
                        {
                            logs.Add(_logQueue.Dequeue());
                            count--;
                        }
                    }
                    //send the logs to the correct place
                    if ((CurrentLogMode & LogMode.External) == LogMode.External)
                    {
                        await SaveLogs(logs);
                    }

                    if ((CurrentLogMode & LogMode.Local) == LogMode.Local)
                    {
                        PrintLogs(logs);
                    }
                    Thread.Sleep(10000);
                }
                OnDisposeCompleted?.Invoke();
            });
            backgroundTask.Start();
        }
        #endregion

        public static void Initialise(string host, LogMode loggingMode)
        {
            Instance = new LogService()
            {
                ApiHost = host ?? throw new ArgumentNullException("Services cannot be instantiated without an host e.g. https://test-service"),
                CurrentLogMode = loggingMode
            };
        }

        public void AddLog(ILogModel log)
        {
            lock (_queueLock)
            {
                _logQueue.Enqueue(log);
            }
        }
        public void TerminateLogger()
        {
            this.Dispose = true;
        }

        private async Task SaveLogs(List<ILogModel> logsToSave)
        {
            if (logsToSave != null && logsToSave.Count > 0)
            {
                string url = $"{ApiHost}/api/log/bulk";
                LogRequest req = new LogRequest()
                {
                    Logs = logsToSave
                };

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string json = JsonConvert.SerializeObject(req, Formatting.Indented);
                        var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            //fallback method of writing logs to a file 
                        }
                        else if (response.StatusCode == HttpStatusCode.Created)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    lock (_listLock)
                    {
                        _failures.Add(ex);
                    }
                }
            }
        }

        public void PrintLogs(List<ILogModel> logsToSave)
        {
            if (logsToSave != null && logsToSave.Count > 0)
            {
                for (int i = 0; i < logsToSave.Count; i++)
                {
                    ILogModel item = logsToSave[i];
                    if (item != null)
                    {
                        try
                        {
                            OnLogsAvailable?.Invoke(item);
                        }
                        catch (Exception ex)
                        {
                            lock (_listLock)
                            {
                                _failures.Add(ex);
                            }
                        }
                    }
                }
            }
        }
    }
}
