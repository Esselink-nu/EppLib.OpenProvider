using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Xml;

namespace EppLib.OpenProvider
{
    public class WebConnection : ITransport
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _schema;
        private HttpClient _client;
        private readonly int _readTimeout;

        /// <summary>
        /// Stores the response of the latest write ready to be fetched by the read command
        /// </summary>
        private string _resultBuffer;

        private readonly TraceSource _traceSource = new TraceSource("EppLib");

        public WebConnection(string host, int port, string schema = "https", int readTimeout = Timeout.Infinite)
        {
            _host = host;
            _port = port;
            _readTimeout = readTimeout;
            _schema = schema;

            _host = _host.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
                ? _host.Replace("https://", string.Empty)
                : _host;

            _host = _host.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase)
                ? _host.Replace("http://", string.Empty)
                : _host;

            _traceSource.TraceInformation($"Set connection to: {_schema}://{_host}:{_port}");
        }

        public void Connect(SslProtocols sslProtocols = SslProtocols.Tls12)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{_schema}://{_host}:{_port}"),
                Timeout = TimeSpan.FromMilliseconds(_readTimeout)
            };

            _traceSource.TraceData(TraceEventType.Verbose, 0, $"Connection set to {_client.BaseAddress} with timeout value {_client.Timeout}");
        }

        public void Disconnect()
        {
            _traceSource.TraceData(TraceEventType.Verbose, 0, $"Disconnection");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }


        /// <summary>
        /// Writes a command to the EPP server and stores the result in an internal buffer.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public void Write(XmlDocument xmlDocument)
        {
            if (_client == null)
                throw new Exception("Not connected");

            var bytes = GetBytes(xmlDocument.OuterXml);

            var traceLog = $"Write: {Beautify(bytes)}";

            var lenght = bytes.Length + 4;

            var lenghtBytes = BitConverter.GetBytes(lenght);
            Array.Reverse(lenghtBytes);

            using (var stream = new MemoryStream())
            {
                stream.Write(lenghtBytes, 0, 4);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                bytes = stream.GetBuffer();
            }

            _traceSource.TraceData(TraceEventType.Verbose, 0, $"Write: Payload size : {lenght}");
            _traceSource.TraceData(TraceEventType.Verbose, 0, traceLog);

            var post = _client.PostAsync(string.Empty, new StringContent(xmlDocument.OuterXml))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            if (!post.IsSuccessStatusCode)
            {
                throw new Exception("HTTP status = " + post.StatusCode);
            }

            _resultBuffer = post.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }


        /// <summary>
        /// Reads the internal buffer from the latest response.
        /// </summary>
        /// <returns></returns>
        public byte[] Read()
        {
            _traceSource.TraceData(TraceEventType.Verbose, 0, $"Read: {Beautify(_resultBuffer)}");
            var result = GetBytes(_resultBuffer);
            _resultBuffer = null;
            return result;
        }


        private static byte[] GetBytes(string s) => string.IsNullOrWhiteSpace(s) ? new byte[0] : Encoding.UTF8.GetBytes(s);

        public static string Beautify(byte[] bytes) => Beautify(Encoding.UTF8.GetString(bytes));
        

        public static string Beautify(string bytes)
        {
            if (string.IsNullOrWhiteSpace(bytes))
                return string.Empty;

            var doc = new XmlDocument();
            doc.LoadXml(bytes);

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine,
                NewLineHandling = NewLineHandling.Replace,
                Encoding = Encoding.UTF8
            };

            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
    }
}
