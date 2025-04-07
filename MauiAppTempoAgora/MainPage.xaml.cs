using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using Microsoft.Maui.Networking;
using System.Collections.Generic;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly Dictionary<string, string> descricaoTraducoes = new Dictionary<string, string>
        {
            { "clear sky", "céu limpo" },
            { "few clouds", "poucas nuvens" },
            { "scattered clouds", "nuvens dispersas" },
            { "broken clouds", "nuvens quebradas" },
            { "shower rain", "chuva de banho" },
            { "rain", "chuva" },
            { "thunderstorm", "trovoada" },
            { "snow", "neve" },
            { "mist", "névoa" }
        };

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                        if (t != null && t.lat != 0 && t.lon != 0)
                        {
                            string descricao = t.description;
                            if (descricaoTraducoes.ContainsKey(descricao))
                            {
                                descricao = descricaoTraducoes[descricao];
                            }

                            string dados_previsao = $"Latitude: {t.lat} \n" +
                                                    $"Longitude: {t.lon} \n" +
                                                    $"Nascer do Sol: {t.sunrise} \n" +
                                                    $"Por do Sol: {t.sunset} \n" +
                                                    $"Temp Máx: {t.temp_max} \n" +
                                                    $"Temp Min: {t.temp_min} \n" +
                                                    $"Descrição: {descricao} \n" +
                                                    $"Velocidade do Vento: {t.wind_speed} \n" +
                                                    $"Visibilidade: {t.visibility} \n";
                            lbl_res.Text = dados_previsao;
                        }
                        else
                        {
                            lbl_res.Text = "Cidade não encontrada.\nDigite um nome de cidade válido.";
                        }
                    }
                    else
                    {
                        await DisplayAlert("Sem Conexão", "Você está sem conexão com a internet. Verifique sua conexão e tente novamente.", "OK");
                    }
                }
                else
                {
                    lbl_res.Text = "Digite uma cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                count++;
                SemanticScreenReader.Announce($"You clicked {count} times.");
            }
        }
    }
}
