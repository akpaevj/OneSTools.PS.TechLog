using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OneSTools.PS.TechLog
{
    class TjEvent
    {
        public DateTime DateTime{ get; set; } = DateTime.MinValue;
        public int Duration { get; set; } = 0;
        public string EventName { get; set; } = "";
        public int Level { get; set; } = 0;
        public Dictionary<string, string> Properties { get; internal set; } = new Dictionary<string, string>();

        public bool TryGetValue(string property, out string value)
            => Properties.TryGetValue(property, out value);

        public bool Contains(string property)
            => Properties.ContainsKey(property);

        public string this[string property]
        {
            get => Properties[property];
        }

        /// <summary>
        /// A number of the thread
        /// </summary>
        public string OSThread => Properties.GetValueOrDefault("OSThread", null);

        /// <summary>
        /// A name of the process
        /// </summary>
        public string Process => Properties.GetValueOrDefault("process", null);

        /// <summary>
        /// Время зависания процесса
        /// </summary>
        public string AbandonedTimestamp => Properties.GetValueOrDefault("abandonedTimestamp", null);

        /// <summary>
        /// Текстовое описание выполняемой операции во время загрузки конфигурации из файлов
        /// </summary>
        public string Action => Properties.GetValueOrDefault("Action", null);

        /// <summary>
        /// Имя администратора кластера или центрального сервера
        /// </summary>
        public string Admin => Properties.GetValueOrDefault("Admin", null);

        /// <summary>
        /// Имя администратора
        /// </summary>
        public string Administrator => Properties.GetValueOrDefault("Administrator", null);

        /// <summary>
        /// Адрес текущего процесса агента сервера системы «1С:Предприятие»
        /// </summary>
        public string AgentURL => Properties.GetValueOrDefault("agentURL", null);

        public string AlreadyLocked => Properties.GetValueOrDefault("AlreadyLocked", null);

        public string AppID => Properties.GetValueOrDefault("AppID", null);

        public string Appl => Properties.GetValueOrDefault("Appl", null);

        /// <summary>
        /// Уточнение требования назначения функциональности
        /// </summary>
        public string ApplicationExt => Properties.GetValueOrDefault("ApplicationExt", null);

        /// <summary>
        /// Количество попыток установки соединения с процессом, завершившихся ошибкой
        /// </summary>
        public long? Attempts => GetLongOrNull("Attempts");

        /// <summary>
        /// Среднее количество исключений за последние 5 минут по другим процессам
        /// </summary>
        public long? AvgExceptions => GetLongOrNull("AvgExceptions");

        /// <summary>
        /// Значение показателя Доступная память в момент вывода в технологический журнал
        /// </summary>
        public long? AvMem => GetLongOrNull("AvMem");

        /// <summary>
        /// Формирование индекса полнотекстового поиска выполнялось в фоновом процессе
        /// </summary>
        public string BackgroundJobCreated => Properties.GetValueOrDefault("BackgroundJobCreated", null);

        /// <summary>
        /// Размер в байтах тела запроса/ответа
        /// </summary>
        public long? Body => GetLongOrNull("Body");

        public int? CallID => GetIntOrNull("CallID");

        /// <summary>
        /// Количество обращений клиентского приложения к серверному приложению через TCP
        /// </summary>
        public int? Calls => GetIntOrNull("Calls");

        /// <summary>
        /// Описание проверяемого сертификата
        /// </summary>
        public string Certificate => Properties.GetValueOrDefault("certificate", null);

        /// <summary>
        /// Имя класса, в котором сгенерировано событие
        /// </summary>
        public string Class => Properties.GetValueOrDefault("Class", null);

        public int? CallWait => GetIntOrNull("callWait");

        /// <summary>
        /// Очищенный текст SQL запроса
        /// </summary>
        public string CleanSql => GetCleanSql(Sql);

        public string ClientComputerName => Properties.GetValueOrDefault("ClientComputerName", null);

        public int? ClientID => GetIntOrNull("ClientID");

        /// <summary>
        /// Номер основного порта кластера серверов
        /// </summary>
        public string Cluster => Properties.GetValueOrDefault("Cluster", null);

        public string ClusterID => Properties.GetValueOrDefault("ClusterID", null);

        public string ConnectionID => Properties.GetValueOrDefault("connectionID", null);

        /// <summary>
        /// Имя компоненты платформы, в которой сгенерировано событие
        /// </summary>
        public string Component => Properties.GetValueOrDefault("Component", null);

        /// <summary>
        /// Номер соединения с информационной базой
        /// </summary>
        public long? Connection => GetLongOrNull("Connection");

        /// <summary>
        /// Количество соединений, которым не хватило рабочих процессов
        /// </summary>
        public long? Connections => GetLongOrNull("Connections");

        /// <summary>
        /// Установленное максимальное количество соединений на один рабочий процесс
        /// </summary>
        public long? ConnLimit => GetLongOrNull("ConnLimit");

        /// <summary>
        /// Контекст исполнения
        /// </summary>
        public string Context
        {
            get => Properties.GetValueOrDefault("Context", null);
            set => Properties["Context"] = value;
        }

        /// <summary>
        /// Общий размер скопированных значений при сборке мусора
        /// </summary>
        public long? CopyBytes => GetLongOrNull("CopyBytes");

        /// <summary>
        /// Длительность вызова в микросекундах
        /// </summary>
        public long? CpuTime => GetLongOrNull("CpuTime");

        public int? CreateDump => GetIntOrNull("createDump");

        public string Cycles => Properties.GetValueOrDefault("Cycles", null);

        /// <summary>
        /// Количество исключений в процессе за последние 5 минут
        /// </summary>
        public long? CurExceptions => GetLongOrNull("CurExceptions");

        /// <summary>
        /// Путь к используемой базе данных
        /// </summary>
        public string Database => Properties.GetValueOrDefault("DataBase", null);

        /// <summary>
        /// Идентификатор соединения с СУБД внешнего источника данных
        /// </summary>
        public string DBConnID => Properties.GetValueOrDefault("DBConnID", null);

        /// <summary>
        /// Строка соединения с внешним источником данных
        /// </summary>
        public string DBConnStr => Properties.GetValueOrDefault("DBConnStr", null);

        /// <summary>
        /// Имя используемой копии базы данных
        /// </summary>
        public string DBCopy => Properties.GetValueOrDefault("DBCopy", null);

        /// <summary>
        /// Имя СУБД, используемой для выполнения операции, которая повлекла формирование данного события технологического журнала
        /// </summary>
        public string DBMS => Properties.GetValueOrDefault("DBMS", null);

        /// <summary>
        /// Строковое представление идентификатора соединения сервера системы «1С:Предприятие» с сервером баз данных в терминах сервера баз данных
        /// </summary>
        public string Dbpid => Properties.GetValueOrDefault("dbpid", null);

        /// <summary>
        /// Имя пользователя СУБД внешнего источника данных
        /// </summary>
        public string DBUsr => Properties.GetValueOrDefault("DBUsr", null);

        /// <summary>
        /// Список пар транзакций, образующих взаимную блокировку
        /// </summary>
        public string DeadlockConnectionIntersections => Properties.GetValueOrDefault("DeadlockConnectionIntersections", null);

        public DeadlockConnectionIntersectionsInfo DeadlockConnectionIntersectionsInfo 
            => string.IsNullOrEmpty(DeadlockConnectionIntersections) ? null : new DeadlockConnectionIntersectionsInfo(DeadlockConnectionIntersections);

        /// <summary>
        /// Пояснения к программному исключению
        /// </summary>
        public string Descr => Properties.GetValueOrDefault("Descr", null);

        /// <summary>
        /// Текст, поясняющий выполняемое действие
        /// </summary>
        public string Description => Properties.GetValueOrDefault("description", null);

        /// <summary>
        /// Назначенный адрес рабочего процесса
        /// </summary>
        public string DstAddr => Properties.GetValueOrDefault("DstAddr", null);

        /// <summary>
        /// Уникальный идентификатор назначенного рабочего процесса
        /// </summary>
        public string DstId => Properties.GetValueOrDefault("DstId", null);

        /// <summary>
        /// Системный идентификатор назначенного рабочего процесса
        /// </summary>
        public string DstPid => Properties.GetValueOrDefault("DstPid", null);

        /// <summary>
        /// Назначенное имя рабочего сервера
        /// </summary>
        public string DstSrv => Properties.GetValueOrDefault("DstSrv", null);

        public string DstClientID => Properties.GetValueOrDefault("DstClientID", null);

        public int? Err => GetIntOrNull("Err");

        public string Exception => Properties.GetValueOrDefault("Exception", null);

        public int? ExpirationTimeout => GetIntOrNull("expirationTimeout");

        public int? FailedJobsCount => GetIntOrNull("FailedJobsCount");

        public string Finish => Properties.GetValueOrDefault("Finish", null);

        public string First => Properties.GetValueOrDefault("first", null);

        /// <summary>
        /// Первая строка контекста исполнения
        /// </summary>
        public string FirstContextLine
        {
            get
            {
                if (string.IsNullOrEmpty(Context))
                    return "";

                var c = Context.Trim();

                var index = c.IndexOf('\n');

                return index > 0 ? c.Substring(0, index).Trim() : c;
            }
        }

        public string Func => Properties.GetValueOrDefault("Func", null);

        public string Headers => Properties.GetValueOrDefault("Headers", null);

        public string Host => Properties.GetValueOrDefault("Host", null);

        public string HResultNC2012 => Properties.GetValueOrDefault("hResultNC2012", null);

        public string Ib => Properties.GetValueOrDefault("IB", null);

        public string ID => Properties.GetValueOrDefault("ID", null);

        /// <summary>
        /// Имя передаваемого интерфейса, метод которого вызывается удаленно
        /// </summary>
        public string IName => Properties.GetValueOrDefault("IName", null);

        /// <summary>
        /// Количество данных, прочитанных с диска за время вызова (в байтах)
        /// </summary>
        public long? InBytes => GetLongOrNull("InBytes");

        public string InfoBaseID => Properties.GetValueOrDefault("InfoBaseID", null);

        public string Interface => Properties.GetValueOrDefault("Interface", null);

        /// <summary>
        /// Последняя строка контекста исполнения
        /// </summary>
        public string LastContextLine
        {
            get
            {
                if (string.IsNullOrEmpty(Context))
                    return "";
                else
                {
                    var c = Context.Trim();

                    var index = c.LastIndexOf('\t');

                    if (index > 0)
                        return c.Substring(index + 1).Trim();
                    else
                        return c;
                }
            }
        }

        /// <summary>
        /// Поток является источником блокировки
        /// </summary>
        public string Lka => Properties.GetValueOrDefault("lka", null);

        /// <summary>
        /// Поток является жертвой блокировки
        /// </summary>
        public string Lkp => Properties.GetValueOrDefault("lkp", null);

        /// <summary>
        /// Номер запроса к СУБД, «кто кого заблокировал» (только для потока-жертвы блокировки). Например, ‘423’
        /// </summary>
        public string Lkpid => Properties.GetValueOrDefault("lkpid", null);

        /// <summary>
        /// Cписок номеров запросов к СУБД, «кто кого заблокировал» (только для потока-источника блокировки). Например, ‘271,273,274’
        /// </summary>
        public string Lkaid => Properties.GetValueOrDefault("lkaid", null);

        /// <summary>
        /// Массив значений из свойства lkaid
        /// </summary>
        public int[] LkaidArray => Lkaid != "" ? Lkaid.Split(',').Select(int.Parse).ToArray() : default;

        /// <summary>
        /// Номер соединения источника блокировки, если поток является жертвой, например, ‘23’
        /// </summary>
        public string Lksrc => Properties.GetValueOrDefault("lksrc", null);

        /// <summary>
        /// Время в секундах, прошедшее с момента обнаружения, что поток является жертвой. Например: ‘15’
        /// </summary>
        public string Lkpto => Properties.GetValueOrDefault("lkpto", null);

        /// <summary>
        /// Время в секундах, прошедшее с момента обнаружения, что поток является источником блокировок. Например, ‘21’
        /// </summary>
        public string Lkato => Properties.GetValueOrDefault("lkato", null);

        public string Locks => Properties.GetValueOrDefault("Locks", null);

        public LocksInfo LocksInfo => string.IsNullOrEmpty(Locks) ? null : new LocksInfo(Locks);

        public int? LogOnly => GetIntOrNull("logOnly");

        public long? Memory => GetLongOrNull("Memory");

        public long? MemoryPeak => GetLongOrNull("MemoryPeak");

        public string Method => Properties.GetValueOrDefault("Method", null);

        public int? MinDataId => GetIntOrNull("MinDataId");

        /// <summary>
        /// Имя удаленно вызываемого метода
        /// </summary>
        public string MName => Properties.GetValueOrDefault("MName", null);

        public string Module => Properties.GetValueOrDefault("Module", null);

        public string ModuleName => Properties.GetValueOrDefault("ModuleName", null);

        public string Name => Properties.GetValueOrDefault("Name", null);

        public int? Nmb => GetIntOrNull("Nmb");

        /// <summary>
        /// Количество данных, записанных на диск за время вызова (в байтах)
        /// </summary>
        public long? OutBytes => GetLongOrNull("OutBytes");

        public string Phrase => Properties.GetValueOrDefault("Phrase", null);

        public int? Pid => GetIntOrNull("Pid");

        /// <summary>
        /// План запроса, содержащегося в свойстве Sql
        /// </summary>
        public string PlanSQLText => Properties.GetValueOrDefault("planSQLText", null);

        /// <summary>
        /// Номер основного сетевого порта процесса
        /// </summary>
        public int? Port => GetIntOrNull("Port");

        /// <summary>
        /// Имя серверного контекста, который обычно совпадает с именем информационной базы
        /// </summary>
        public string PProcessName => Properties.GetValueOrDefault("p:processName", null);

        public string Prm => Properties.GetValueOrDefault("Prm", null);

        public string ProcedureName => Properties.GetValueOrDefault("ProcedureName", null);

        public string ProcessID => Properties.GetValueOrDefault("ProcessID", null);

        /// <summary>
        /// Наименование процесса
        /// </summary>
        public string ProcessName => Properties.GetValueOrDefault("ProcessName", null);

        /// <summary>
        /// Адрес процесса сервера системы «1С:Предприятие», к которому относится событие
        /// </summary>
        public string ProcURL => Properties.GetValueOrDefault("procURL", null);

        public int? Protected => GetIntOrNull("Protected");

        /// <summary>
        /// Текст запроса на встроенном языке, при выполнении которого обнаружилось значение NULL в поле, для которого такое значение недопустимо
        /// </summary>
        public string Query => Properties.GetValueOrDefault("Query", null);

        /// <summary>
        /// Перечень полей запроса, в которых обнаружены значения NULL
        /// </summary>
        public string QueryFields => Properties.GetValueOrDefault("QueryFields", null);

        public string Ranges => Properties.GetValueOrDefault("Ranges", null);

        /// <summary>
        /// Причина недоступности рабочего процесса
        /// </summary>
        public string Reason => Properties.GetValueOrDefault("Reason", null);

        /// <summary>
        /// Имена пространств управляемых транзакционных блокировок
        /// </summary>
        public string Regions => Properties.GetValueOrDefault("Regions", null);

        public string Res => Properties.GetValueOrDefault("res", null);

        public string Result => Properties.GetValueOrDefault("Result", null);

        public string RetExcp => Properties.GetValueOrDefault("RetExcp", null);

        /// <summary>
        /// Количество полученных записей базы данных
        /// </summary>
        public int? Rows => GetIntOrNull("Rows");

        /// <summary>
        /// Количество измененных записей базы данных
        /// </summary>
        public int? RowsAffected => GetIntOrNull("RowsAffected");

        /// <summary>
        /// Режим запуска процесса (приложение или сервис)
        /// </summary>
        public string RunAs => Properties.GetValueOrDefault("RunAs", null);

        public long? SafeCallMemoryLimit => GetLongOrNull("SafeCallMemoryLimit");

        /// <summary>
        /// Текст запроса на встроенном языке модели базы данных
        /// </summary>
        public string Sdbl => Properties.GetValueOrDefault("Sdbl", null);

        /// <summary>
        /// Имя рабочего сервера
        /// </summary>
        public string ServerComputerName => Properties.GetValueOrDefault("ServerComputerName", null);

        public string ServerID => Properties.GetValueOrDefault("ServerID", null);

        public string ServerName => Properties.GetValueOrDefault("ServerName", null);

        /// <summary>
        /// Номер сеанса, назначенный текущему потоку. Если текущему потоку не назначен никакой сеанс, то свойство не добавляется
        /// </summary>
        public string SessionID => Properties.GetValueOrDefault("SessionID", null);

        /// <summary>
        /// Текст оператора SQL
        /// </summary>
        public string Sql => Properties.GetValueOrDefault("Sql", null);

        /// <summary>
        /// MD5 хеш значения свойства CleanSql
        /// </summary>
        public string SqlHash => GetMd5Hash(CleanSql);

        public string SrcName => Properties.GetValueOrDefault("SrcName", null);

        public string SrcProcessName => Properties.GetValueOrDefault("SrcProcessName", null);

        public string State => Properties.GetValueOrDefault("State", null);

        /// <summary>
        /// Код состояния HTTP
        /// </summary>
        public int? Status => GetIntOrNull("Status");

        public int? SyncPort => GetIntOrNull("SyncPort");

        /// <summary>
        /// Объем занятой процессом динамической памяти на момент вывода события MEM (в байтах)
        /// </summary>
        public long? Sz => GetLongOrNull("Sz");

        /// <summary>
        /// Изменение объема динамической памяти, занятой процессом, с момента вывода предыдущего события MEM (в байтах)
        /// </summary>
        public long? Szd => GetLongOrNull("Szd");

        public string TableName => Properties.GetValueOrDefault("tableName", null);

        /// <summary>
        /// Идентификатор клиентской программы
        /// </summary>
        public string TApplicationName => Properties.GetValueOrDefault("t:applicationName", null);

        /// <summary>
        /// Идентификатор соединения с клиентом по TCP
        /// </summary>
        public int? TClientID => GetIntOrNull("t:clientID");

        /// <summary>
        /// Имя клиентского компьютера
        /// </summary>
        public string TComputerName => Properties.GetValueOrDefault("t:computerName", null);

        /// <summary>
        /// Идентификатор соединения с информационной базой
        /// </summary>
        public int? TConnectID => GetIntOrNull("t:connectID");

        /// <summary>
        /// Время вывода записи в технологический журнал
        /// **Для события ATTN содержит имя серверного процесса: rmngr или rphost
        /// </summary>
        public string Time => Properties.GetValueOrDefault("Time", null);

        public int? TotalJobsCount => GetIntOrNull("TotalJobsCount");
        /// <summary>
        /// Идентификатор активности транзакции на момент начала события:
        /// 0 ‑ транзакция не была открыта;
        /// 1 ‑ транзакция была открыта.
        /// </summary>
        public int? Trans => GetIntOrNull("Trans");

        public int? TTmpConnectID => GetIntOrNull("t:tmpConnectID");

        /// <summary>
        /// Текст информационного сообщения
        /// </summary>
        public string Txt 
        {
            get 
            {
                if (Properties.TryGetValue("Txt", out var txt))
                    return txt;
                else
                    return Properties.GetValueOrDefault("txt", null);
            }
        }

        /// <summary>
        /// Ресурс, к которому производится обращение
        /// </summary>
        public string URI => Properties.GetValueOrDefault("URI", null);

        public string UserName => Properties.GetValueOrDefault("UserName", null);

        /// <summary>
        /// Размер используемого места в хранилище, в байтах
        /// </summary>
        public long? UsedSize => GetLongOrNull("UsedSize");

        /// <summary>
        /// Имя пользователя информационной базы (если в информационной базе не определены пользователи, это свойство будет иметь значение DefUser). 
        /// Значение свойства берется из назначенного сеанса.
        /// </summary>
        public string Usr => Properties.GetValueOrDefault("Usr", null);

        /// <summary>
        /// Список соединений, с которыми идет столкновение по управляемым транзакционным блокировкам
        /// </summary>
        public string WaitConnections => Properties.GetValueOrDefault("WaitConnections", null);

        private long? GetLongOrNull(string propertyName)
        {
            if (Properties.TryGetValue(propertyName, out var value))
                return long.Parse(value);
            else
                return null;
        }

        private int? GetIntOrNull(string propertyName)
        {
            if (Properties.TryGetValue(propertyName, out var value))
                return int.Parse(value);
            else
                return null;
        }

        public static string GetMd5Hash(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            using (var cp = MD5.Create())
            {
                var src = Encoding.UTF8.GetBytes(str);
                var res = cp.ComputeHash(src);

                return BitConverter.ToString(res).Replace("-", null);
            }
        }

        private string GetCleanSql(string data)
        {
            if (string.IsNullOrEmpty(data))
                return "";
            else
            {
                // Remove parameters
                int startIndex = data.IndexOf("sp_executesql", StringComparison.OrdinalIgnoreCase);

                if (startIndex < 0)
                    startIndex = 0;
                else
                    startIndex += 16;

                int e1 = data.IndexOf("', N'@P", StringComparison.OrdinalIgnoreCase);
                if (e1 < 0)
                    e1 = data.Length;

                var e2 = data.IndexOf("p_0:", StringComparison.OrdinalIgnoreCase);
                if (e2 < 0)
                    e2 = data.Length;

                var endIndex = Math.Min(e1, e2);

                // Remove temp table names, parameters and guids
                var result = Regex.Replace(data.Substring(startIndex, endIndex - startIndex), @"(#tt\d+|@P\d+|\d{8}-\d{4}-\d{4}-\d{4}-\d{12})", "{RD}", RegexOptions.ExplicitCapture);

                return result;
            }
        }

        public override string ToString()
        {
            return EventName;
        }
    }
}