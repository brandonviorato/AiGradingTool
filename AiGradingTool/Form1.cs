using System.Text.Json;
using System.Windows.Forms.Design;

namespace AiGradingTool
{
    public partial class Form1 : Form
    {
        private readonly OllamaApi ollamaApi;
        public Form1()
        {
            InitializeComponent();

            // Initialize HttpClient (best practice: reuse a single instance)
            var httpClient = new HttpClient();

            ollamaApi = new OllamaApi(httpClient);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // text to be sent to LLM
            string text = textBox1.Text;

            // set new message (default is hello there!)
            ollamaApi.SetMessage(text);

            try
            {
                // call the api
                var data = await ollamaApi.GetAsync<object>();

                // Serialize object to JSON for display
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                MessageBox.Show(json, "API Response");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // return home to MainPage
            MainPage newForm = new MainPage();
            newForm.Show();
        }
    }
}
