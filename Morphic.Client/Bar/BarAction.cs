namespace Morphic.Client.Bar
{
    using System;
    using System.Diagnostics;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// An action for a bar item.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(TypedJsonConverter), "type")]
    public abstract class BarAction
    {
        public abstract Task<bool> Invoke();
    }

    /// <summary>
    /// A web-link action.
    /// </summary>
    [JsonTypeName("web")]
    public class BarWebAction : BarAction
    {
        [JsonProperty("data")]
        public string UrlString
        {
            // Wrapping a Uri means the URL is validated during load.
            get => this.Uri.ToString();
            set => this.Uri = new Uri(value);
        }

        public Uri Uri { get; set; }

        public override async Task<bool> Invoke()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = this.Uri.ToString(),
                UseShellExecute = true
            });

            return true;
        }
    }

    /// <summary>
    /// Action to start an application.
    /// </summary>
    [JsonTypeName("app")]
    public class BarAppAction : BarAction
    {
        [JsonProperty("data")]
        public string AppName { get; set; }

        public override async Task<bool> Invoke()
        {
            MessageBox.Show($"Opens the application {this.AppName}");
            return true;
        }
    }

    [JsonTypeName("gpii")]
    public class BarGpiiAction : BarAction
    {
        [JsonProperty("data")]
        public JObject RequestObject { get; set; }
        
        public override async Task<bool> Invoke()
        {
            ClientWebSocket socket = new ClientWebSocket();
            CancellationTokenSource cancel = new CancellationTokenSource();
            await socket.ConnectAsync(new Uri("ws://localhost:8081/pspChannel"), cancel.Token);

            string requestString = this.RequestObject.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(requestString);
            
            ArraySegment<byte> sendBuffer = new ArraySegment<byte>(bytes);
            await socket.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true,
                cancellationToken: cancel.Token);

            return true;
        }
    }
}