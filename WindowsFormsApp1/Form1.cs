using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// 导入 System.Net.Http 命名空间
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string textToTranslate = textBox1.Text;
            // 使用翻译服务将文本进行翻译
            string translatedText = await TranslateTextAsync(textToTranslate);

            // 将翻译后的结果显示在标签中
            label1.Text = translatedText;
        }
        async Task<string> TranslateTextAsync(string text)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&q=" + text;
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                Console.WriteLine(response.Content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // 解析 JSON 响应
                    JArray jsonArray = JArray.Parse(jsonResponse);
                    JArray translationArray = (JArray)jsonArray[0];

                    // 提取翻译结果
                    string translatedText = "";
                    foreach (JArray item in translationArray)
                    {
                        translatedText += item[0].ToString();
                    }

                    return translatedText;
                }
                else
                {
                    return "翻译失败";
                }
            }
            
        }
    }
}
