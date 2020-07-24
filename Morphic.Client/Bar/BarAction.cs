namespace Morphic.Client.Bar
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;

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
            MessageBox.Show($"Opens a browser with: {this.UrlString}");
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
}