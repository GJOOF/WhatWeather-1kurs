using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Drawing;

namespace WeatherApplication
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.Size = new Size (922, 500);
        }
        string APIKey = "e8bb2307ab09037674a50226a0f9ad79"; // уникальный идентификатор, который используется для аутентификации запросов, связанных с проектом
        private void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                try
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric", TbCity.Text, APIKey); // создает ссылку,вставля название нужного города и API-ключ
                    var json = web.DownloadString(url); // вбивает выше созданную ссылку в Интернет
                    WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json); // записывает полученную по ссылке в формате JSON информацию в класс WeatherInfo
                    pic_icon.ImageLocation = "http://openweathermap.org/img/wn/" + Info.weather[0].icon + "@2x.png"; // создает ссылку иконки и выводит ее
                    lab_condtion.Text = Info.weather[0].main; // описание погоды
                    lab_detail.Text = Info.weather[0].description; // подробности
                    lab_sunset.Text = convertDateTime(Info.sys.sunset).ToShortTimeString(); // время захода Солнца за горизонт
                    lab_sunrise.Text = convertDateTime(Info.sys.sunrise).ToShortTimeString(); // время восхода Солнца
                    lab_windspeed.Text = Info.wind.speed.ToString() + " m/s"; // скорость ветра (в кметрах в час)
                    lab_pressure.Text = Info.main.pressure.ToString() + " mm"; // давление (в миллиметров ртутного столба)
                    lab_temp.Text = (Math.Ceiling(Info.main.temp).ToString() + " °C"); // температура, округлена до целой части (в градусах Цельсия)
                    lab_humidity.Text = Info.main.humidity.ToString() + " %"; // влажность
                }
                catch
                {
                    pic_icon.ImageLocation = ""; // создает ссылку иконки и выводит ее
                    lab_condtion.Text = "CITY NOT FOUND"; // описание погоды
                    lab_detail.Text = ""; // подробности
                    lab_sunset.Text = ""; // время захода Солнца за горизонт
                    lab_sunrise.Text = ""; // время восхода Солнца
                    lab_windspeed.Text = ""; // скорость ветра (в кметрах в час)
                    lab_pressure.Text = ""; // давление (в миллиметров ртутного столба)
                    lab_temp.Text = ""; // температура, округлена до целой части (в градусах Цельсия)
                    lab_humidity.Text = ""; // влажность
                }
            }
        }
        DateTime convertDateTime(long sec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            day = day.AddSeconds(sec).ToLocalTime();
            return day;
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            getWeather(); // после нажатия на кнопку поиска запускает функцию "getWeather"
        }
        private void keys(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getWeather(); // после нажатия на "Enter" запускает функцию "getWeather"
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lab_time.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start(); // запускает таймер после загрузки формы
        }
    }
}
