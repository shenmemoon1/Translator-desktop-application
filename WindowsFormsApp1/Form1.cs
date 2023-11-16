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
                // 确定输入文本的语言（假设中文为源语言）
                string sourceLanguage = "zh-CN";
                string targetLanguage = "en";

                // 如果输入文本包含英文字母，则将源语言和目标语言互换
                if (!IsChinese(text))
                {
                    sourceLanguage = "en";
                    targetLanguage = "zh-CN";
                }

                string apiUrl = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q=" + text;
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

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
        bool IsChinese(string text)
        {
            foreach (char c in text)
            {
                // 检查字符是否在中文的 Unicode 范围内
                if (c >= '\u4e00' && c <= '\u9fff')
                {
                    return true; // 包含中文字符
                }
            }
            return false; // 不包含中文字符
        }

    }
    
    
}
